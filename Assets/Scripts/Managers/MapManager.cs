using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Theme { desert, jungle, city }

public class MapManager : NetworkBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _groundTilemap; 
    [SerializeField] private Tilemap _wallTilemap;   
    [SerializeField] private Tilemap _breakableTilemap; 

    [Header("Multiplayer Prefab Ayarları")]
    [SerializeField] private GameObject desertBreakablePrefab;
    [SerializeField] private GameObject jungleBreakablePrefab;
    [SerializeField] private GameObject cityBreakablePrefab;

    private NetworkVariable<Theme> _syncTheme = new NetworkVariable<Theme>(Theme.desert);

    private IMapFactory _activeFactory;
    private Theme _currentTheme;

    private int[,] _mapData = new int[,]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 0, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };

    public void ChooseThemeCreateMap(IMapThemeStrategy theme, Theme themeType)
    {
        _currentTheme = themeType;
        _activeFactory = theme.GetFactory();

        _groundTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
        _breakableTilemap.ClearAllTiles();
        
        DrawMap();
        Debug.Log("Map created with theme: " + themeType.ToString());
        KamerayiOrtala();
    }

    private void DrawMap()
    {
        int width = _mapData.GetLength(1);
        int length = _mapData.GetLength(0);

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileCode = _mapData[y, x];
                Vector3Int tilePozition = new Vector3Int(x, length - 1 - y, 0); 
                // Tile merkezini hesapla
                Vector3 worldPos = _groundTilemap.CellToWorld(tilePozition) + new Vector3(0.5f, 0.5f, 0);

                // Zemin her zaman çizilir (Tilemap)
                IGround groundProduct = _activeFactory.CreateGround();
                if (groundProduct.UnityTile != null) 
                    _groundTilemap.SetTile(tilePozition, groundProduct.UnityTile);

                if (tileCode == 1) // Kalıcı Duvar (Tilemap)
                {
                    IWall wallProduct = _activeFactory.CreateWall();
                    _wallTilemap.SetTile(tilePozition, wallProduct.UnityTile);
                }
                else if (tileCode == 2) // Yıkılabilir Duvar (Prefab & Network Spawn)
                {
                    // Sadece Sunucu prefab oluşturur, otomatik senkronize olur
                    if (IsServer)
                    {
                        SpawnBreakablePrefab(worldPos);
                    }
                }
            }
        }
    }

    private void SpawnBreakablePrefab(Vector3 position)
    {
        GameObject targetPrefab = _currentTheme switch
        {
            Theme.desert => desertBreakablePrefab,
            Theme.jungle => jungleBreakablePrefab,
            Theme.city => cityBreakablePrefab,
            _ => desertBreakablePrefab
        };

        if (targetPrefab != null)
        {
            GameObject brick = Instantiate(targetPrefab, position, Quaternion.identity);
            brick.GetComponent<NetworkObject>().Spawn();
        }
        Debug.Log($"Kutu oluşturuluyor: {position}");
    }
    
    public void KamerayiOrtala()
    {
        int genislik = _mapData.GetLength(1);
        int yukseklik = _mapData.GetLength(0);
        float merkezX = (genislik / 2f) - 0.5f;
        float merkezY = (yukseklik / 2f) - 0.5f;
        Camera.main.transform.position = new Vector3(merkezX, merkezY, Camera.main.transform.position.z);
        float gorunusBoyutu = Mathf.Max(genislik, yukseklik) / 2f;
        Camera.main.orthographicSize = gorunusBoyutu + 1f; 
    }

    public int GetTileCode(int x, int y)
    {
        int width = _mapData.GetLength(1);
        int length = _mapData.GetLength(0);
        if (x < 0 || x >= width || y < 0 || y >= length) return 1;
        int mapY = length - 1 - y; 
        return _mapData[mapY, x];
    }

    public override void OnNetworkSpawn()
{
    // Tema değiştiğinde hem Host hem Client bu metodu çalıştırır
    _syncTheme.OnValueChanged += (oldTheme, newTheme) => {
        ApplySyncedTheme(newTheme);
    };

    if (IsServer)
    {
        // Host, GameManager'daki aktif temayı NetworkVariable'a yazar
        _syncTheme.Value = GameManager.Instance.activeTheme;
        DrawMap();
        KamerayiOrtala();
    }
    else
    {
        // Client sonradan bağlandığında güncel temayı uygular
        ApplySyncedTheme(_syncTheme.Value);
    }
}

    private void UpdateThemeAndDraw(Theme newTheme)
    {
        // Burada Factory'yi yeni temaya göre güncelle ve Tilemap'leri boya
        // Bu metod hem Host hem Client tarafında yerel çalışır (Tilemap senkronu için)
    }

    
    private void ApplySyncedTheme(Theme newTheme)
{
    // Client'ın yerel seçimi yerine sunucudan gelen temayı kullan
    IMapThemeStrategy strategy = newTheme switch
    {
        Theme.city => new CityStrategy(),
        Theme.jungle => new JungleStrategy(),
        Theme.desert => new DesertStrategy(),
        _ => new CityStrategy()
    };

    // Haritayı sunucudan gelen strateji ile tekrar çiz
    ChooseThemeCreateMap(strategy, newTheme);
}
}
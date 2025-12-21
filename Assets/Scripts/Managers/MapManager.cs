using UnityEngine;
using UnityEngine.Tilemaps;

public enum Theme {desert, jungle, city}

public class MapManager : MonoBehaviour
{
    // inspectordan atanacak bileşenler
    [SerializeField] private Grid _grid;
    // TEK TILEMAP YERİNE ÜÇ YENİ ALAN
    [SerializeField] private Tilemap _groundTilemap; 
    [SerializeField] private Tilemap _wallTilemap;   
    [SerializeField] private Tilemap _breakableTilemap; 

    //aktif fabrika (tema seçiminden sonra atanacak)
    private IMapFactory _activeFactory;

    // Not: 1=Duvar, 0=Yürünebilir Zemin, 2=Yıkılabilir Duvar.
    private int[,] _mapData = new int[,]
    {
        // 15 sütun genişliğinde (X)
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, // 13 satır yüksekliğinde (Y)
        {1, 0, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 0, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
        {1, 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };

    public void ChooseThemeCreateMap(IMapThemeStrategy theme)
    {
        _activeFactory = theme.GetFactory();

        // Üç Tilemap'i temizle
        _groundTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
        _breakableTilemap.ClearAllTiles();
        
        DrawMap();
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

            // --- HER ZAMAN ZEMİN ÇİZ (Katman 0) ---
            IGround groundProduct = _activeFactory.CreateGround();
            if (groundProduct.UnityTile != null) 
                _groundTilemap.SetTile(tilePozition, groundProduct.UnityTile);

            // --- ÜST KATMANLARI ÇİZ ---
            if (tileCode == 1) // KALICI DUVAR
            {
                IWall wallProduct = _activeFactory.CreateWall();
                _wallTilemap.SetTile(tilePozition, wallProduct.UnityTile);
            }
            else if (tileCode == 2) // YIKILABİLİR DUVAR
            {
                IBreakableWall breakableProduct = _activeFactory.CreateBreakableWall();
                _breakableTilemap.SetTile(tilePozition, breakableProduct.UnityTile);
            }
        }
    }
}
    
    public void KamerayiOrtala()
    {
        // ... (Kamera kodunuz burada devam ediyor)
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
        // ... (GetTileCode metodunuz burada devam ediyor)
        int width = _mapData.GetLength(1);
        int length = _mapData.GetLength(0);
        if (x < 0 || x >= width || y < 0 || y >= length)
        {
            return 1; // Engel döndür
        }
        int mapY = length - 1 - y; 
        return _mapData[mapY, x];
    }
}
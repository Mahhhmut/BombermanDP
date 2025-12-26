using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [Header("Spawn Settings")]
[SerializeField] private GameObject enemyPrefab; // Düşman prefabı
[SerializeField] private Vector3 enemySpawnPos = new Vector3(1.75f, 11.48f, 0); // Koordinat
    public static GameManager Instance { get; private set; }

    private MapManager mapManager; 
    public Theme activeTheme { get; private set; } = Theme.city; 

    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ClientRpc]
public void NotifyGameOverClientRpc(string message)
{
    // Her iki tarafın konsolunda büyük harflerle görünür
    Debug.Log($"<color=yellow>*** {message} ***</color>");

    if (IsServer) StartCoroutine(ResetGameRoutine());
}

private IEnumerator RespawnPlayersRoutine()
{
    // 1. Eski Player ve Enemy'leri temizle (Despawn)
    PlayerPresenter[] oldPlayers = FindObjectsByType<PlayerPresenter>(FindObjectsSortMode.None);
    foreach (var p in oldPlayers)
    {
        if (p.NetworkObject != null && p.NetworkObject.IsSpawned)
            p.NetworkObject.Despawn();
    }

    // Sahnedeki eski düşmanları da bul ve temizle
    GameObject[] oldEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (var e in oldEnemies)
    {
        var netObj = e.GetComponent<NetworkObject>();
        if (netObj != null && netObj.IsSpawned) netObj.Despawn();
    }

    yield return new WaitForNextFrameUnit();

    // 2. Oyuncuları doğurt
    foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
    {
        Vector3 spawnPos = (client.ClientId == 0) 
            ? new Vector3(1.52f, 1.52f, 0) 
            : new Vector3(13.5f, 11.5f, 0);

        GameObject playerObj = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, spawnPos, Quaternion.identity);
        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
    }

    // 3. Düşmanı belirlenen koordinatta doğurt
    if (enemyPrefab != null)
    {
        Vector3 enemyPos = new Vector3(1.75f, 11.48f, 0);
        GameObject enemyObj = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
        enemyObj.GetComponent<NetworkObject>().Spawn();
    }
}

    // Sahne her yüklendiğinde otomatik harita çizmeyi tetikler
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "MainScene")
    {
        mapManager = FindFirstObjectByType<MapManager>();
        if (mapManager != null) LoadStartingScene();

        // Sahne ilk açıldığında veya resetlendiğinde oyuncuları doğru yerde doğurt
        if (IsServer)
        {
            // Eğer halihazırda oyuncu varsa temizle, yoksa direkt doğurt
            StartCoroutine(RespawnPlayersRoutine());
        }
    }
}

    public void SetTheme(Theme newTheme) => activeTheme = newTheme;

    public void LoadStartingScene()
    {
        IMapThemeStrategy selectedStrategy = activeTheme switch
        {
            Theme.city => new CityStrategy(),
            Theme.jungle => new JungleStrategy(),
            Theme.desert => new DesertStrategy(),
            _ => new CityStrategy()
        };

        if (mapManager != null)
            mapManager.ChooseThemeCreateMap(selectedStrategy, activeTheme);
    }

    private IEnumerator ResetGameRoutine()
    {
        // 1. Sahneyi herkes için yeniden yükle
        //  tüm bağlı client'ları MainScene'e taşır
        NetworkManager.Singleton.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);

        // 2. Sahne yüklenene kadar bekle
        yield return new WaitForSeconds(0.5f);

        // 3. Oyuncuları spawn etme işlemini başlat (Önceki mesajdaki RespawnPlayersRoutine)
        StartCoroutine(RespawnPlayersRoutine());
    }

    private void ClearOldEntities()
    {
        // Sahnedeki tüm NetworkObject'leri bul ve sil
        var allNetworkObjects = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
        foreach (var netObj in allNetworkObjects)
        {
            // GameManager'ın kendisini silmemesi için kontrol
            if (netObj != NetworkObject && netObj.IsSpawned) 
                netObj.Despawn(true);
        }
    }

    
}
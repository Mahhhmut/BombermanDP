using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
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
    // 1. Önce sahnedeki tüm eski oyuncu objelerini bul ve yok et
    PlayerPresenter[] oldPlayers = FindObjectsByType<PlayerPresenter>(FindObjectsSortMode.None);
    foreach (var p in oldPlayers)
    {
        if (p.NetworkObject != null && p.NetworkObject.IsSpawned)
            p.NetworkObject.Despawn();
    }

    // Kısa bir bekleme (Senkronizasyon için)
    yield return new WaitForNextFrameUnit();

    // 2. Yeni koordinatlara göre spawn et
    foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
    {
        // Host ise (ID 0), Client ise diğer koordinat
        Vector3 spawnPos = (client.ClientId == 0) 
            ? new Vector3(1.52f, 1.52f, 0) 
            : new Vector3(13.5f, 11.5f, 0);

        GameObject playerObj = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, spawnPos, Quaternion.identity);
        
        // Oyuncuyu ilgili ID ile ağda doğurt
        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
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
    // Not: Bu satır tüm bağlı client'ları MainScene'e taşır.
    NetworkManager.Singleton.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);

    // 2. Sahne yüklenene kadar bekle (Opsiyonel ama güvenlidir)
    yield return new WaitForSeconds(0.5f);

    // 3. Oyuncuları spawn etme işlemini başlat (Önceki mesajdaki RespawnPlayersRoutine)
    StartCoroutine(RespawnPlayersRoutine());
}

    
}
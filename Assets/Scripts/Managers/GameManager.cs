using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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

    // Sahne her yüklendiğinde otomatik harita çizmeyi tetikler
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene") // Oyun sahnesinin adı tam olarak bu olmalı
        {
            mapManager = FindFirstObjectByType<MapManager>();
            if (mapManager != null) LoadStartingScene();
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
            mapManager.ChooseThemeCreateMap(selectedStrategy);
    }
}
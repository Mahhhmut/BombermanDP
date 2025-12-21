using UnityEngine;

// Bu script'i yalnızca bir kere oluşturacağımız için Singleton deseni kullanıyoruz.
public class GameManager : MonoBehaviour
{
    // 1. Singleton Örneği (Instance)
    public static GameManager Instance { get; private set; }

    // Inspector'dan atanacak MapManager referansı
    // MapManager'ı sahnede aktif hale getiren script'i tutar.
    [SerializeField] private MapManager mapManager; 

    // 2. Oyun durumunu veya seviyesini tutmak için bir değişken
    public Theme activeTheme { get; private set; } = Theme.city; 

    void Awake()
    {
        // Singleton mantığı: Sadece tek bir kopyanın varlığını garanti eder.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Sahne değiştirildiğinde objenin yok olmamasını sağlar (Opsiyonel)
            DontDestroyOnLoad(gameObject); 
        }
    }

    void Start()
    {
        // 3. Harita Oluşturmayı Tetikleme
        if (mapManager != null)
        {
            LoadStartingScene();
        }
        else
        {
            Debug.LogError("Map Manager atanmamış! Lütfen Inspector'dan atayın.");
        }
    }
    
    // 4. Seviye Yükleme Metodu
    public void LoadStartingScene()
    {
        // MapManager'a aktif temayı ileterek harita çizimini başlatır (Client çağrısı).
        Debug.Log($"GameManager: {activeTheme} Teması ile harita oluşturuluyor.");
        mapManager.ChooseThemeCreateMap(activeTheme);
    }

    // Harita oluşturulduktan sonra oyuncuyu yerleştirme vb. metotlar buraya gelir...
}
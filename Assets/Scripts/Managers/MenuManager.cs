using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode; // Bunu eklemeyi unutma

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject themesSubMenu;

    // "Start" butonu yerine artık bu olacak (Host hem kurar hem girer)
    public void StartGameAsHost()
    {
        // Önemli: Önce sahneyi yükle, sonra host başlat
        // Veya NetworkManager.Singleton.SceneManager.LoadScene kullan
        NetworkManager.Singleton.StartHost();
        
        // Host sahneyi yüklediğinde, bağlanan client'lar otomatik olarak o sahneye çekilir
        NetworkManager.Singleton.SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    // "Join" butonu için
    public void JoinGameAsClient()
    {
        // Client sadece bağlanır, server hangi sahnedeyse oraya otomatik gider
        NetworkManager.Singleton.StartClient();
    }

    public void SelectTheme(string themeName)
    {
        if (System.Enum.TryParse(themeName, true, out Theme selected))
        {
            GameManager.Instance.SetTheme(selected);
            if (themesSubMenu != null) themesSubMenu.SetActive(false);
        }
    }

    public void ToggleThemeMenu()
    {
        if (themesSubMenu != null) themesSubMenu.SetActive(!themesSubMenu.activeSelf);
    }
}
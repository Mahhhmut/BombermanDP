using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panelleri")]
    [SerializeField] private GameObject themesSubMenu; // Inspector'dan 'ThemesSubMenu' panelini buraya sürükle

    void Start()
    {
        // Başlangıçta alt menünün kapalı olduğundan emin olalım
        if (themesSubMenu != null) 
            themesSubMenu.SetActive(false);
    }

    // "Theme" ana butonuna basınca çalışır
    public void ToggleThemeMenu()
    {
        if (themesSubMenu != null)
        {
            bool isActive = themesSubMenu.activeSelf;
            themesSubMenu.SetActive(!isActive);
        }
    }

    // Alt butonlar (City, Jungle, Desert) buna bağlı olacak
    public void SelectTheme(string themeName)
    {
        // String veriyi Enum tipine güvenli bir şekilde çeviriyoruz
        if (System.Enum.TryParse(themeName, true, out Theme selected))
        {
            GameManager.Instance.SetTheme(selected);
            
            // "GameScene" sahnesine geçiş yap
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.LogError($"Hatalı tema adı: {themeName}");
        }
    }

    // Gelecekte eklenecek Multiplayer butonu için örnek
    public void OpenMultiplayerMenu()
    {
        Debug.Log("Multiplayer menüsü yakında...");
    }
}
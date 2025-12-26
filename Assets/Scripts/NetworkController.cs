using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject menuPanel; // Başlayınca menüyü gizlemek için

    void Start()
    {
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            menuPanel.SetActive(false);
        });

        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            menuPanel.SetActive(false);
        });
    }
}
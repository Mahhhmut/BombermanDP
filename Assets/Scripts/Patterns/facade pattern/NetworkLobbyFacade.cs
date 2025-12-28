using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkLobbyFacade : MonoBehaviour
{
    private NetworkManager _networkManager => NetworkManager.Singleton;

    public void HostGame()
    {
        // Karmaşık ağ başlatma ve sahne yükleme mantığı burada gizli
        _networkManager.StartHost();
        _networkManager.SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        Debug.Log("Facade: Host başlatıldı ve sahne yükleniyor...");
    }

    public void JoinGame(string ipAddress)
    {
        // Transport ayarlarını yapma karmaşasını Facade çözer
        var transport = _networkManager.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
        _networkManager.StartClient();
        Debug.Log($"Facade: {ipAddress} adresine bağlanılıyor...");
    }
}
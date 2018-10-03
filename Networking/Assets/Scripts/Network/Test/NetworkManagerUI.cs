using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Clase test para NetworkManager
/// </summary>
public class NetworkManagerUI : MonoBehaviour
{
    #region References
    public NetworkManager NetworkManager;

    public Text PortText;
    public Text ClientText;
    public Text ConnectionText;

    #endregion
    public void StartClient()
    {
        NetworkManager.StartClient();
        ClientText.text = "Client: address=" + NetworkManager.networkAddress + " port=" + NetworkManager.networkPort;
    }

    public void StartServer()
    {
        NetworkManager.StartServer();
        PortText.text = "Server: Port=" + NetworkManager.networkPort;
    }
    public void StartHost()
    {
        NetworkManager.StartHost();
        PortText.text = "Server: Port=" + NetworkManager.networkPort;
        ClientText.text = "Client: address=" + NetworkManager.networkAddress + " port=" + NetworkManager.networkPort;
    }

    public void StopClient()
    {
        NetworkManager.StopClient();
    }

    public void StopServer()
    {
        NetworkManager.StopServer();
    }

    public void StopHost()
    {
        NetworkManager.StopHost();
    }

    public void OnNetworkAddressChanges(string newAddress)
    {
        NetworkManager.networkAddress = newAddress;
    }

    //TODO: Que funcione esto
    private void OnConnectedToServer()
    {
        //ConnectionText.gameObject.SetActive(false);
    }

}

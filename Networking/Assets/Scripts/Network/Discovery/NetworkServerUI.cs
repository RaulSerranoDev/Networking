using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// UI para el servidor. Permite iniciar/parar el Broadcast y el Server. 
/// Muestra información del Network.
/// </summary>
public class NetworkServerUI : MonoBehaviour
{
    #region References

    public NetworkDiscovery NetworkDiscovery;
    public Text IPText;
    public EnemySpawner EnemySpawner;

    #endregion References

    /// <summary>
    /// Es llamado cuando se pulsa el botón de iniciar Server.
    /// Inicializa el Broadcast y crea el Server.
    /// Muestra la IP del servidor
    /// </summary>
    public void StartServer()
    {
        NetworkDiscovery.Initialize();
        NetworkDiscovery.StartAsServer();
        NetworkManager.singleton.StartServer();

        IPText.text = "Address=" + LocalIPAddress();
    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón de salir del Server.
    /// Para el servidor y el Broadcast
    /// TODO: Destruir la escena
    /// </summary>
    public void StopServer()
    {
        NetworkDiscovery.StopBroadcast();
        NetworkManager.singleton.StopServer();
    }



    #region Auxiliar

    /// <summary>
    /// Devuelve la IP local del servidor
    /// </summary>
    /// <returns></returns>
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    #endregion Auxiliar
}

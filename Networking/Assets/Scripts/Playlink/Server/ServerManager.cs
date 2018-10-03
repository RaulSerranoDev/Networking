using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;

namespace Playlink
{
    /// <summary>
    /// UI para el servidor. Permite iniciar/parar el Broadcast y el Server. 
    /// Muestra información del Network.
    /// </summary>
    public class ServerManager : MonoBehaviour
    {
        public static ServerManager Instance;

        #region References

        public NetworkDiscovery NetworkDiscovery;       //Discovery
        public GameObject StartServerButton;            //Botón de inicio
        public Text IPText;                             //Texto que muestra la IP

        //Cuando ocurre algún error
        public GameObject StartPanel;
        public GameObject ExitPanel;

        #endregion References

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Es llamado cuando escribimos en el InputField
        /// Activa/Desactiva el botón de empezar server y guarda el nombre del partido en NetworkDiscovery.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void NamePlaced(string name)
        {
            if (name != "")
            {
                StartServerButton.SetActive(true);
                NetworkDiscovery.broadcastData = name;
            }
            else
                StartServerButton.SetActive(false);

        }

        /// <summary>
        /// Es llamado cuando se pulsa el botón de iniciar Server.
        /// Inicializa el Broadcast y crea el Server.
        /// Muestra la IP del servidor.
        /// Construye las fichas del partido y muestra información del partido en el Server
        /// </summary>
        /// <param name="matchName"></param>
        public void StartServer()
        {
            //Inicializa broadcast
            NetworkDiscovery.Initialize();
            NetworkDiscovery.StartAsServer();

            //Crea servidor
            NetworkManager.singleton.StartServer();

            //Registra al servidor para escuchar cuando un cliente se desconecta
            NetworkServer.RegisterHandler(MsgType.Disconnect, ConnectionError);

            //TODO: THIS IS FOR DEBUG
            //Muestra IP del servidor
            IPText.text = "Match name=" + NetworkDiscovery.broadcastData + '\n' + "Address=" + LocalIPAddress();

            ServerGameManager.Instance.OnStartServer(NetworkDiscovery.broadcastData);
        }

        /// <summary>
        /// Es llamado por la Network cuando algún cliente se ha desconectado del servidor
        /// </summary>
        /// <param name="netMsg"></param>
        public void ConnectionError(NetworkMessage netMsg)
        {
            StopServer();

            StartPanel.SetActive(true);
            ExitPanel.SetActive(false);

           // Debug.Log("ConnectionError");
        }

        void OnPlayerDisconnected(NetworkPlayer player)
        {
            Debug.Log("Clean up after player " + player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
        }

        /// <summary>
        /// Es llamado cuando se pulsa el botón de salir del Server.
        /// Para el servidor, el broadcast y destruye la partida
        /// </summary>
        public void StopServer()
        {
            //Acaba el juego
            ServerGameManager.Instance.OnStopServer();

            //Para broadcast
            NetworkDiscovery.StopBroadcast();

            //Para el servidor
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
}

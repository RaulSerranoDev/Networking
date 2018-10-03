using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Playlink
{
    /// <summary>
    /// Maneja el UI para el Cliente. Permite encontrar servidores y unirse a la partida.
    /// Muestra información del Network.
    /// </summary>
    public class ClientManager : MonoBehaviour
    {
        public static ClientManager Instance;

        #region References

        public NetworkDiscoveryLink NetworkDiscovery;   //Referencia al componente Broadcast

        public Dropdown IPDropdown;                     //Referencia al dropdown de Partidos
        public Button JoinServerButton;                 //Referencia al botón de unirse a servidor

        public GameObject SelectMatchPanel;                  //Referencia al panel con el dropdown de partidos

        //TODO: THIS IS FOR DEBUG
        public GameObject ExitPanel;                    //Referencia al panel para salir del servidor
        public Text ConnectionText;                     //Referencia al texto de información de la conexiñon

        #endregion References

        #region Attributes

        /// <summary>
        /// Lista de IPs encontradas en la red Local
        /// </summary>
        private List<string> IPs;

        /// <summary>
        /// Propiedad que devuelve si ha surgido algún error en la conexión
        /// </summary>
        public bool Error { get; private set; }

        #endregion Attributes

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Error = false;

            //Creación de la lista
            IPs = new List<string>();
        }

        /// <summary>
        /// Es llamado cuando se pulsa el botón de Escuchar Broadcast
        /// Empieza a escuchar señales de broadcast e inicializa el cliente
        /// </summary>
        public void StartBroadcast()
        {
            NetworkDiscovery.Initialize();
            NetworkDiscovery.StartAsClient();

            //Se suscribe al evento de cuando el cliente recibe una señal de Broadcast del servidor
            NetworkDiscovery.OnReceivedDataCallback += OnReceivedBroadcast;
        }

        /// <summary>
        /// Es llamado cuando se pulsa el botón de Dejar de Escuchar Broadcast
        /// Deja de escuchar señales de broadcast y vuelve al menú principal
        /// Reinicia el menú de Listen
        /// </summary>
        public void StopBroadcast()
        {
            NetworkDiscovery.StopBroadcast();

            ResetMatchList();
        }

        /// <summary>
        /// Es llamado cuando el cliente recibe una señal de Broadcast del servidor.
        /// Recibe la IP del servidor encontrado.
        /// Comprueba si la IP no había sido encontrada y si es nueva, la añade a la lista y al dropdown
        /// </summary>
        /// <param name="IP"></param>
        private void OnReceivedBroadcast(string data, string IP)
        {
            if (!IPs.Contains(IP))
            {
                IPs.Add(IP);
                Dropdown.OptionData optionData = new Dropdown.OptionData(data);
                IPDropdown.options.Add(optionData);
            }
        }

        /// <summary>
        /// Es llamado cuando seleccionamos un nuevo valor en el IPDropdown.
        /// Si seleccionamos un valor válido(IP), muestra un botón para conectarse
        /// </summary>
        /// <param name="index"></param>
        public void SelectedIP(int index)
        {
            //Si el index es 0, el valor seleccionado es "Select IP"
            if (index > 0)
            {
                NetworkManager.singleton.networkAddress = IPs[index - 1];
                JoinServerButton.gameObject.SetActive(true);
            }
            else
                JoinServerButton.gameObject.SetActive(false);

        }

        /// <summary>
        /// Es llamado cuando pulsamos el botón de Play.
        /// Empieza el cliente y registra al componente para escuchar si hay algún fallo en la conexión.
        /// Modifica el texto de info para mostrar la IP y el puerto
        /// </summary>
        public void StartClient()
        {
            Error = false;

            //Inicializa como cliente
            NetworkClient client = NetworkManager.singleton.StartClient();

            //Registra el componente para escuchar si hay algún fallo en la conexión, si ocurre algún fallo, se llamará al método "ConnectionError"
            client.RegisterHandler(MsgType.Disconnect, ConnectionError);

            //Muestra info de la Network
            ConnectionText.text = "Client: address=" + NetworkManager.singleton.networkAddress + " port=" + NetworkManager.singleton.networkPort;

            ClientGameManager.Instance.JoinMatch();
        }

        /// <summary>
        /// Es llamado cuando el pulsamos el botón de Exit.
        /// Desconecta al cliente del servidor y reinicia el UI del cliente
        /// </summary>
        public void StopClient()
        {
            //Desconecta al cliente del servidor
            NetworkManager.singleton.StopClient();

            // Reinicia la lista de Matchs y el dropdown
            ResetMatchList();

            //Reinicio del UI
            SelectMatchPanel.gameObject.SetActive(true);
            JoinServerButton.gameObject.SetActive(false);

            ExitPanel.gameObject.SetActive(false);

            //Acaba el partido
            ClientGameManager.Instance.ExitMatch();
        }

        /// <summary>
        /// Reinicia la lista de Matchs y el dropdown
        /// </summary>
        private void ResetMatchList()
        {
            //Reinicio de las listas. Pueden haber IPs que ya no estén disponibles
            IPs.Clear();
            IPDropdown.ClearOptions();

            Dropdown.OptionData optionData = new Dropdown.OptionData("Select IP");
            IPDropdown.options.Add(optionData);
            IPDropdown.value = 0;
        }

        /// <summary>
        /// Es llamado por la Network cuando ha habido algún error al conectarse al servidor.
        /// Por ejemplo, cuando la IP a la que se conecta ya no existe.
        /// Desconecta al cliente del servidor y reinicia el UI
        /// </summary>
        /// <param name="netMsg"></param>
        public void ConnectionError(NetworkMessage netMsg)
        {
            Debug.Log("ConnectionError");
            Error = true;
            StopClient();
        }
    }
}
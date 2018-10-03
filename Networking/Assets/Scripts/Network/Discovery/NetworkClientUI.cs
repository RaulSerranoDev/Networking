using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Maneja el UI para el Cliente. Permite encontrar servidores y unirse a la partida.
/// Muestra información del Network.
/// </summary>
public class NetworkClientUI : MonoBehaviour
{
    #region References

    public NetworkDiscoveryExtra NetworkDiscovery;
    public Dropdown IPDropdown;
    public Button PlayButton;
    public GameObject ExitPanel;
    public GameObject ListenPanel;
    public Text ConnectionText;

    #endregion References

    #region Attributtes

    /// <summary>
    /// Lista de IPs encontradas en la red Local
    /// </summary>
    public List<string> IPs { get; protected set; }

    #endregion Attributtes

    private void Start()
    {
        IPs = new List<string>();

        //Se suscribe al evento de cuando el cliente recibe una señal de Broadcast del servidor
        NetworkDiscovery.OnReceivedIPCallback += OnReceivedIP;
    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón de Escuchar Broadcast
    /// Empieza a escuchar señales de broadcast e inicializa el cliente
    /// </summary>
    public void StartBroadcast()
    {
        NetworkDiscovery.Initialize();
        NetworkDiscovery.StartAsClient();

        //TODO: NO SE VE
        ResetListenUI();
    }

    /// <summary>
    /// Es llamado cuando se pulsa el botón de Dejar de Escuchar Broadcast
    /// Deja de escuchar señales de broadcast y vuelve al menú principal
    /// </summary>
    public void StopBroadcast()
    {
        NetworkDiscovery.StopBroadcast();
    }

    /// <summary>
    /// Es llamado cuando el cliente recibe una señal de Broadcast del servidor.
    /// Recibe la IP del servidor encontrado.
    /// Comprueba si la IP no había sido encontrada y si es nueva, la añade a la lista
    /// </summary>
    /// <param name="IP"></param>
    private void OnReceivedIP(string IP)
    {
        if (!IPs.Contains(IP))
        {
            IPs.Add(IP);
            Dropdown.OptionData optionData = new Dropdown.OptionData(IP);
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
            PlayButton.gameObject.SetActive(true);
        }
        else
            PlayButton.gameObject.SetActive(false);

    }

    /// <summary>
    /// Es llamado cuando pulsamos el botón de Play.
    /// Empieza el cliente y registra al componente para escuchar si hay algún fallo en la conexión.
    /// Modifica el texto de info para mostrar la IP y el puerto
    /// TODO: Cuando se consigue conectar, es cuando se cambia el texto
    /// </summary>
    public void StartClient()
    {
        //Inicializa como cliente
        NetworkClient client = NetworkManager.singleton.StartClient();

        //Registra el componente para escuchar si hay algún fallo en la conexión, si ocurre algún fallo, se llamará al método "ConnectionError"
        client.RegisterHandler(MsgType.Disconnect, ConnectionError);
         
        //Muestra info de la Network
        ConnectionText.text = "Client: address=" + NetworkManager.singleton.networkAddress + " port=" + NetworkManager.singleton.networkPort;
    }

    /// <summary>
    /// Es llamado cuando el pulsamos el botón de Exit.
    /// Desconecta al cliente del servidor y reinicia el UI del cliente
    /// </summary>
    public void StopClient()
    {
        //Desconecta al cliente del servidor
        NetworkManager.singleton.StopClient();

        ResetListenUI();
    }

    /// <summary>
    /// Reinicia el UI del cliente para volver a buscar nuevas señales de Broadcast
    /// </summary>
    private void ResetListenUI()
    {
        //Reinicio de las listas. Pueden haber IPs que ya no estén disponibles
        IPs.Clear();
        IPDropdown.ClearOptions();

        //Reinicio del UI
        PlayButton.gameObject.SetActive(false);
        ListenPanel.gameObject.SetActive(true);
        ExitPanel.gameObject.SetActive(false);

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
        StopClient();
    }


}

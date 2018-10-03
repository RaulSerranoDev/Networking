using UnityEngine.Networking;

/// <summary>
/// Ampliación del componente de Unity NetworkDiscovery.
/// La ampliación permite suscribir a otros componentes al evento de cuando el Cliente recibe el Broadcast del Servidor.
/// </summary>
public class NetworkDiscoveryExtra : NetworkDiscovery
{
    #region Callback
    /*Callback que es incovado cuando recibe el cliente una señal del servidor*/
    public delegate void OnReceivedIP(string IP);
    public OnReceivedIP OnReceivedIPCallback;
    #endregion

    /// <summary>
    /// Es llamado desde el servidor al cliente, cuando el cliente encuentra su señal
    /// Informa a todos los componentes susbritos al Callback con la IP del Servidor encontrado
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="data"></param>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        OnReceivedIPCallback.Invoke(fromAddress.Split(':')[3]);
    }
}
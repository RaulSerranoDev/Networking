using UnityEngine.Networking;

/// <summary>
/// Extensión de Network Discovery. Permite suscribirse a diferentes métodos al evento OnReceivedDataCallback, que es llamado cuando
/// el cliente recibe una señal de broadcast.
/// </summary>
public class NetworkDiscoveryLink : NetworkDiscovery
{
    #region Callback
    /*Callback que es invocado cuando recibe el cliente una señal del servidor*/
    public delegate void OnReceivedData(string data, string IP);
    public OnReceivedData OnReceivedDataCallback;
    #endregion

    /// <summary>
    /// Es llamado desde el servidor al cliente, cuando el cliente encuentra su señal
    /// Informa a todos los componentes suscritos al Callback con la data del Servidor encontrado
    /// </summary>
    /// <param name="fromAddress"></param>
    /// <param name="data"></param>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        OnReceivedDataCallback.Invoke(data,fromAddress.Split(':')[3]);
    }

}
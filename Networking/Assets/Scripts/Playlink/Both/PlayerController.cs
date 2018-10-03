using UnityEngine;
using UnityEngine.Networking;

namespace Playlink
{
    /// <summary>
    /// Hereda de NetworkBehaviour, que hereda de MonoBehaviour.
    /// Scripts que se añaden a gameObjects que necesitan utilizar utilidades network, debe heredar de esta.
    /// Puente de conexión entre el servidor y el cliente
    /// </summary>
    public class PlayerController : NetworkBehaviour
    {
        /// <summary>
        /// Manda un mensaje al servidor
        /// La función es llamada por el cliente, pero ejecutada por el Server.
        /// Solo pueden lanzarse Commands desde el local Player.
        /// La función tiene que empezar por Cmd
        /// </summary>
        [Command]
        public void CmdPlayerStart(string name)
        {
            ServerGameManager.Instance.OnPlayerStart(name);
        }

        [Command]
        public void CmdPlayerFinish(int totalpoints)
        {
            ServerGameManager.Instance.OnPlayerFinish(totalpoints);
        }
  
        /// <summary>
        /// Esta función es llamada solo en el cliente cuando se conecta el jugador
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            ClientGameManager.Instance.PlayerController = this;
        }

        /// <summary>
        /// Esta función es llamada solo en el servidor cuando se conecta el jugador
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();

            ServerGameManager.Instance.PlayerController = this;
        }

        
    }
}

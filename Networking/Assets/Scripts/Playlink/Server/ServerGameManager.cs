using UnityEngine;
using UnityEngine.UI;

namespace Playlink
{
    /// <summary>
    /// Controla el UI durante la partida. Muestra puntuaciones y permite acabar la partida
    /// </summary>
    public class ServerGameManager : MonoBehaviour
    {       
        public static ServerGameManager Instance;

        #region References
     
        public GameObject ServerGamePanel;
        public Text MatchNameText;
        public Text Player1Text;
        public Text Player2Text;

        public Button ExitGameButton;

        #endregion References

        public PlayerController PlayerController { get; set; }

        private MatchTab playerInfo1;
        private MatchTab playerInfo2;

        private int index;  //Guarda quién está jugando actualmente

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Es llamado cuando es creado el servidor con nombre
        /// Inicializa datos de partido.
        /// Crea el UI de información del partido
        /// </summary>
        public void OnStartServer(string serverID)
        {
            index = -1;

            playerInfo1 = new MatchTab(serverID);
            playerInfo2 = new MatchTab(serverID);

            //Inicialización UI del juego
            ServerGamePanel.SetActive(true);

            MatchNameText.text = serverID;
            Player1Text.text = Player2Text.text = "";

        }

        /// <summary>
        /// Es llamado cuando se para el servidor (Se pulsa el boton de salir o ocurre un error)
        /// </summary>
        public void OnStopServer()
        {
            //Destruye la partida
            ServerGamePanel.SetActive(false);

            if (PlayerController != null)
                Destroy(PlayerController.gameObject);

        }


        /// <summary>
        /// Es llamado cuando el cliente pulsa el botón de iniciar partido
        /// </summary>
        public void OnPlayerStart(string playerName)
        {
            index++;
            //Player 1
            if (index == 0)
            {
                Player1Text.text = playerName + " is playing";
                playerInfo1.PlayerName = playerName;
            }
            //Player 2
            else 
            {
                Player2Text.text = playerName + " is playing";
                playerInfo2.PlayerName = playerName;
            }
        }

        /// <summary>
        /// Es llamado cuando a un jugador se le ha acabado el tiempo y por la tanto su turno
        /// </summary>
        /// <param name="finalPoints"></param>
        public void OnPlayerFinish(int finalPoints)
        {
            //Player 1
            if (index == 0)
            {
                Player1Text.text = "Score " +playerInfo1.PlayerName + ": "+ finalPoints;
                playerInfo1.Score = finalPoints;

            }
            //Player 2
            else if (index == 1)
            {
                Player2Text.text = "Score " + playerInfo2.PlayerName + ": " + finalPoints;
                playerInfo2.Score = finalPoints;

                //Acaba el partido
                ExitGameButton.gameObject.SetActive(true);
            }
        }


    }
}

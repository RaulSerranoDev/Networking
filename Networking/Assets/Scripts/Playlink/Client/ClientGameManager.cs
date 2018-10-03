using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Playlink
{
    /// <summary>
    /// Encargado del comportamiento del minijuego del cliente
    /// </summary>
    public class ClientGameManager : MonoBehaviour
    {
        public static ClientGameManager Instance;

        #region Inspector

        /// <summary>
        /// Duración del minijuego
        /// </summary>
        public int GameTime;

        #endregion Inspector

        #region References

        public GameObject StartGamePanel;
        public GameObject GamePanel;
        public GameObject GameOverPanel;

        public Text ScoreText;
        public Text TimeText;

        public GameObject StartPlayButton;
        public InputField NameInputField;

        #endregion References

        #region Properties

        public PlayerController PlayerController { get; set; }

        private string playerName;
        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                StartPlayButton.SetActive(playerName != "");
            }
        }

        #endregion Properties

        #region Private variables

        private int timeLeft;
        private int score;

        private int currentPlayerIndex;

        #endregion Private variables

        private void Awake()
        {
            Instance = this;
        }

        public void JoinMatch()
        {
            currentPlayerIndex = -1;
        }

        public void StartMatch()
        {
            currentPlayerIndex++;
            timeLeft = GameTime;
            score = 0;

            ScoreText.text = "Score: " + score;

            PlayerController.CmdPlayerStart(playerName);

            //Empieza a contar el tiempo restante
            StartCoroutine("TimeCount");
        }

        /// <summary>
        /// Corrutina que avanza el contador del tiempo
        /// Controla cuando se acaba el juego y manda el mensaje al servidor cuando termina
        /// </summary>
        /// <returns></returns>
        IEnumerator TimeCount()
        {
            while (timeLeft > 0 && !ClientManager.Instance.Error)
            {
                timeLeft--;
                TimeText.text = "Time left: " + timeLeft;

                yield return new WaitForSeconds(1.0f);
            }

            if (!ClientManager.Instance.Error)
            {
                PlayerController.CmdPlayerFinish(score);

                GamePanel.SetActive(false);

                //Tiene que jugar el siguiente jugador
                if (currentPlayerIndex == 0)
                {
                    StartGamePanel.SetActive(true);
                    NameInputField.text = "";
                    PlayerName = "";
                }

                //Ha acabado el partido
                else
                    GameOverPanel.SetActive(true);

            }
        }

        /// <summary>
        /// Es llamado cuando pulsamos alguno de los botones de juego.
        /// Aumenta el número de puntos conseguidos.
        /// </summary>
        public void AddScore()
        {
            score++;
            ScoreText.text = "Score: " + score;
        }

        public void ExitMatch()
        {
            //Reinicio game
            StartGamePanel.SetActive(false);
            GamePanel.SetActive(false);
        }

    }
}
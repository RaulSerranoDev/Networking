
namespace Playlink
{
    /// <summary>
    /// Estructura para guardar los datos de un jugador de una partida
    /// </summary>
    public class MatchTab
    {
        public MatchTab(string matchName)
        {
            IDMatch = matchName;
            PlayerName = "";
            Score = 0;
        }

        /// <summary>
        /// Identificador del partido
        /// </summary>
        public string IDMatch;

        /// <summary>
        /// Nombre del jugador
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// Puntos conseguidos
        /// </summary>
        public int Score;

    }
}
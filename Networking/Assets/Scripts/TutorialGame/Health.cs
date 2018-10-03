using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    /// <summary>
    /// Referencia a la barra de vida
    /// </summary>
    public RectTransform HealthBar;

    /// <summary>
    /// Vida máxima del jugador
    /// </summary>
    public const int MAXHEALTH = 100;

    /// <summary>
    /// Vida actual del jugador.
    /// Los cambios a la vida solo tienen que realizarse por parte del Server, y estos cambios se sincronizan en los clientes. Esto es llamado Server Authority.
    /// Las variables que se sincronizan en el servidor, son indicadas son [SyncVar].
    /// (hook = "OnChangeHealth"). Esto hace que cuando se modifica este valor, automáticamente se llama a la función
    /// </summary>
    [SyncVar(hook = "OnChangeHealth")]
    public int CurrentHealth = MAXHEALTH;

    /// <summary>
    /// Para marcar la diferencia entre players y enemigos
    /// </summary>
    public bool DestroyOnDeath;

    /// <summary>
    /// Array de posiciones de Spawn
    /// </summary>
    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if (isLocalPlayer)
        {
            //Encontramos todos los spawns y los guardamos en un array
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    /// <summary>
    /// Hace daño al personaje y detecta si muere
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        //Solo se hace daño desde el servidor
        if (!isServer)
            return;

        CurrentHealth -= amount;

        //Si muere, respawnea
        if (CurrentHealth <= 0)
        {
            //Si es enemigo
            if (DestroyOnDeath)
                Destroy(gameObject);

            //Si es jugador
            else
            {
                CurrentHealth = MAXHEALTH;
                RpcRespawn();
            }
        }
    }

    /// <summary>
    /// Esta función es llamada cuando se modifica el valor de CurrentHealth. En el servidor y en todos los clientes
    /// </summary>
    /// <param name="currentHealth"></param>
    private void OnChangeHealth(int health)
    {
        HealthBar.sizeDelta = new Vector2(health, HealthBar.sizeDelta.y);

    }

    /// <summary>
    /// Llamadas ClientRpc se pueden lanzar desde cualquier objeto spawneado en el servidor con una NetworkIdentity
    /// Es lo contrario a Command. Esta función es llamada desde el servidor y ejecutada en el ciente.
    /// Necesita el prefijo Rpc
    /// Necesitamos que sea una llamada en el cliente, porque si el servidor mueve al jugador, el cliente tiene autoridad en su transform debido a NetworkTransform
    /// </summary>
    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick a spawn point at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }
}

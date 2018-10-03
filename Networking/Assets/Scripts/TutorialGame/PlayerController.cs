using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Hereda de NetworkBehaviour, que hereda de MonoBehaviour.
/// Scripts que se añaden a gameObjects que necesitan utilizar utilidades network, debe heredar de esta.
/// </summary>
public class PlayerController : NetworkBehaviour
{
    public GameObject BulletPrefab;
    public Transform BulletSpawn;

    void Update()
    {
        //Solo se procesa el input del jugador local
        //El jugador local es el que ha sido creado por el networkManager
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    /// <summary>
    /// Dispara una bala.
    /// La función es llamada por el cliente, pero ejecutada por el Server.
    /// Solo pueden lanzarse Commands desde el local Player.
    /// La función tiene que empezar por Cmd
    /// </summary>
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            BulletPrefab,
            BulletSpawn.position,
            BulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        /* Crea el Gameobject en el servidor y en todos los clientes conectados.
         Es manejado por el servidor y manda los mensajes correspondientes a los clientes.*/
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    /// <summary>
    /// Esta función es llamada solo desde el cliente Local al inicio
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }


}


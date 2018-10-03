using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Se destruye en contacto y hace daño al jugador con el que choca.
    /// Como la bala es manejada por NetworkManager. Cuando se destruye, también es destruida en todos los clientes.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
            health.TakeDamage(10);

        Destroy(gameObject);
    }
}
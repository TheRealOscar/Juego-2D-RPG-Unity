using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb; // Referencia al Rigidbody2D del enemigo
    private bool isKnockback = false; // Para saber si el enemigo está en retroceso
    [SerializeField] private float knockbackForce = 5f; // Fuerza del retroceso
    [SerializeField] private float knockbackDuration = 0.5f; // Duración del retroceso

    private void Awake()
    {
        // Obtener el Rigidbody2D del enemigo
        rb = GetComponent<Rigidbody2D>();
    }

    // Este método se llama para aplicar el retroceso
    public void ApplyKnockback(Vector2 direction)
    {
        if (!isKnockback)
        {
            isKnockback = true;
            // Aplicar la fuerza en la dirección opuesta al golpe (dirección invertida)
             rb.linearVelocity = Vector2.zero;  // Detenemos el movimiento del enemigo al aplicar el retroceso
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
            // Llamar a la corutina para manejar la duración del retroceso
            StartCoroutine(EndKnockback());
        }
    }

    // Coroutine que termina el retroceso después de un tiempo
    private IEnumerator EndKnockback()
    {
        // Esperar el tiempo de retroceso
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;

        //Detener cualquier movimiento posterior del enemigo al final del retroceso
        rb.linearVelocity = Vector2.zero;
    }
    // Método para comprobar si el retroceso está activo
    public bool IsKnockbackActive()
    {
        return isKnockback;
    }
}

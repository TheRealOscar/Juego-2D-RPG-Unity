using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [SerializeField] private int health = 100; // Salud inicial del enemigo
    [SerializeField] private GameObject deathAnimation;
    //[SerializeField] private EnemyKnockback knockbackScript; // Referencia al script de retroceso

    private Rigidbody2D rb; // Rigidbody2D del enemigo
    private Flash flash;

    void Awake()
    {
        flash = GetComponent<Flash>();
        rb = GetComponent<Rigidbody2D>();
        //knockbackScript = GetComponent<EnemyKnockback>();
    }

    // Método para recibir daño desde el sistema de ataque del jugador
    public void TakeDamage(int damage)
    {
        /*if (knockbackScript.IsKnockbackActive())  // Verificar si el enemigo ya está en retroceso
        {
            return;  // No aplicar el daño si está en retroceso
        }*/
        health -= damage; // Resta el daño a la salud del enemigo
        Debug.Log($"El enemigo ha recibido {damage} de daño. Salud restante: {health}");
        StartCoroutine(flash.FlashRoutine());

        if (health <= 0)
        {
            health = 0;
            Die(); // Llama al método para manejar la muerte
        } 
        /*
        else
        {
            knockbackScript.ApplyKnockback(knockbackDirection); // Llama al retroceso
        }*/
    }

    // Método para manejar la muerte del enemigo
    public void Die()
    {
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Debug.Log("El enemigo ha sido derrotado.");
        Destroy(gameObject); // Destruye al enemigo al morir
    }
}

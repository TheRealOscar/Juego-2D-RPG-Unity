using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] private int health = 100; // Salud inicial del jugador
     private Flash flash;

    void Awake()
    {
        flash = GetComponent<Flash>();
    }

    // Método para recibir daño desde el sistema de ataque del enemigo
    public void TakeDamage(int damage)
    {
        health -= damage; // Resta el daño a la salud del jugador
        Debug.Log($"El jugador ha recibido {damage} de daño. Salud restante: {health}");
        StartCoroutine(flash.FlashRoutine());
        if (health <= 0)
        {
            health = 0;
            Die(); // Llama al método para manejar la muerte
        }
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        Debug.Log("El jugador ha sido derrotado.");
        // Aquí podrías reiniciar el nivel, mostrar una pantalla de derrota, etc.
    }
}

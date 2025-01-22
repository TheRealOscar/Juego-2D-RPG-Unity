using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10; // Daño fijo del ataque

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            PlayerDamageReceiver player = collision.GetComponent<PlayerDamageReceiver>();
            if (player != null)
            {
                player.TakeDamage(attackDamage); // Aplica el daño al jugador
            }
        }
    }
}

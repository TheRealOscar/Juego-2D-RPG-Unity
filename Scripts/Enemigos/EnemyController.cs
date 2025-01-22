using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private Transform target;
    public Transform homePos;
    [SerializeField] private float speed;
    [SerializeField] private float maxRange;
    [SerializeField] private float minRange;
    [SerializeField] private float attackRange;

    [SerializeField] private GameObject attackZoneUp; // Zona de ataque arriba
    [SerializeField] private GameObject attackZoneDown; // Zona de ataque abajo
    [SerializeField] private GameObject attackZoneLeft; // Zona de ataque izquierda
    [SerializeField] private GameObject attackZoneRight; // Zona de ataque derecha

    private float ultimoX;
    private float ultimoY;
    private CapsuleCollider2D playerCollider;

    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = Object.FindFirstObjectByType<PlayerController>().transform;
        playerCollider = target.GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= maxRange && distanceToPlayer > minRange)
        {
            FollowPlayer();
        }
        else if (distanceToPlayer <= minRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer > maxRange)
        {
            GoHome();
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        ultimoX = direction.x;
        ultimoY = direction.y;
        animator.SetBool("isMoving", true);
        animator.SetFloat("moveX", ultimoX);
        animator.SetFloat("moveY", ultimoY);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void GoHome()
    {
        Vector2 direction = (homePos.position - transform.position).normalized;
        ultimoX = direction.x;
        ultimoY = direction.y;
        animator.SetFloat("moveX", ultimoX);
        animator.SetFloat("moveY", ultimoY);
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, homePos.position) < 0.1f)
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetFloat("ultimoX", ultimoX);
            animator.SetFloat("ultimoY", ultimoY); animator.SetBool("isMoving", false);
            animator.SetTrigger("attack");

            // Desactivar todas las zonas de ataque 
            attackZoneUp.SetActive(false);
            attackZoneDown.SetActive(false);
            attackZoneLeft.SetActive(false);
            attackZoneRight.SetActive(false);

            // Activar la zona de ataque correspondiente 
            if (Mathf.Abs(ultimoY) > Mathf.Abs(ultimoX))
            {
                if (ultimoY > 0.1f) // Arriba 
                {
                    attackZoneUp.SetActive(true);
                }
                else if (ultimoY < -0.1f) // Abajo 
                {
                    attackZoneDown.SetActive(true);
                }
            }
            else
            {
                if (ultimoX > 0.1f) // Derecha 
                {
                    attackZoneRight.SetActive(true);
                }
                else if (ultimoX < -0.1f) // Izquierda 
                {
                    attackZoneLeft.SetActive(true);
                }
            } 

        }
    }

     // Método llamado al final de la animación de ataque para resetear el estado
    public void OnAttackAnimationComplete()
    {
        isAttacking = false; // Permite al enemigo atacar de nuevo
        // Desactiva las zonas de ataque
        attackZoneUp.SetActive(false);
        attackZoneDown.SetActive(false);
        attackZoneLeft.SetActive(false);
        attackZoneRight.SetActive(false);
    }
  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minRange);
    }
}

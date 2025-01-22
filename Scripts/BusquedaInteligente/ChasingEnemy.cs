using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyHealth
{
    [Header("ChasingEnemy parameters")]
    public float speed; // Velocidad de movimiento

    List<Node> path; // Ruta calculada por el sistema de PathFinding
    Vector3 destination = Vector3.zero; // Próximo destino
    bool destinationReached = true; // Indica si alcanzó el destino actual

    PlayerController player; // Referencia al jugador
    public Node[][] grid; // Cuadrícula de nodos usada para la navegación

    Animator animator; // Control de animaciones
    Vector2 direction; // Dirección actual del enemigo

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        player = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        // Continuar el movimiento hacia el próximo destino
        if (!destinationReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                destinationReached = true;
                FindNextStep();
            }
        }
    }

    private void FindNextStep()
    {
        // Solicitar el siguiente paso al PathFindingManager
        PathFindingManager.Instance.FindNextStepCoroutine(
            MoveToNextStep, transform.position, player.transform.position, grid);

        CancelInvoke("FindNextStep");
        Invoke("FindNextStep", 0.5f); // Reintentar periódicamente
    }

    private void MoveToNextStep(List<Node> path)
    {
        this.path = path;

        if (path == null || path.Count == 0)
        {
            destinationReached = true;
            Invoke("FindNextStep", 0.1f);
        }
        else
        {
            NextInPath();
        }
    }

    private void NextInPath()
    {
        if (path.Count != 0)
        {
            // Actualizar el destino al siguiente nodo
            destination = path[path.Count - 1].worldPosition;

            // Ajustar dirección y animaciones
            DirectionTowardsDestination();
            Animations();

            destinationReached = false;
            //path.RemoveAt(path.Count - 1); // Eliminar el nodo alcanzado
        }
        else
        {
            destinationReached = true;
            Invoke("FindNextStep", 0.1f);
        }
    }

    private void DirectionTowardsDestination()
    {
        // Calcular la dirección hacia el destino
        Vector3 direction = destination - transform.position;
        if (direction == Vector3.zero)
        {
            this.direction = Vector2.zero;
        }
        direction.Normalize();

        // Ajustar dirección a los ejes principales o diagonales
        Vector2[] possibleDirections = new Vector2[]
        {
                Vector2.up, 
                Vector2.down, 
                Vector2.left, 
                Vector2.right,
                new Vector2(1, 1).normalized, 
                new Vector2(1, -1).normalized,
                new Vector2(-1, 1).normalized, 
                new Vector2(-1, -1).normalized
        };

        float maxDot = -Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < possibleDirections.Length; i++)
        {
            float dot = Vector2.Dot(direction, possibleDirections[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestIndex = i;
            }
        }

        this.direction = possibleDirections[closestIndex];
    }

    private void Animations()
    {
        // Actualizar las animaciones según la dirección
        if (direction.magnitude != 0)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.Play("Walk");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    public override void StopBehaviour()
    {
        // Detener el movimiento
        destination = Vector3.zero;
        direction = Vector2.zero;
        Animations();
        destinationReached = true;
        CancelInvoke("FindNextStep");
    }

    public override void ContinueBehaviour()
    {
        // Reanudar el comportamiento
        FindNextStep();
    }
}

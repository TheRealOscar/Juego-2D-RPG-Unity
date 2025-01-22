using System.Collections;
using UnityEngine;

public class RandomPatrol : EnemyHealth
{
    [Header("RandomPatrol parameters")]
    public float speed;
    public float minPatrolTime;
    public float maxPatrolTime;
    public float minWaitTime;
    public float maxWaitTime;

    Animator animator;

    Vector2 direction;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        //StartCoroutine(Patrol());
        
        direction = RandomDirection();
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    IEnumerator Patrol()
    {
        direction = RandomDirection();
        Animations();
        yield return new WaitForSeconds(Random.Range(minPatrolTime, maxPatrolTime));

        // Pausa en el movimiento
        direction = Vector2.zero;
        Animations();
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        // Reiniciar la patrulla
        StartCoroutine(Patrol());
    }

    private Vector2 RandomDirection()
    {
        int x = Random.Range(0, 8);

        return x switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            4 => new Vector2(1, 1),
            5 => new Vector2(1, -1),
            6 => new Vector2(-1, 1),
            _ => new Vector2(-1, -1),
        };
    }

    private void Animations()
    {
        if (direction.magnitude != 0)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.Play("Walk");

        }
        else animator.Play("Idle");

        rigidBody.linearVelocity = direction.normalized * speed;

    }

    public override void StopBehaviour()
    {
        StopAllCoroutines(); // Reiniciar la patrulla
        direction = Vector2.zero;
        Animations();
    }

    public override void ContinueBehaviour()
    {
        StartCoroutine(Patrol());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    [Header("EnemyHP parameters")]
    [SerializeField] protected GameObject deathAnimation;

    public int maxHp = 5;
    public int hp = 5;
    protected Flash flash;

    public float knockbakStrength = 2f;
    protected float knockbackTime = 0.3f;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;

    // Variables de WaitAndChaseEnemy
    protected Vector2 initialPosition;
    
    public virtual void Awake() 
    {
        flash = GetComponent<Flash>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
        

        hp = maxHp;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Vector3 hitPosition = collision.transform.position;
            hp--;
            if (hp <= 0){
                DetectDeath();

            }
            StopBehaviour();
            StartCoroutine(flash.FlashRoutine());
            StartCoroutine(Knockback(hitPosition));
        }
    }

    public void DetectDeath() {
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        //GetComponent<PickUpSpawner>().DropItems();
        Destroy(gameObject);
    }

    IEnumerator Knockback(Vector3 hitPosition) {
        if (knockbakStrength <= 0) {
            if(hp > 0) ContinueBehaviour();
            yield break;
        }
            // Aplicar fuerza de knockback
            rigidBody.linearVelocity = (transform.position - hitPosition).normalized * knockbakStrength;
            yield return new WaitForSeconds(knockbackTime);

            // Detener el movimiento
            rigidBody.linearVelocity = Vector3.zero;
            yield return new WaitForSeconds(knockbackTime);
        if (hp > 0) ContinueBehaviour();
    }

    public virtual void StopBehaviour() {}

    public virtual void ContinueBehaviour() {}

    public virtual void ResetPosition (){
        transform.position = initialPosition;
        hp = maxHp;
    }
}

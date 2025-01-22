using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //public static PlayerController Instance { get; private set; }

    public float speed = 5f;
    public float runSpeed = 8f;
    public SpriteRenderer pickItem;
    Vector2 direction;

    Rigidbody2D rigidBody;
    Animator animator;

    bool isAttacking;

    GameManager gameManager;
    private Flash flash;

    public float knockbakStrength = 2f;
    protected float knockbackTime = 0.3f;

    bool uncontrollable;

    List<BasicInteraction> basicInteractionList = new List<BasicInteraction>();
    
    private void Awake()
    {
        transform.position = DataInstance.Instance.playerPosition;
    }

    public void Start()
    {
        flash = GetComponent<Flash>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!uncontrollable)
        {
            rigidBody.linearVelocity = direction * speed;
        }
    }
    void Update()
    {
        Inputs();
        Animations();
    }

    private void Inputs()
    {
        if (isAttacking || uncontrollable) return;

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Detectar si está corriendo
        if (Input.GetKey(KeyCode.LeftShift) && direction.magnitude > 0)
        {
            speed = runSpeed;
        }
        else
        {
            speed = 5f; // Restablecer a la velocidad normal
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E)){
            if (basicInteractionList != null){
                Vector2 playerFacing = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
                bool interactionSuccess = false;
                foreach (BasicInteraction basicInteraction in basicInteractionList){
                    if (interactionSuccess) return;
                    if(basicInteraction.Interact(playerFacing, transform.position)){
                        interactionSuccess = true;
                    }
                }
            
            }
        }

    }

    private void Attack()
    {
        animator.Play("Attack");
        isAttacking = true;
        direction = Vector2.zero;
    }

    private void Animations()
    {
        if (isAttacking || uncontrollable) return;

        if (direction.magnitude != 0)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.Play("Run");
            }
            else
            {

                animator.Play("Movimiento");
            }

        }
        else animator.Play("Idle");
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MaxHpUp"))
        {
            Destroy(collision.gameObject);
            gameManager.IncreaseMaxHP();
            pickItem.sprite = collision.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PickItem());
            DataInstance.Instance.SaveSceneData(collision.name);
        }
        else if (collision.CompareTag("Heal") && gameManager.CanHeal())
        {
            Destroy(collision.gameObject);
            gameManager.UpdateCurrentHP(4);
        }
        else if (collision.CompareTag("Interaction"))
        {
            basicInteractionList.Add(collision.GetComponent<BasicInteraction>());
        }
        else if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.UpdateCurrentKeys(1);
            pickItem.sprite = collision.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PickItem());
            DataInstance.Instance.SaveSceneData(collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Interaction")){
            basicInteractionList.Remove(collision.GetComponent<BasicInteraction>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.CompareTag("Enemy"))
        {
            gameManager.UpdateCurrentHP(-2); // Resta vida al jugador
            StartCoroutine(flash.FlashRoutine()); // Ejecuta el efecto flash
            StartCoroutine(KnockbackRoutine(collision.transform.position)); // Aplica knockback
        }
    }

    private IEnumerator KnockbackRoutine(Vector3 hitPosition)
    {
        uncontrollable = true;
        direction = Vector2.zero;
        rigidBody.linearVelocity = (transform.position - hitPosition).normalized * knockbakStrength;
        yield return new WaitForSeconds(knockbackTime);

        // Detener el movimiento
        rigidBody.linearVelocity = Vector3.zero;
        uncontrollable = false;
    }

    IEnumerator PickItem()
    {
        animator.Play("RecogerItem");
        uncontrollable = true;
        direction = rigidBody.linearVelocity = Vector2.zero;
        Camera.main.GetComponent<CameraController>().PauseEnemies();

        yield return new WaitForSeconds(0.55f);

        uncontrollable = false;
        Camera.main.GetComponent<CameraController>().ResumeEnemies();

    }
}

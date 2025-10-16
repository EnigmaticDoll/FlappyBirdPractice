using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Liniar Motion")]
    [SerializeField] private float velocityOnClick = 6.0f;
    [SerializeField] private float gravity = 0.5f;
    [Header("Angular Motion")]
    [SerializeField] private float minRotation = -70f;
    [SerializeField] private float maxRotation = 20f;
    [SerializeField] private float angularVelocityOnClick = 720f;
    [SerializeField] private float angularAcceleration = -2400;
    [SerializeField] private Animator animator;

    private bool isGameStarted = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public bool isDead;
    public bool isOnGround;
    private Vector2 startPosition;
    private float startRotation;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startPosition = rigidBody.position;
        startRotation = rigidBody.rotation;
    }

    private void OnEnable()
    {
        isDead = false;
        isOnGround = false;

        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        rigidBody.position = startPosition;
        rigidBody.rotation = startRotation;

        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0f);
        }
    }

    private void Start()
    {
        ResetPlayer();
    }

    private void Update()
    {
        if (!isGameStarted || isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            rigidBody.velocity = Vector2.up * velocityOnClick;
            rigidBody.angularVelocity = angularVelocityOnClick;
            animator.Rebind();
            animator.Update(0f);
        }
    }

    private void FixedUpdate()
    {
        if (!isGameStarted) return;

        rigidBody.velocity =
            isOnGround
            ? Vector2.zero
            : rigidBody.velocity + Vector2.down * gravity;

        if (isDead)
        {
            rigidBody.angularVelocity = 0;
        }
        else
        {
            rigidBody.angularVelocity += angularAcceleration * Time.fixedDeltaTime;
            rigidBody.rotation = Mathf.Clamp(rigidBody.rotation, minRotation, maxRotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            animator.enabled = false;
        }
        if (collision.gameObject.CompareTag("Base"))
        {
            isDead = true;
            isOnGround = true;
            animator.enabled = false;
        }

    }

    public void StartGame()
    {
        isGameStarted = true;
        rigidBody.gravityScale = 1;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (animator != null)
            animator.enabled = true;
    }

    public void ResetPlayer()
    {
        //isGameStarted = false;
        //isDead = false;
        //rb.gravityScale = 0;
        //rb.velocity = Vector2.zero;
        //transform.position = startPosition;
        //transform.rotation = startRotation;
        //rotationVector = Vector3.zero;

        //if (spriteRenderer != null)
        //    spriteRenderer.enabled = false;
        //if (animator != null)
        //{
        //    animator.enabled = false;
        //    animator.Rebind();
        //    animator.Update(0f);
        //}
    }
}
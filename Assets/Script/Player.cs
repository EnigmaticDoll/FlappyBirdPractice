using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float flyForce = 3.0f;
    [Header("Rotation")]
    [SerializeField] private float upRotate = 5f;
    [SerializeField] private float downRotate = -5f;
    [SerializeField] private Animator animator;

    private Vector3 rotationVector;
    private bool isGameStarted = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public bool isDead;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Start()
    {
        ResetPlayer();
    }

    private void Update()
    {
        if (!isGameStarted) return;
        if (isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * flyForce;
        }
    }

    private void FixedUpdate()
    {
        if (!isGameStarted) return;

        float angleDelta = 0;
        if (!isDead)
        {
            if (rb.velocity.y > 0) { angleDelta = upRotate; }
            else if (rb.velocity.y < 0) { angleDelta = downRotate; }
            rotationVector = new Vector3(0, 0, Mathf.Clamp((rotationVector.z + angleDelta), -70, 30));
            transform.eulerAngles = rotationVector;
        }
    }

    private void OnEnable()
    {
        isDead = false;
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
        rotationVector = Vector3.zero;
        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            animator.enabled = false;
            UIController uiController = FindObjectOfType<UIController>();
            if (uiController != null)
            {
                uiController.ShowGameOverUI();
            }
        }
        if (collision.gameObject.CompareTag("Base"))
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            animator.enabled = false;
            UIController uiController = FindObjectOfType<UIController>();
            if (uiController != null)
            {
                uiController.ShowGameOverUI();
            }
        }

    }

    public void StartGame()
    {
        isGameStarted = true;
        rb.gravityScale = 1;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (animator != null)
            animator.enabled = true;
    }

    public void ResetPlayer()
    {
        isGameStarted = false;
        isDead = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
        rotationVector = Vector3.zero;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (animator != null)
        {
            animator.enabled = false;
            animator.Rebind();
            animator.Update(0f);
        }
    }
}
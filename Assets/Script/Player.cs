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
    private Rigidbody2D rb;
    public bool isDead;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        startRotation = transform.rotation;
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
    //Player Move
    private void Update()
    {
        if (isDead) return;
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * flyForce;
        }
    }
    //Player Rotation
    private void FixedUpdate()
    {
        float angleDelta = 0;

        if (!isDead)
        {
            if (rb.velocity.y > 0) { angleDelta = upRotate; }
            else if (rb.velocity.y < 0) { angleDelta = downRotate; }

            rotationVector = new Vector3(0, 0, Mathf.Clamp((rotationVector.z + angleDelta), -70, 30));
            transform.eulerAngles = rotationVector;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            animator.enabled = false;
        }
    }
}

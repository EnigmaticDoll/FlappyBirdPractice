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

    GameManager.GameState gameStateCache;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public bool isDead;
    public bool isOnGround;
    private Vector2 startPosition;
    private float startRotation;

    bool hitSoundPlayed = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startPosition = rigidBody.position;
        startRotation = rigidBody.rotation;
    }

    private void OnGameReady()
    {
        isDead = false;
        isOnGround = false;
        hitSoundPlayed = false;

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
        if (GameManager.GameState.Ongoing != gameStateCache || isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            rigidBody.velocity = Vector2.up * velocityOnClick;
            rigidBody.angularVelocity = angularVelocityOnClick;
            animator.Rebind();
            animator.Update(0f);
            SoundManager.instance.playerWingSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.GameState.Ongoing != gameStateCache && GameManager.GameState.GameOver != gameStateCache) return;

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
        if (collision.gameObject.CompareTag("Obstacle") && !hitSoundPlayed)
        {
            isDead = true;
            animator.enabled = false;
            SoundManager.instance.playerWingSound.Stop();
            SoundManager.instance.playerHitSound.Play();
            SoundManager.instance.playerDeadSound.Play();
            hitSoundPlayed = true;
        }
        if (collision.gameObject.CompareTag("Base"))
        {
            isOnGround = true;
            if (!hitSoundPlayed)
            {
                isDead = true;
                animator.enabled = false;
                SoundManager.instance.playerWingSound.Stop();
                SoundManager.instance.playerHitSound.Play();
                SoundManager.instance.playerDeadSound.Play();
                hitSoundPlayed = true;

            }

        }

    }

    //public void StartGame()
    //{
    //    isGameStarted = true;
    //    rigidBody.gravityScale = 1;
    //    if (spriteRenderer != null)
    //        spriteRenderer.enabled = true;
    //    if (animator != null)
    //        animator.enabled = true;
    //}

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

    private void OnGameStateChange(GameManager.GameState gameState)
    {
        gameStateCache = gameState;
        // TODO: this function is called when GameManager changes its game state.
        //       so cache this value and use that value in the FixedUpdate and Update function.
        // TODO: because the cached value do not cover other non-script components such as animators
        //       and rigidbodies, we need to disable and enable those components in this function.
        //       enable such components when the game is resummed and disable them when the game is paused.
        switch (gameState)
        {
            case GameManager.GameState.GameReady:
                OnGameReady();
                break;
            case GameManager.GameState.Ongoing:
                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.Paused:
                break;
        }
    }
}
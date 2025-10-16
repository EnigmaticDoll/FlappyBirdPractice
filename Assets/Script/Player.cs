using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�ӵ�")]
    [SerializeField] private float flyForce = 3.0f;

    [Header("ȸ��")]
    [SerializeField] private float upRotate = 5f;
    [SerializeField] private float downRotate = -5f;

    private Vector3 rotationVector;
    private Rigidbody2D rb;
    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Player �̵�
    private void Update()
    { if (isDead) return;
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * flyForce;
        }


    }

    //Player ȸ��
    private void FixedUpdate()
    {

        float angleDelta = 0;

        if (rb.velocity.y > 0) { angleDelta = upRotate; }
        else if (rb.velocity.y < 0) { angleDelta = downRotate; }

        rotationVector = new Vector3(0, 0, Mathf.Clamp((rotationVector.z + angleDelta), -70, 30));
        transform.eulerAngles = rotationVector;
    }

}

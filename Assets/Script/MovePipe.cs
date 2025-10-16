using UnityEngine;

public class MovePipe : MonoBehaviour
{
    public float pipSpeed = 2.0f;
    void Update()
    {
        transform.position += Vector3.left * pipSpeed * Time.deltaTime;
    }
}

using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float minPipe = 0.5f;
    [SerializeField] private float maxPipe = 2.0f;

    public GameObject pipePrefab;
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            GameObject newPipe = Instantiate(pipePrefab);
            newPipe.transform.position = new Vector3(2, Random.Range(minPipe, maxPipe), 0);
            timer = 0;
            Destroy(newPipe, 5.0f);
        }
    }
}

using UnityEngine;

public class MakePipe : MonoBehaviour
{
    [SerializeField] float minPipe = 0.5f;
    [SerializeField] float maxPipe = 2.0f;

    public GameObject pips;
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            GameObject newPipe = Instantiate(pips);
            newPipe.transform.position = new Vector3(2, Random.Range(minPipe, maxPipe), 0);
            timer = 0;
            Destroy(newPipe, 5.0f);

        }
    }

}

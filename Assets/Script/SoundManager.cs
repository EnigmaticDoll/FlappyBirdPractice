using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource playerDeadSound;
    public AudioSource coinGetSound;
    public AudioSource playerHitSound;
    public AudioSource playerWingSound;

    private void Awake()
    {
        if(instance == null) 
        {
        instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
}

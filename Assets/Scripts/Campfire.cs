using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceLoop;
    [SerializeField] private AudioSource audioSourceOneHit;

    public void PlayAudioSourceTwo()
    {
        if (audioSourceOneHit)
        {
            audioSourceOneHit.Play();
            //Debug.Log("playing campfire sound");
        }
    }
}

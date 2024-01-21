using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource laughter1;
    [SerializeField]
    private AudioSource laughter2;
    [SerializeField]
    private AudioSource laughter3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomLaughter()
    {
        int randomIndex = UnityEngine.Random.Range(1, 3);
        switch (randomIndex)
        {
            case 1:
                laughter1.Play();
                break;
            case 2:
                laughter2.Play();
                break;
            case 3:
                laughter3.Play();
                break;
            default:
                laughter1.Play();
                break;
        }
    }

    public bool IsPlayingLaughter()
    {
        return (laughter1.isPlaying || laughter2.isPlaying || laughter3.isPlaying);
    }
}

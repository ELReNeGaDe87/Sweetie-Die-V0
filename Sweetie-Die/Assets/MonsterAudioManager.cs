using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource footsteps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootsteps()
    {
        footsteps.Play();
    }

    public void PauseFootsteps()
    {
        footsteps.Pause();
    }
}

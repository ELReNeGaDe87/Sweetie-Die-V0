using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntidadAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource jumpscare;
    [SerializeField]
    private AudioSource laughter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayLaughter()
    {
        if (!laughter.isPlaying) laughter.Play();
    }

    public void PlayJumpscare()
    {
        if (!jumpscare.isPlaying) jumpscare.Play();
    }
}


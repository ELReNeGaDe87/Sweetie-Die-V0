using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup output;
    public Sound[] sounds;
    public static AudioManager instance;
    private Scene scene;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Add the listener to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        scene = SceneManager.GetActiveScene();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = output;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    void playInitSound()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Play("MusicMenu");
        }
        else if (SceneManager.GetActiveScene().name == "Hotel")
        {
            Play("BackgroundNoise");
        }
    }

    private void OnDestroy()
    {
        // Always clean up your listeners when not needed anymore
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Listener for sceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // return if not the start calling scene
        if (!string.Equals(scene.path, this.scene.path)) {
            this.scene = scene;
            StopAll();
            playInitSound();
            return;
        };

        UnityEngine.Debug.Log("Re-Initializing", this);
        playInitSound();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            UnityEngine.Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            UnityEngine.Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            UnityEngine.Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            Stop(s.name);
        }
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            UnityEngine.Debug.LogWarning("Sound: " + name + " not found!");
        }
        if (s.source == null)
        {
            UnityEngine.Debug.LogWarning("SoundSource to: " + name + " not found!");
        }
        return s.source.isPlaying;
    }

}

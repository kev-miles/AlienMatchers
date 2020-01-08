using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;

public class Music : MonoBehaviour, ISound, IObserver
{
    public AudioMixerSnapshot regularGameplay;
    public AudioMixerSnapshot gameOver;
    public AudioSource music;
    public AudioClip[] musiclibrary = new AudioClip[1];

    void Start()
    {
        music = GetComponent<AudioSource>();
        PlaySound(0);
        regularGameplay.TransitionTo(0.1f);
    }

    public void PlaySound(int clip)
    {
        music.clip = musiclibrary[clip];
        if (music.isPlaying == false)
        {
            music.Play();
        }
    }

    public void StopSound(int clip)
    {
        music.clip = musiclibrary[clip];
        if (music.isPlaying == true)
        {
            music.Stop();
        }
    }

    public void Notify(int evento)
    {
        if (evento == 0)
            gameOver.TransitionTo(0.1f);
    }
}

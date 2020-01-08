using UnityEngine;
using System.Collections;
using System;

public class ShieldBlock : MonoBehaviour, ISound {

    public AudioSource blocksound;
    public AudioClip[] blocksoundlibrary = new AudioClip[1];

    SpriteRenderer sp;
    Collider2D box;

    void Awake ()
    {
        blocksound = this.gameObject.GetComponent<AudioSource>();
        sp = this.gameObject.GetComponent<SpriteRenderer>();
        box = this.gameObject.GetComponent<Collider2D>();
    }

    void Start ()
    {
        sp.material.color = Color.cyan;

        if (!sp.enabled)
        {
            sp.enabled = true;
        }
        if (!box.enabled)
        {
            box.enabled = true;
        }
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.GetComponent<Bullet>() != null)
        {
            col.GetComponent<Bullet>().OnRelease();
            sp.enabled = false;
            box.enabled = false;
            PlaySound(0);
        }

        if (col.GetComponent<EnemyModel>() != null)
        {
            sp.enabled = false;
            box.enabled = false;
            PlaySound(0);
        }

    }

    public void PlaySound(int clip)
    {
        blocksound.clip = blocksoundlibrary[clip];
        if (blocksound.isPlaying == false)
        {
            blocksound.Play();
        }
        else
            return;
    }

    public void StopSound(int clip)
    {
        blocksound.clip = blocksoundlibrary[clip];
        if (blocksound.isPlaying == true)
        {
            blocksound.Stop();
        }
    }
}

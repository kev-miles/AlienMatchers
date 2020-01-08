using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour, IColor, ISound {

    public event Action OnReanimate = delegate { };

    public Sprite sp;
    public AudioSource playeraudio;
    public AudioClip[] playeraudiolibrary = new AudioClip[3];

    Sprite originalSprite;
    Color playertint = Color.magenta;

    Text _cdtext;

    float scaletimer;
    bool scaleddown;

	void Start ()
    {
        playeraudio = this.gameObject.GetComponent<AudioSource>();
        originalSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        SetColor(this.gameObject, playertint);
        _cdtext = GameObject.FindGameObjectWithTag("UICanvas").GetComponentInChildren<Text>();
        _cdtext.color = playertint;
	}

	void Update ()
    {
        ShotAnim();
	}

    public void MoveLeft (float speed)
    {
        this.gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
        PlaySound(2);
    }

    public void MoveRight(float speed)
    {
        this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        PlaySound(2);
    }


    #region Shoot
    public void Shoot()
    {
        if (!scaleddown)
        {
            PlaySound(0);
            this.gameObject.transform.localScale -= new Vector3(0, 0.25f, 0);
            scaleddown = true;
            scaletimer = 0.0f;
        }
        _cdtext.enabled = false;
    }

    public void CooldownFeedback()
    {
        _cdtext.enabled = true;
    }

    void ShotAnim ()
    {
        scaletimer += Time.deltaTime;
        if (scaletimer >= 0.1f && scaleddown)
        {
            this.gameObject.transform.localScale += new Vector3(0, 0.25f, 0);
            scaleddown = false;
        }
    }
    #endregion

    #region Death

    public IEnumerator Death()
    {
        PlaySound(1);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sp;
        this.gameObject.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1f);

        if (this.gameObject.GetComponent<PlayerModel>().lives != 0)
            Reanimate();
        else
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;     
    }

    void Reanimate ()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        OnReanimate();
    }

    public void SetColor (GameObject go, Color tint)
    {
        go.GetComponent<SpriteRenderer>().material.color = tint;
    }

    #endregion

    #region Sound

    public void PlaySound(int clip)
    {
        playeraudio.clip = playeraudiolibrary[clip];
        if (playeraudio.isPlaying == false)
        {
            playeraudio.Play();
        }
        else
            return;
    }

    public void StopSound(int clip)
    {
        playeraudio.clip = playeraudiolibrary[clip];
        if (playeraudio.isPlaying == true)
        {
            playeraudio.Stop();
        }
    }

    #endregion
}

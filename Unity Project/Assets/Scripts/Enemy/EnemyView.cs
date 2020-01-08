using UnityEngine;
using System.Collections;
using System;

public class EnemyView : MonoBehaviour, IColor, ISound {

    public Sprite[] animSprites;
    public Sprite[] deathSprites;
    public AudioSource enemyaudio;
    public AudioClip[] enemyaudiolibrary = new AudioClip[2];

    [HideInInspector] public bool isAlive = true;

    public event Action OnAnimate = delegate { };
    public event Action OnRemove = delegate { };

    public float animtimer;
    float timer;
    int spriteNumber;

    SpriteRenderer _sr;
    Sprite sprite;

    void Awake ()
    {
        _sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

	void Start ()
    {
        spriteNumber = 0;
        sprite = animSprites[spriteNumber];
        _sr.sprite = sprite;
        enemyaudio = this.gameObject.GetComponent<AudioSource>();

        OnAnimate += () => StartCoroutine(BasicAnimation());
        OnRemove += StopAllCoroutines;
        OnRemove += this.gameObject.GetComponent<EnemyModel>().OnRelease;

        StartCoroutine(BasicAnimation());
    }

    #region Animation

    IEnumerator BasicAnimation ()
    {
        yield return new WaitForSeconds(animtimer);

        if (spriteNumber >= animSprites.Length-1)
            spriteNumber = 0;
        else
            spriteNumber += 1;

        SetSprite(animSprites[spriteNumber]);
    }

    void SetSprite(Sprite sp)
    {
        sprite = sp;
        _sr.sprite = sprite;
        StopCoroutine(BasicAnimation());
        OnAnimate();
    }

    public void DeathTransition ()
    {
        StopAllCoroutines();
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        sprite = deathSprites[0];
        _sr.sprite = sprite;
        yield return new WaitForSeconds(0.8f);
        OnRemove();
    }

    #endregion

    public void SetColor(GameObject go, Color tint)
    {
        _sr.material.color = tint;
    }

    #region Sound

    public void PlaySound(int clip)
    {
        enemyaudio.clip = enemyaudiolibrary[clip];
        if (enemyaudio.isPlaying == false)
        {
            enemyaudio.Play();
        }
        else
            return;
    }

    public void StopSound(int clip)
    {
        enemyaudio.clip = enemyaudiolibrary[clip];
        if (enemyaudio.isPlaying == true)
        {
            enemyaudio.Stop();
        }
    }

    #endregion

}

using UnityEngine;
using System.Collections;
using System;
using BulletTypes;

public class Bullet : MonoBehaviour, IPoolable
{
    [HideInInspector] public SpriteRenderer spriteR;
    [HideInInspector] public int bulletType;
    [HideInInspector] public BulletPool _origin;
    [HideInInspector] public Color tint;

    public Sprite[] _allsprites;

    public void OnAcquire()
    {
        spriteR = this.gameObject.GetComponent<SpriteRenderer>();
        ImplementSprite();
    }

    public void OnRelease()
    {
        spriteR.sprite = default(Sprite);
        bulletType = default(int);
        _origin.Release(this);
    }

    void Update ()
    {
        ImplementBehaviour();
    }

    void ImplementBehaviour()
    {
        switch (bulletType)
        {
            case 0:
                this.transform.Translate(Vector3.up * 15f * Time.deltaTime);
                break;
            case 1:
                this.transform.Translate(Vector3.down * 15f * Time.deltaTime);
                break;
        }
    }

    void ImplementSprite()
    {
        switch (bulletType)
        {
            case 0:
                spriteR.sprite = _allsprites[bulletType];
                break;
            case 1:
                spriteR.sprite = _allsprites[bulletType];
                break;
        }
    }
}

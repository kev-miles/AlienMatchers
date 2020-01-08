using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BulletTypes;

public class PlayerModel : MonoBehaviour, IObservable {

    public event Action OnMoveLeft = delegate { };
    public event Action OnMoveRight = delegate { };
    public event Action OnShoot = delegate { };
    public event Action OnShotReady = delegate { };
    public event Action OnDeath = delegate { };
    public event Action OnGameOver = delegate { };
    public event Action OnLeftLimit = delegate { };
    public event Action OnRightLimit = delegate { };

    public float speed;
    public float cooldown;
    public int lives;

    public float LeftScreenLimit;
    public float RightScreenLimit;

    BulletPool pool;

    float timer;
    bool canShoot;

    List<IObserver> myObservers = new List<IObserver>();

    void Awake ()
    {
        OnLeftLimit += () => this.gameObject.transform.position = new Vector2(LeftScreenLimit, this.transform.position.y);
        OnRightLimit += () => this.gameObject.transform.position = new Vector2(RightScreenLimit, this.transform.position.y);
        OnGameOver += () => this.gameObject.GetComponent<Collider2D>().enabled = false;

        OnDeath += HandleLives;
        OnDeath += () => SendMessage(lives);
    }

    void Start ()
    {
        pool = GameObject.FindGameObjectWithTag("SceneScripts").GetComponent<BulletPool>();
	}

	void Update ()
    {
        ScreenLimits();
        CooldownTimer();
	}

    #region Observer

    public void AddObserver(IObserver obs)
    {
        myObservers.Add(obs);
    }

    public void RemoveObserver(IObserver obs)
    {
        myObservers.Remove(obs);
    }

    #endregion

    #region Movement

    public void MoveLeft()
    {
        OnMoveLeft();
    }

    public void MoveRight()
    {
        OnMoveRight();
    }

    void ScreenLimits()
    {
        if (this.gameObject.transform.position.x > RightScreenLimit)
            OnRightLimit();
        if (this.gameObject.transform.position.x < LeftScreenLimit)
            OnLeftLimit();
    }

    #endregion

    #region Shooting

    public void Shoot()
    {
        if(canShoot)
        {
            pool.Acquire(this.transform, (int)BulletTypes.Types.PlayerRegular);
            OnShoot();
            canShoot = false;
        }
    }

    void CooldownTimer ()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            canShoot = true;
            OnShotReady();
            timer = 0;
        }
    }
    #endregion

    void OnTriggerEnter2D (Collider2D col)
    {
        if(col.gameObject.GetComponent<Bullet>() != null)
        {
            var bullet = col.gameObject.GetComponent<Bullet>();

            if (bullet.bulletType != (int)BulletTypes.Types.PlayerRegular)
            {
                bullet.OnRelease();               
                OnDeath();
            }
            else
                return;
        }
    }

    #region Misc

    void HandleLives()
    {
        if (lives > 0)
            lives--;
    }

    public void RemovePlayer()
    {
        OnGameOver();
    }

    #endregion

    void SendMessage(int value)
    {
        foreach(var o in myObservers)
        {
            o.Notify(value);
        }
    } 
}

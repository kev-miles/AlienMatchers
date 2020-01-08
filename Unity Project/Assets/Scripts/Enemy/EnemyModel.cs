using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnemyModel : MonoBehaviour, IPoolable, IObservable {

    public event Action OnDeath = delegate { };
    public event Action OnShoot = delegate { };
    public event Action OnSetColor = delegate { };

    public int points;
    public float shotTimer;

    [HideInInspector] public Color tint;

    [HideInInspector] public EnemyModel _enemyTop;
    [HideInInspector] public EnemyModel _enemyBot;
    [HideInInspector] public EnemyModel _enemyLeft;
    [HideInInspector] public EnemyModel _enemyRight;

    [HideInInspector] public int gridX;
    [HideInInspector] public int gridY;

    [HideInInspector] public EnemyPool _origin;

    BulletPool pool;

    EnemyView view;

    List<IObserver> myObservers = new List<IObserver>();
    [HideInInspector] public List<EnemyModel> neighbours = new List<EnemyModel>();

    #region SizeData

    public float Width
    {
        get { return this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x; }
    }

    public float Height
    {
        get { return this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y; }
    }

    #endregion

    public void OnAcquire()
    {
        view = this.gameObject.GetComponent<EnemyView>();

        OnDeath += () => NotifyObserver(points);
        OnShoot += () => view.PlaySound(0);
        OnShoot += () => StartCoroutine(Shoot());
        OnDeath += view.DeathTransition;
        OnDeath += () => this.gameObject.GetComponent<Collider2D>().enabled = false;
        OnSetColor += () => view.SetColor(this.gameObject, tint);
    }

    void Awake ()
    {
        pool = GameObject.FindGameObjectWithTag("SceneScripts").GetComponent<EnemyBulletPool>();
    }

    #region Observers

    public void AddObserver(IObserver obs)
    {
        myObservers.Add(obs);
    }

    public void RemoveObserver(IObserver obs)
    {
        myObservers.Remove(obs);
    }

    #endregion

    #region Shooting

    public void ShotEvent()
    {
        OnShoot();
    }

    IEnumerator Shoot ()
    {
        yield return new WaitForSeconds(shotTimer);       
        pool.Acquire(this.transform, (int)BulletTypes.Types.EnemyRegular);
    }

    #endregion

    #region Collision & Death

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.GetComponent<Bullet>() != null)
        {
            var bullet = col.gameObject.GetComponent<Bullet>();

            StopAllCoroutines();

            if (bullet.bulletType != (int)BulletTypes.Types.EnemyRegular)
            {
                view.PlaySound(1);
                bullet.OnRelease();            
                Death(0);
            }
            else
                return;
        }
        if (col.gameObject.name == "SideLimitR" || col.gameObject.name == "SideLimitL")
        {
            foreach(var o in myObservers)
            {
                o.Notify(0);
            }
        }
        if (col.gameObject.GetComponent<PlayerController>() != null)
        {
            NotifyObserver(2);
        }    
        else
            return;
    }

    public void Death(int multiplier)
    {
        if (multiplier >= 5)
        {
            return;
        }
        else
        {
            OnDeath();
            RecountNeighbours(multiplier);
        }       
    }

    #endregion

    #region Neighbours
     
    public void RecountNeighbours(int multiplier)
    {
        if (_enemyBot != null)
        {
            if (_enemyBot.tint == this.tint)
            {
                multiplier++;
            }
        }

        if (_enemyTop != null)
        {
            if (_enemyTop.tint == this.tint)
            {
                multiplier++;
            }
        }

        if (_enemyLeft != null)
        {
            if (_enemyLeft.tint == this.tint)
            {
                multiplier++;
            }
        }

        if (_enemyRight != null)
        {
            if (_enemyRight.tint == this.tint)
            {
                multiplier++;
            }
        }

        TerminateNeighbours(multiplier);
    }

    void TerminateNeighbours (int multiplier)
    {
        if (_enemyTop != null)
        {
            if (_enemyBot != null)
                _enemyTop._enemyBot = this._enemyBot;
            else
            {
                _enemyTop._enemyBot = null;
            }
            
            if (_enemyTop.tint == this.tint)
            {
                _enemyTop.Death(multiplier);
            }
        }

        if (_enemyBot != null)
        {
            _enemyBot._enemyTop = null;

            if (_enemyBot.tint == this.tint)
            {
                _enemyBot.Death(multiplier);
            }
        }      
         
        if (_enemyLeft != null)
        {
            _enemyLeft._enemyRight = null;

            if (_enemyLeft.tint == this.tint)
            {
                _enemyLeft.Death(multiplier);
            }
        }

        if (_enemyRight != null)
        {
            _enemyRight._enemyLeft = null;

            if (_enemyRight.tint == this.tint)
            {
                _enemyRight.Death(multiplier);
            }
        }
    }

    #endregion

    #region Misc

    public void SetColor ()
    {
        OnSetColor();
    }

    public void NotifyObserver(int value)
    {
        foreach(var o in myObservers)
        {
            o.Notify(value);
        }
    }
    #endregion

    public void OnRelease()
    {
        myObservers.Clear();
        this.transform.parent = _origin.enemyContainer.transform;
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        tint = Color.white;
        
        _origin.Release(this);
    }
}

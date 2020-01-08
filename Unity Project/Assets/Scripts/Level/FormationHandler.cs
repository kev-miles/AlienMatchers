using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class FormationHandler : MonoBehaviour, IObserver, IObservable {

    public event Action OnChange = delegate { };
    public event Action OnLeft = delegate { };
    public event Action OnRight = delegate { };

    int initialcheck = 0;
    bool movesRight = true;
    bool movedDown = false;

    bool keepShooting = true;
    float time = 0;
    float timeToShoot = 2;

    float cd = 1;

    List<IObserver> myObservers = new List<IObserver>();
    List<GameObject> mySubjects = new List<GameObject>();

    void Start ()
    {
        OnLeft += () => StartCoroutine(MoveLeft());
        OnRight += () => StartCoroutine(MoveRight());
        OnChange += () => StartCoroutine(MoveDown());       
        StartCoroutine(MoveRight());

        foreach (Transform child in this.transform)
        {
            initialcheck++;           
        }
    }

    void Update ()
    {
        SubjectHandling();
        CheckEnemies();
    }

    void SubjectHandling ()
    {
        if (!keepShooting)
            return;

        time += Time.deltaTime;

        if(time > timeToShoot)
        {
            foreach(Transform child in this.transform)
            {
                mySubjects.Add(child.gameObject);
            }

            time = 0;
            var random = new System.Random();
            var shooters = mySubjects.Select(x => x).Where(x => x.GetComponent<EnemyModel>()._enemyBot == null).OrderBy(x => random.Next()).Take(5);
            
            foreach (var s in shooters)
            {
                s.GetComponent<EnemyModel>().ShotEvent();
            }

            mySubjects.Clear();
        }
    }

    void CheckEnemies ()
    {
        int enemyCounter = 0;

        foreach (Transform child in this.transform)
        {
            enemyCounter++;
        }

        if (enemyCounter == 0)
            foreach(var o in myObservers)
            { o.Notify(4); }
    }

    IEnumerator MoveRight ()
    {
        this.transform.position += Vector3.right;
        yield return new WaitForSeconds(cd);
        OnRight();
    }

    IEnumerator MoveLeft()
    {
        this.transform.position += Vector3.left;
        yield return new WaitForSeconds(cd);
        OnLeft();
    }

    IEnumerator MoveDown()
    {
        yield return new WaitForSeconds(1);

        if (!movedDown)
        {
            this.transform.position += Vector3.down;
            cd -= cd / 10;
            movedDown = true;

            if (movesRight)
            {
                movesRight = false;
                movedDown = false;
                OnLeft();
            }
            else
            {
                movesRight = true;
                movedDown = false;
                OnRight();
            }
        }                      
    }

    void SendMessage (int code)
    {
        foreach (var o in myObservers)
        {
            o.Notify(0);
        }
    }

    public void Notify(int evento)
    {
        switch(evento)
        {
            case 0:
                StopAllCoroutines();
                OnChange();
                break;
            case 1:
                StopAllCoroutines();
                keepShooting = false;
                break;
            case 2:
                StopAllCoroutines();
                keepShooting = false;
                SendMessage(0);
                break;              
        }
    }

    public void AddObserver(IObserver obs)
    {
        myObservers.Add(obs);
    }

    public void RemoveObserver(IObserver obs)
    {
        myObservers.Remove(obs);
    }
}

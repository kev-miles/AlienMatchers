using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BotTrigger : MonoBehaviour, IObservable {

    List<IObserver> myObservers = new List<IObserver>();

    void Start()
    {
        AddObserver(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>());
    }

    public void AddObserver(IObserver obs)
    {
        myObservers.Add(obs);
    }

    public void RemoveObserver(IObserver obs)
    {
        myObservers.Remove(obs);
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.GetComponent<EnemyModel>() != null)
        {
            foreach(var o in myObservers)
            {
                o.Notify(0);
            }
        }
    }
}

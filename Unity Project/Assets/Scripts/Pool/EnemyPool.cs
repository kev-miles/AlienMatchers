using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour {

    public EnemyModel enemyPrefab;

    Stack<EnemyModel> _usable;
    [HideInInspector] public GameObject enemyContainer;
    
    string objectname = "Enemy Container";

    public int amount;

    public void Awake()
    {
        enemyContainer = new GameObject();
        enemyContainer.name = objectname;
        _usable = new Stack<EnemyModel>();

        for (int i = 0; i <= amount; i++)
        {
            var _enemyprefab = GameObject.Instantiate(enemyPrefab);
            _enemyprefab.transform.position = new Vector2(100, 100);
            _enemyprefab.gameObject.SetActive(false);
            Add(_enemyprefab);
            _usable.Push(_enemyprefab);
        }
    }

    public EnemyModel Acquire(Vector2 pos)
    {
        var obj = _usable.Pop();
        obj.gameObject.SetActive(true);
        obj.transform.position = pos;
        obj._origin = this;
        obj.OnAcquire();
        return obj;
    }

    void Add(EnemyModel obj)
    {
        obj.transform.parent = enemyContainer.transform;
        obj.gameObject.SetActive(false);
        _usable.Push(obj);
    }

    public void Release(EnemyModel obj)
    {
        _usable.Push(obj);
        obj.transform.position = new Vector2(100, 100);
        Add(obj);
    }
}

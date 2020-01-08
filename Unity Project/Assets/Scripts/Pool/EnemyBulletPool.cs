using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnemyBulletPool : BulletPool {

    Stack<Bullet> _usable;
    GameObject _bulletcontainer;

    string _objectname = "EnemyBullets Container";

    public override void Awake()
    {
        _bulletcontainer = new GameObject();
        _bulletcontainer.name = _objectname;
        _usable = new Stack<Bullet>();

        for (int i = 0; i <= amount; i++)
        {
            var _enemyprefab = GameObject.Instantiate(_bulletprefab);
            _enemyprefab.transform.position = new Vector2(100, 100);
            _enemyprefab.gameObject.SetActive(false);
            Add(_enemyprefab);
            _usable.Push(_enemyprefab);
        }
    }

    public override Bullet Acquire(Transform shooter, int type)
    {
        var obj = _usable.Pop();
        obj.gameObject.SetActive(true);
        obj.transform.position = shooter.position;
        obj._origin = this;
        obj.bulletType = type;
        obj.OnAcquire();
        return obj;
    }

    void Add(Bullet obj)
    {
        obj.transform.parent = _bulletcontainer.transform;
        obj.gameObject.SetActive(false);
        _usable.Push(obj);
    }

    public override void Release(Bullet obj)
    {
        _usable.Push(obj);
        obj.transform.position = new Vector2(100, 100);
        Add(obj);
    }
}

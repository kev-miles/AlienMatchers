using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BulletPool : MonoBehaviour
{
    Stack<Bullet> usable;
    GameObject bulletcontainer;
    public Bullet _bulletprefab;
    public int amount;

    string objectname = "PlayerBullets Container";

    public virtual void Awake()
    {
        bulletcontainer = new GameObject();
        bulletcontainer.name = objectname;
        usable = new Stack<Bullet>();

        for (int i=0; i<=amount; i++)
        {
            var _playerprefab = GameObject.Instantiate(_bulletprefab);
            _playerprefab.transform.position = new Vector2(100, 100);
            _playerprefab.gameObject.SetActive(false);
            Add(_playerprefab);
            usable.Push(_playerprefab);
        }
    }

    public virtual Bullet Acquire (Transform shooter, int type)
    {
        var obj = usable.Pop();
        obj.gameObject.SetActive(true);
        obj.transform.position = shooter.position;
        obj._origin = this;
        obj.bulletType = type;
        obj.OnAcquire();
        return obj;
    }

    void Add (Bullet obj)
    {
        obj.transform.parent = bulletcontainer.transform;
        obj.gameObject.SetActive(false);
        usable.Push(obj);
    }

    public virtual void Release(Bullet obj)
    {
        usable.Push(obj);
        obj.transform.position = new Vector2(100, 100);
        Add(obj);
    }
}

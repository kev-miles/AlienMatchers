using UnityEngine;
using System.Collections;

public class Limits : MonoBehaviour {

    void OnCollisionEnter2D (Collision2D col)
    {
        if(col.gameObject.GetComponent<Bullet>() != null)
        {
            col.gameObject.GetComponent<Bullet>().OnRelease();
        }
    }
}

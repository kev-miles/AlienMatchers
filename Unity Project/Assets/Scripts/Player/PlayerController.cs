using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour, IObserver {

    bool gameOver = false;
    bool isActive = true;
    PlayerView view;
    PlayerModel model;

    void Awake ()
    {
        view = this.gameObject.GetComponent<PlayerView>();
        model = this.gameObject.GetComponent<PlayerModel>();

        model.OnMoveLeft += () => view.MoveLeft(model.speed);
        model.OnMoveRight += () => view.MoveRight(model.speed);
           
        model.OnShotReady += view.CooldownFeedback;
        model.OnShoot += view.Shoot;
        model.OnDeath += () => StartCoroutine(view.Death());
        model.OnDeath += () => isActive = false;
        view.OnReanimate += () => isActive = true;
    }

	void Update ()
    {
        UserInput();
	}

    void UserInput ()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (!isActive || gameOver)
            return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            model.MoveLeft();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            model.MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
        {
            model.Shoot();
        }
    }

    public void Notify(int evento)
    {
        if (evento == 0)
        {
            gameOver = true;
            model.RemovePlayer();
        }                   
    }
}

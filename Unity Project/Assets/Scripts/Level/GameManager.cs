using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour, IObserver, IObservable
{
    public event Action OnGameOver = delegate { };
    public event Action OnUpdateLives = delegate { };

    [Range(4, 12)] public int _MatrixDepth;
    int _MatrixHeight;

    Vector2 startPos;

    public GameObject endMessage;
    GameObject containerObject;
    
    int lives;
    int allenemies;

    EnemyModel[,] allEnemies;    
    EnemyPool enemyPool;

    List<IObserver> myObservers = new List<IObserver>();

    void Awake()
    {
        _MatrixHeight = _MatrixDepth / 2;
        _MatrixDepth *= 2;       
        allEnemies = new EnemyModel[_MatrixDepth, _MatrixHeight];

        startPos = Camera.main.ScreenToWorldPoint(new Vector2(5, Screen.height / 2));

        OnGameOver += () => endMessage.SetActive(true);
    }

    void Start()
    {
        enemyPool = GameObject.FindGameObjectWithTag("SceneScripts").GetComponent<EnemyPool>();
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerModel>().AddObserver(this);
        AddObserver(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        AddObserver(this.gameObject.GetComponent<Music>());
        
        containerObject = new GameObject();
        containerObject.name = "Enemy Block";
        containerObject.transform.position = startPos;
        containerObject.AddComponent<FormationHandler>();
        containerObject.AddComponent<ScoreManager>();

        containerObject.GetComponent<FormationHandler>().AddObserver(this);
        AddObserver(containerObject.GetComponent<FormationHandler>());

        GenerateEnemies();

        OnGameOver += EndGame;
        OnUpdateLives += () => GameObject.FindGameObjectWithTag("Lives").GetComponent<Text>().text = "Lives " + lives.ToString();
    }

    void GenerateEnemies()
    {
        List<EnemyModel> enemyList = new List<EnemyModel>();

        for (int y = 0; y < _MatrixHeight; y++)
        {
            for (int x = 0; x < _MatrixDepth; x++)
            {
                var enemy = enemyPool.Acquire(new Vector2(startPos.x + x/2, y+0.5f));
                allEnemies[x, y] = enemy;
                enemy.gridX = x;
                enemy.gridY = y;
                enemyList.Add(enemy);
                enemy.tint = DefineColors((int)UnityEngine.Random.Range(0, 4));
                enemy.SetColor();
                enemy.transform.parent = containerObject.transform;
                enemy.AddObserver(containerObject.GetComponent<FormationHandler>());
                enemy.AddObserver(containerObject.GetComponent<ScoreManager>());
            }
        }

        foreach (var e in enemyList)
        {
            if (e.gridY - 1 >= 0)
                e._enemyBot = allEnemies[e.gridX, e.gridY - 1];
            if (e.gridY + 1 < _MatrixHeight)
                e._enemyTop = allEnemies[e.gridX, e.gridY + 1];
            if (e.gridX - 2 >= 0)
                e._enemyLeft = allEnemies[e.gridX - 2, e.gridY];
            if (e.gridX + 1 < _MatrixDepth)
                e._enemyRight = allEnemies[e.gridX + 1, e.gridY];
        }
    }

    Color DefineColors(int value)
    {
        Color newColor = Color.white;

        if (value <= 3)
        {
            switch (value)
            {
                case 0:
                    newColor = Color.red;
                    break;

                case 1:
                    newColor = Color.blue;
                    break;

                case 2:
                    newColor = Color.green;
                    break;

                case 3:
                    newColor = Color.yellow;
                    break;
            }
        }
        return newColor;
    }

    void EndGame ()
    {
        foreach(var o in myObservers)
        {
            o.Notify(0);
            o.Notify(1);
        }
    } 

    public void Notify(int evento)
    {
        if (evento == 0)            
            OnGameOver();
        if (evento <= 3)
            lives = evento;
            OnUpdateLives();
        if (evento == 4)
            OnGameOver();
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


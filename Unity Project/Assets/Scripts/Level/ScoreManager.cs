using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour, IObserver
{
    public event Action OnChange = delegate { };

    int tempscore;
    int totalscore = 0;
    int scoreToAdd;

    float timer;
    bool enableTimer;
    Text score;

    void Awake()
    {
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();

        OnChange += () => enableTimer = true;
    }

    void Update()
    {
        if (enableTimer)
        {
            Invoke("PrepScore", 0.2f);
            enableTimer = false;
        }
    }

    int Recount(int value)
    {
        if (value == 0)
            return 10;
        if (value == 1)
            return 10;

        int prevPrev = 10;
        int prev = 10;
        int result = 0;

        for (int i = 2; i <= value; i++)
        {
            result = prev + prevPrev;
            prevPrev = prev;
            prev = result;
        }

        return result;
    }

    void ShowScore(int add)
    {
        totalscore += add;
        score.text = "Points " + totalscore.ToString();
    }

    void PrepScore()
    {
        int value = tempscore / 40;
        ShowScore(value * Recount(value));
        tempscore = 0;
    }

    public void Notify(int amount)
    {
        switch (amount)
        {
            case 0:
                break;
            case 2:
                break;
            case 10:
                tempscore += amount;
                OnChange();
                break;
        }
    }
}

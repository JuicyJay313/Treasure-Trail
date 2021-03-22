using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI numOfLives;

    private void Awake()
    {
        // Singleton pattern
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        numOfLives.text = playerLives.ToString();
    }

    private void Update()
    {
        numOfLives.text = playerLives.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        playerLives--;
        FindObjectOfType<LevelManagement>().LoseLife();
        Debug.Log("Number of lives left: " + playerLives);
    }

    private void ResetGameSession()
    {
        FindObjectOfType<LevelManagement>().LevelFailed();
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] TextMeshProUGUI scoreText;
    void Awake() 
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if(numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        liveText.text = "" + playerLives;
        scoreText.text = "" + score;
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLive();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int value)
    {
        score += value;
        scoreText.text = "" + score;
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLive()
    {
        playerLives = playerLives - 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        liveText.text = "" + playerLives;
    }
}

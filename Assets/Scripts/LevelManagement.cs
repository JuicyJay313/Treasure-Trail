using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagement : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float deathDelay = 2f;
    [SerializeField] Enemy[] enemies;
    int currentSceneIndex;
    int enemiesCount;

    private void Start()
    {
        enemiesCount = enemies.Length;
    }

    private void Update()
    {
        LevelComplete();
    }

    public void enemyDestroy()
    {
        enemiesCount--;
        Debug.Log("Enemies Left: " + enemiesCount);
    }
    public void LevelComplete()
    {
        if(enemiesCount <= 0)
        {
            StartCoroutine(LoadNextLevel());
        }
        
    }

    public void LevelFailed()
    {
        // When the players loses all his hearts...
        // Go to game over screen
        StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSecondsRealtime(deathDelay);
        SceneManager.LoadScene("Game Over");
        Time.timeScale = 1f;
    }
}

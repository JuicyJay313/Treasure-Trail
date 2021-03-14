using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagement : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float deathDelay = 2f;
    [SerializeField] float slowMoMultiplier = 0.2f;
    public void LevelComplete()
    {
        // When all enemies are defeated....
        // Go to the next level
        StartCoroutine(LoadNextLevel());
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
    }

    IEnumerator LoadGameOver()
    {
        Time.timeScale = slowMoMultiplier;
        yield return new WaitForSecondsRealtime(deathDelay);
        //SceneManage.LoadScene("Game Over");
        Time.timeScale = 1f;
    }
}

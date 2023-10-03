using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr inst;

    private void Awake()
    {
        inst = this;
    }

    public void Update()
    {
        
    }

    public void ChangeSceneToGame()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void ChangeSceneToDifficultySelector()
    {
        SceneManager.LoadScene("Difficulty Selector");
    }
    public void ChangeSceneToPlayAgainstAI()
    {
        SceneManager.LoadScene("Play Against AI");
    }

    public void ChangeSceneToEndGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

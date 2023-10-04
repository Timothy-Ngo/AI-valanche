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
    public void ChangeSceneToHowToPlay()
    {
        SceneManager.LoadScene("How To Play");
    }
    public void ChangeSceneToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ChangeSceneToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ChangeSceneToDifficultySelector()
    {
        SceneManager.LoadScene("Difficulty Selector");
    }

    public void ChangeSceneToModeSelector()
    {
        SceneManager.LoadScene("Mode Selector");
    }
    public void ChangeSceneToPlayAgainstAI(int difficulty)
    {
        Difficulty.inst.difficulty = difficulty;
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

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}

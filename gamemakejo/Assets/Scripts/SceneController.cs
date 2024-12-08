using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public string stage1Scene = "Stage1";
    public string stage2Scene = "Stage2";
    public string stage3Scene = "Stage3";
    public string lobbyScene = "Lobby";
    public string gameOverScene = "GameOver";
    public string clearScene = "Clear";

    public void LoadStage1Scene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(stage1Scene);
    }

    public void LoadStage2Scene()
    {
        SceneManager.LoadScene(stage2Scene);
    }

    public void LoadStage3Scene()
    {
        SceneManager.LoadScene(stage3Scene);
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene(lobbyScene);
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    public void LoadClearScene()
    {
        SceneManager.LoadScene(clearScene);
    }
}

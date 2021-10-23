using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameStateEvents.OnQuitGame += ExitGame;
    }

    private void OnDestroy()
    {
        GameStateEvents.OnQuitGame -= ExitGame;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

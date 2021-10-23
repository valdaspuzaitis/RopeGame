using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameEvents.OnQuitGame += ExitGame;
    }

    private void OnDestroy()
    {
        GameEvents.OnQuitGame -= ExitGame;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

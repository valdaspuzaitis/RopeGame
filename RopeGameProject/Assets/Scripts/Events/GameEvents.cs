public static class GameEvents
{
    public delegate void GameStateChange();
    public delegate void GemAction(int gemID);
    public delegate void RopeAction();
    public delegate void LevelSelect(int level);
    public delegate void LevelLoadAction();

    public static event LevelSelect OnLevelSelect;
    public static event LevelSelect OnLevelDataLoad;
    public static event GameStateChange OnLevelChoose;
    public static event GameStateChange OnLevelStart;
    public static event GameStateChange OnLevelWin;
    public static event GameStateChange OnLevelExit;
    public static event GameStateChange OnQuitGame;
    public static event GemAction OnGemTouch;
    public static event RopeAction OnRopeReachDestination;
    public static event LevelLoadAction OnBadLevelData;

    public static void LevelSelected(int levelIndex)
    {
        OnLevelSelect?.Invoke(levelIndex);
    }
    public static void LevelDataLoaded(int amountOfLevels)
    {
        OnLevelDataLoad?.Invoke(amountOfLevels);
    }

    public static void ChooseLevel()
    {
        OnLevelChoose?.Invoke();
    }

    public static void LevelStart()
    {
        OnLevelStart?.Invoke();
    }

    public static void LevelWon()
    {
        OnLevelWin?.Invoke();
    }

    public static void LevelExit()
    {
        OnLevelExit?.Invoke();
    }

    public static void QuitGame()
    {
        OnQuitGame?.Invoke();
    }

    public static void GemWasTouched(int gemID)
    {
        OnGemTouch?.Invoke(gemID);
    }

    public static void RopeReachedDestination()
    {
        OnRopeReachDestination?.Invoke();
    }
    public static void BadLevelData()
    {
        OnBadLevelData?.Invoke();
    }
}

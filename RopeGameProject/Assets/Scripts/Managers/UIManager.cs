using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject levelChoose;
    [SerializeField]
    private GameObject chooseLevelButton;
    [SerializeField]
    private GameObject noLevelsFound;
    [SerializeField]
    private GameObject returnToMainMenu;
    [SerializeField]
    private GameObject youWonScreen;

    private void Start()
    {
        GameStateEvents.OnLevelStart += GameHasStarted;
        GameStateEvents.OnLevelExit += MainMenuDisplay;
        GameStateEvents.OnLevelChoose += ShowLevelChooseItems;
        GameStateEvents.OnLevelDataLoad += CreateLevelChooseButton;
        GameStateEvents.OnLevelWin += ShowYouWinScreen;

        MainMenuDisplay();
    }

    private void MainMenuDisplay()
    {
        MainMenu.SetActive(true);
        returnToMainMenu.SetActive(false);
        youWonScreen.SetActive(false);
    }

    private void GameHasStarted()
    {
        MainMenu.SetActive(false);
        returnToMainMenu.SetActive(true);
    }

    private void ShowYouWinScreen()
    {
        youWonScreen.SetActive(true);
    }

    private void ShowLevelChooseItems()
    {
        levelChoose.SetActive(true);
    }

    private void CreateLevelChooseButton(int amountOfLevels)
    {
        DestroyLevelChooseButtons();
        if (amountOfLevels > 0)
        {
            noLevelsFound.SetActive(false);
            for (int i = 0; i < amountOfLevels; i++)
            {
                GameObject currentChooseLevelButton = GameObject.Instantiate(chooseLevelButton);
                currentChooseLevelButton.GetComponentInChildren<LevelSelected>().levelIndex = i;
                currentChooseLevelButton.transform.SetParent(levelChoose.transform);
                currentChooseLevelButton.transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            noLevelsFound.SetActive(true);
        }
    }

    private void DestroyLevelChooseButtons()
    {
        foreach (Transform levelChooseButton in levelChoose.GetComponentInChildren<Transform>())
        {
            if (levelChooseButton.name != "EmptySpaceOnTopOfGroup")
            {
                Destroy(levelChooseButton.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        GameStateEvents.OnLevelStart -= GameHasStarted;
        GameStateEvents.OnLevelExit -= MainMenuDisplay;
        GameStateEvents.OnLevelChoose -= ShowLevelChooseItems;
        GameStateEvents.OnLevelDataLoad -= CreateLevelChooseButton;
        GameStateEvents.OnLevelWin -= ShowYouWinScreen;
    }
}

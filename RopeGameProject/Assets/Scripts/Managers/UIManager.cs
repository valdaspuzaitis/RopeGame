using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject MainMenu;
    [Tooltip ("Container under which buttons to choose level are grouped")]
    [SerializeField] private GameObject levelChoose;
    [Tooltip ("Prefab of button which chooses level")]
    [SerializeField] private GameObject chooseLevelButton;
    [Tooltip ("Object to display if game data does not contain any levels")]
    [SerializeField] private GameObject noLevelsFound;
    [Tooltip ("Button to return to main menu")]
    [SerializeField] private GameObject returnToMainMenu;
    [Tooltip ("Object to display when playwer has won the game")]
    [SerializeField] private GameObject youWonScreen;
    [Tooltip ("Object to display when level data is corupt and game cannot be created from it")]
    [SerializeField] private GameObject badLevelData;

    private void Start()
    {
        GameEvents.OnLevelStart += GameHasStarted;
        GameEvents.OnLevelExit += MainMenuDisplay;
        GameEvents.OnLevelChoose += ShowLevelChooseItems;
        GameEvents.OnLevelDataLoad += CreateLevelChooseButtons;
        GameEvents.OnLevelWin += ShowYouWinScreen;
        GameEvents.OnBadLevelData += ShowBadDataScreen;

        MainMenuDisplay();
    }

    private void MainMenuDisplay()
    {
        MainMenu.SetActive(true);
        returnToMainMenu.SetActive(false);
        youWonScreen.SetActive(false);
        badLevelData.SetActive(false);
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

    private void ShowBadDataScreen()
    {
        badLevelData.SetActive(true);
    }

    private void CreateLevelChooseButtons(int amountOfLevels)
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
        GameEvents.OnLevelStart -= GameHasStarted;
        GameEvents.OnLevelExit -= MainMenuDisplay;
        GameEvents.OnLevelChoose -= ShowLevelChooseItems;
        GameEvents.OnLevelDataLoad -= CreateLevelChooseButtons;
        GameEvents.OnLevelWin -= ShowYouWinScreen;
        GameEvents.OnBadLevelData -= ShowBadDataScreen;
    }
}

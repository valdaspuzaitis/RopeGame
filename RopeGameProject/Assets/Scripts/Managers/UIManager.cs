using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        GameStateEvents.OnLevelStart += HideMainMenu;
        GameStateEvents.OnLevelStart += ShowReturnToMainMenu;
        GameStateEvents.OnLevelExit += ShowMainMenu;
        GameStateEvents.OnLevelExit += HideReturnToMainMenu;
        GameStateEvents.OnLevelChoose += ShowLevelChooseItems;
        GameStateEvents.OnLevelDataLoad += CreateLevelChooseButton;

        HideReturnToMainMenu();
    }

    private void ShowMainMenu()
    {
        MainMenu.SetActive(true);

    }

    private void HideReturnToMainMenu()
    {
        returnToMainMenu.SetActive(false);
    }

    private void ShowReturnToMainMenu()
    {
        returnToMainMenu.SetActive(true);
    }

    private void HideMainMenu()
    {
        MainMenu.SetActive(false);
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
        GameStateEvents.OnLevelStart -= HideMainMenu;
        GameStateEvents.OnLevelExit -= ShowMainMenu;
        GameStateEvents.OnLevelChoose -= ShowLevelChooseItems;
        GameStateEvents.OnLevelDataLoad -= CreateLevelChooseButton;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private DataRetrieve readDataMethod;
    [SerializeField]
    private GameObject gem;
    [SerializeField]
    private GameObject levelGemsUIContainer;

    [SerializeField]
    [Tooltip("Element to calculate screen size from. Preferred \"Canvas\".")]
    private GameObject fullScreenSize;

    private AllLevels allLevelsData;
    private GameObject[] existingLevelGems;
    private Queue gemsToDrawRopeTo;

    private int lastSelectedGemID;
    private float heightUnit;
    private float widthUnit;
    private float permilleFromScreenHeight = 30f;

    private void Start()
    {
        GameStateEvents.OnLevelChoose += ConstructLevelData;
        GameStateEvents.OnLevelSelect += LoadCurrentLevelData;
        GameStateEvents.OnGemTouch += ChangeGemColor;
        GameStateEvents.OnGemTouch += FadeOutGemID;
}

    private void OnDestroy()
    {
        GameStateEvents.OnLevelChoose -= ConstructLevelData;
        GameStateEvents.OnLevelSelect -= LoadCurrentLevelData;
        GameStateEvents.OnGemTouch -= ChangeGemColor;
        GameStateEvents.OnGemTouch -= FadeOutGemID;
    }

    private void ConstructLevelData()
    {
        allLevelsData = JsonConvert.DeserializeObject<AllLevels>(readDataMethod.ReadData());

        GameStateEvents.LevelDataLoaded(allLevelsData.levels.Count);
        Debug.Log(allLevelsData.levels.Count);
    }

    private void ConstructScreenCoordinatesUnits()
    {
        RectTransform screenSize = fullScreenSize.GetComponent<RectTransform>();
        heightUnit = screenSize.rect.height / 1000f;
        widthUnit = screenSize.rect.width / 1000f;
    }

    private void LoadCurrentLevelData(int levelID)
    {
        GameStateEvents.LevelStart();
        ConstructScreenCoordinatesUnits();
        int gemID = 0;
        SingleLevel currentLevel = allLevelsData.levels[levelID];
        existingLevelGems = new GameObject[currentLevel.level_data.Count];
        for (int i = 0; i < currentLevel.level_data.Count; i++)
        {
            CreateGem(gemID++, currentLevel.level_data[i], -currentLevel.level_data[++i]);
        }
    }

    private void CreateGem(int gemID, float coordinateX, float coordinateY)
    {
        GameObject createdGem = GameObject.Instantiate(gem);
        createdGem.GetComponent<GemID>().gemID = gemID;
        createdGem.transform.SetParent(levelGemsUIContainer.transform);
        createdGem.transform.localPosition = new Vector3(widthUnit * coordinateX, heightUnit * coordinateY, gemID / 1000f);
        createdGem.transform.localScale = new Vector2(heightUnit * permilleFromScreenHeight, heightUnit * permilleFromScreenHeight);
        existingLevelGems[gemID] = createdGem;
    }

    private void CreateNextRope()
    {

    }

    private bool CanGemChange(int gemID)
    {
        if(lastSelectedGemID - 1 == gemID)
        {
            return true;
        }
        return false;
    }

    private void ChangeGemColor(int gemID)
    {
        existingLevelGems[gemID].GetComponent<GemAppearance>().ChangeSprite();
    }

    private void FadeOutGemID(int gemID)
    {
        existingLevelGems[gemID].GetComponent<TextFadeOut>().TextDissapear();
    }
}

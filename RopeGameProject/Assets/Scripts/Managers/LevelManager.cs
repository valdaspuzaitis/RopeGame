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
    private GameObject rope;
    [SerializeField]
    private GameObject levelGemsUIContainer;
    [SerializeField]
    private GameObject levelRopesUIContainer;

    [SerializeField]
    [Tooltip("Element to calculate screen size from. Preferred \"Canvas\".")]
    private GameObject fullScreenSize;

    private AllLevels allLevelsData;
    private GameObject[] existingLevelGems;
    private GameObject[] existingLevelRopes;

    private Queue<int> gemsToDrawRopeTo = new Queue<int>();

    private int nextSelectedGemID;
    private float screenUnit;
    private float screenSizeCorrection;
    private float permilleFromScreenHeight = 30f;

    private bool noRopeInTransit = true;

    private void Start()
    {
        GameStateEvents.OnLevelChoose += ConstructLevelData;
        GameStateEvents.OnLevelSelect += LoadCurrentLevelData;
        GameStateEvents.OnGemTouch += TakeActionsOnGem;
        GameStateEvents.OnLevelExit += RemoveLevelObject;
        GameStateEvents.OnRopeReachDestination += RopeReachedItsDestination;
    }

    private void OnDestroy()
    {
        GameStateEvents.OnLevelChoose -= ConstructLevelData;
        GameStateEvents.OnLevelSelect -= LoadCurrentLevelData;
        GameStateEvents.OnGemTouch -= TakeActionsOnGem;
        GameStateEvents.OnLevelExit -= RemoveLevelObject;
        GameStateEvents.OnRopeReachDestination -= RopeReachedItsDestination;
    }

    private void ConstructLevelData()
    {
        allLevelsData = JsonConvert.DeserializeObject<AllLevels>(readDataMethod.ReadData());

        GameStateEvents.LevelDataLoaded(allLevelsData.levels.Count);
    }

    private void ConstructScreenCoordinatesUnits()
    {
        RectTransform screenSize = fullScreenSize.GetComponent<RectTransform>();
        screenUnit = screenSize.rect.height / 1000f;

        screenSizeCorrection = (screenSize.rect.width - screenSize.rect.height) / 2f;
    }

    private void LoadCurrentLevelData(int levelID)
    {
        GameStateEvents.LevelStart();
        ConstructScreenCoordinatesUnits();
        int gemID = 0;
        SingleLevel currentLevel = allLevelsData.levels[levelID];
        existingLevelGems = new GameObject[currentLevel.level_data.Count / 2];
        existingLevelRopes = new GameObject[currentLevel.level_data.Count / 2];
        noRopeInTransit = true;
        nextSelectedGemID = 0;
        for (int i = 0; i < currentLevel.level_data.Count; i++)
        {
            CreateGem(gemID++, currentLevel.level_data[i], -currentLevel.level_data[++i]);
        }
    }

    private void CreateGem(int gemID, float coordinateX, float coordinateY)
    {
        GameObject createdGem = Instantiate(gem);
        createdGem.GetComponent<GemID>().gemID = gemID;
        createdGem.transform.SetParent(levelGemsUIContainer.transform);
        createdGem.transform.localPosition = new Vector3(screenUnit * coordinateX + screenSizeCorrection, screenUnit * coordinateY, gemID / 1000f);
        createdGem.transform.localScale = new Vector2(screenUnit * permilleFromScreenHeight, screenUnit * permilleFromScreenHeight);
        existingLevelGems[gemID] = createdGem;
    }

    private void RopeReachedItsDestination()
    {
        noRopeInTransit = true;
        if (existingLevelRopes[existingLevelRopes.Length - 2] != null && existingLevelRopes[existingLevelRopes.Length - 1] == null)
        {
            AddLastRope();
        }
        else if (existingLevelRopes[existingLevelRopes.Length - 2] == null)
        {
            BeginNextRope();
        }
        else if (existingLevelRopes[existingLevelRopes.Length - 1] != null)
        {
            GameStateEvents.LevelWon();
        }
    }

    private void AddLastRope()
    {
        CreateRope(nextSelectedGemID - 1, GenerateEndpointsForRope(existingLevelGems[nextSelectedGemID - 1], existingLevelGems[0]));
    }

    private void BeginNextRope()
    {
        if (gemsToDrawRopeTo.Count != 0 && noRopeInTransit)
        {
            int nextGemID = gemsToDrawRopeTo.Dequeue();
            CreateRope(nextGemID - 1, GenerateEndpointsForRope(existingLevelGems[nextGemID - 1], existingLevelGems[nextGemID]));
        }
    }

    private void CreateRope(int ropeID, Vector2[] ropeEndpointCoordinates)
    {
        GameObject createdRope = Instantiate(rope);
        createdRope.GetComponent<RopeController>().startPoint = ropeEndpointCoordinates[0];
        createdRope.GetComponent<RopeController>().endPoint = ropeEndpointCoordinates[1];
        createdRope.transform.SetParent(levelRopesUIContainer.transform);
        createdRope.transform.localPosition = Vector3.one;
        createdRope.transform.localScale = Vector3.one;
        existingLevelRopes[ropeID] = createdRope;

        noRopeInTransit = false;
    }

    private Vector2[] GenerateEndpointsForRope(GameObject ropeStart, GameObject ropeEnd)
    {
        Vector2[] ropeEndpoints = new Vector2[2];
        ropeEndpoints[0] = CorrdinatesFromGem(ropeStart);
        ropeEndpoints[1] = CorrdinatesFromGem(ropeEnd);
        return ropeEndpoints;
    }

    private Vector2 CorrdinatesFromGem(GameObject gem)
    {
        return gem.GetComponent<RectTransform>().anchoredPosition;
    }

    private void RemoveLevelObject()
    {
        ClearObjectsInCollection(existingLevelGems);
        ClearObjectsInCollection(existingLevelRopes);
    }

    private void ClearObjectsInCollection(GameObject[] objectToRemove)
    {
        foreach (GameObject objectInPlay in objectToRemove)
        {
            Destroy(objectInPlay);
        }
    }

    private void TakeActionsOnGem(int gemID)
    {
        if (CanGemChange(gemID) || gemID == 0)
        {
            ChangeGemColor(gemID);
            FadeOutGemID(gemID);
            if (gemID != 0)
            {
                gemsToDrawRopeTo.Enqueue(gemID);
                BeginNextRope();
            }
            nextSelectedGemID++;
        }
    }

    private bool CanGemChange(int gemID)
    {
        if (nextSelectedGemID == gemID)
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

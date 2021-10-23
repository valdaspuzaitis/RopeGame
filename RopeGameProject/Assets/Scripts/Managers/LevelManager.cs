using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private DataRetrieve readDataMethod;

    [Tooltip("Element to calculate screen size from. Preferred \"Canvas\".")]
    [SerializeField] private GameObject fullScreenSize;

    [HideInInspector] public int nextSelectedGemID;
    private float screenUnit;
    private float screenSizeCorrection;

    private AllLevels allLevelsData;

    private void Start()
    {
        GameStateEvents.OnLevelChoose += ConstructLevelData;
        GameStateEvents.OnLevelSelect += LoadCurrentLevelData;
        GameStateEvents.OnGemTouch += TakeActionsOnGem;
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
        GemManager.Instance.existingLevelGems = new GameObject[currentLevel.level_data.Count / 2];
        GemManager.Instance.SetGemScale(screenUnit);

        RopeManager.Instance.existingLevelRopes = new GameObject[currentLevel.level_data.Count / 2];
        RopeManager.Instance.noRopeInTransit = true;
        nextSelectedGemID = 0;
        for (int i = 0; i < currentLevel.level_data.Count; i++)
        {
            GemManager.Instance.CreateGem(gemID++, screenUnit * currentLevel.level_data[i] + screenSizeCorrection, screenUnit * -currentLevel.level_data[++i]);
        }
    }

    public void ClearObjectsInCollection(GameObject[] objectToRemove)
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
            GemManager.Instance.GemActionOnTouch(gemID);
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

    private void OnDestroy()
    {
        GameStateEvents.OnLevelChoose -= ConstructLevelData;
        GameStateEvents.OnLevelSelect -= LoadCurrentLevelData;
        GameStateEvents.OnGemTouch -= TakeActionsOnGem;
    }
}
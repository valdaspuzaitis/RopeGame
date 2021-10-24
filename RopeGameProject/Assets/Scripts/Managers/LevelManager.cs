using UnityEngine;
using Newtonsoft.Json;

public class LevelManager : Singleton<LevelManager>
{
    [Tooltip ("Class inheriting from \"DataRetrieve\" to load game data")]
    [SerializeField] private DataRetrieve readDataMethod;
    [Tooltip("Element to calculate screen size from, preferred \"Canvas\"")]
    [SerializeField] private GameObject fullScreenSize;    

    [HideInInspector] public int nextSelectedGemID;

    private float screenUnit;
    private float screenSizeCorrection;

    private AllLevels allLevelsData;

    private void Start()
    {
        GameEvents.OnLevelChoose += ConstructLevelData;
        GameEvents.OnLevelSelect += LoadCurrentLevelData;
        GameEvents.OnGemTouch += TakeActionsOnGem;
    }

    private void ConstructLevelData()
    {
        allLevelsData = JsonConvert.DeserializeObject<AllLevels>(readDataMethod.ReadData());

        GameEvents.LevelDataLoaded(allLevelsData.levels.Count);
    }

    private void LoadCurrentLevelData(int levelID)
    {       
        SingleLevel currentLevel = allLevelsData.levels[levelID];
        if (currentLevel.level_data.Count % 2 == 0)
        {
            int gemID = 0;
            ConstructScreenCoordinateUnits();
            GemManager.Instance.existingLevelGems = new GameObject[currentLevel.level_data.Count / 2];
            GemManager.Instance.SetGemScale(screenUnit);

            RopeManager.Instance.existingLevelRopes = new GameObject[currentLevel.level_data.Count / 2];
            RopeManager.Instance.noRopeInTransit = true;
            nextSelectedGemID = 0;
            for (int i = 0; i < currentLevel.level_data.Count; i++)
            {
                GemManager.Instance.CreateGem(gemID++, screenUnit * currentLevel.level_data[i] + screenSizeCorrection, screenUnit * -currentLevel.level_data[++i]);
            }
            GameEvents.LevelStart();
        }
        else
        {
            GameEvents.BadLevelData();
        }
    }

    private void ConstructScreenCoordinateUnits()
    {
        RectTransform screenSize = fullScreenSize.GetComponent<RectTransform>();
        screenUnit = screenSize.rect.height / 1000f;

        screenSizeCorrection = (screenSize.rect.width - screenSize.rect.height) / 2f;
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
        if (CanGemChange(gemID))
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
        GameEvents.OnLevelChoose -= ConstructLevelData;
        GameEvents.OnLevelSelect -= LoadCurrentLevelData;
        GameEvents.OnGemTouch -= TakeActionsOnGem;
    }
}
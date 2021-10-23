using System.Collections.Generic;
using UnityEngine;

public class GemManager : Singleton<GemManager>
{
    [Tooltip ("Gem prefab to spawn")]
    [SerializeField] private GameObject gem;
    [Tooltip ("Canvas container to group all spawned Gems under")]
    [SerializeField] private GameObject levelGemsUIContainer;

    [HideInInspector] public GameObject[] existingLevelGems;
    [HideInInspector] public float scaleOfGem;
    [HideInInspector] public Queue<int> gemsToDrawRopeTo = new Queue<int>();

    private float permilleFromScreenHeight = 30f;

    private void Start()
    {
        GameEvents.OnLevelExit += ClearGemData;
    }

    private void ClearGemData()
    {
        LevelManager.Instance.ClearObjectsInCollection(existingLevelGems);
        gemsToDrawRopeTo.Clear();
    }

    public void SetGemScale(float screenUnit)
    {
        scaleOfGem = screenUnit * permilleFromScreenHeight;
    }

    public void CreateGem(int gemID, float coordinateX, float coordinateY)
    {
        GameObject createdGem = Instantiate(gem);
        createdGem.GetComponent<GemController>().gemID = gemID;
        createdGem.transform.SetParent(levelGemsUIContainer.transform);
        createdGem.transform.localPosition = new Vector3(coordinateX, coordinateY, gemID / 1000f);
        createdGem.transform.localScale = new Vector2(scaleOfGem, scaleOfGem);
        existingLevelGems[gemID] = createdGem;
    }

    public void ChangeGemColor(int gemID)
    {
        existingLevelGems[gemID].GetComponent<GemController>().ChangeSprite();
    }

    public void FadeOutGemID(int gemID)
    {
        existingLevelGems[gemID].GetComponent<GemController>().TextDissapear();
    }

    public void GemActionOnTouch(int gemID)
    {
        ChangeGemColor(gemID);
        FadeOutGemID(gemID);
        if (gemID != 0)
        {
            gemsToDrawRopeTo.Enqueue(gemID);
            RopeManager.Instance.BeginNextRope();
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnLevelExit -= ClearGemData;
    }
}
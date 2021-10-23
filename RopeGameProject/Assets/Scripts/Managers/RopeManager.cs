using UnityEngine;

public class RopeManager : Singleton<RopeManager>
{
    [SerializeField] private GameObject rope;
    [SerializeField] private GameObject levelRopesUIContainer;

    [HideInInspector] public GameObject[] existingLevelRopes;
    [HideInInspector] public bool noRopeInTransit = true;

    private void Start()
    {
        GameStateEvents.OnRopeReachDestination += RopeReachedItsDestination;
        GameStateEvents.OnLevelExit += ClearRopeData;
    }

    private void ClearRopeData()
    {
        LevelManager.Instance.ClearObjectsInCollection(existingLevelRopes);
        StopAllCoroutines();
    }

    private void RopeReachedItsDestination()
    {
        noRopeInTransit = true;
        if (existingLevelRopes[existingLevelRopes.Length - 2] != null && existingLevelRopes[existingLevelRopes.Length - 1] == null)
        {
            AddLastRope(LevelManager.Instance.nextSelectedGemID);
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

    private void AddLastRope(int nextSelectedGemID)
    {
        int ropeID = nextSelectedGemID - 1;
        CreateRope(ropeID, GenerateEndpointsForRope(GemManager.Instance.existingLevelGems[ropeID], GemManager.Instance.existingLevelGems[0]));
    }

    public void BeginNextRope()
    {
        if (GemManager.Instance.gemsToDrawRopeTo.Count != 0 && noRopeInTransit)
        {
            int nextGemID = GemManager.Instance.gemsToDrawRopeTo.Dequeue();
            CreateRope(nextGemID - 1, GenerateEndpointsForRope(GemManager.Instance.existingLevelGems[nextGemID - 1], GemManager.Instance.existingLevelGems[nextGemID]));
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

    private void OnDestroy()
    {
        GameStateEvents.OnRopeReachDestination -= RopeReachedItsDestination;
        GameStateEvents.OnLevelExit -= ClearRopeData;
    }
}

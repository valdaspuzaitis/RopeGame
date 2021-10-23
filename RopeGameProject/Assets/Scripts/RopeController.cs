using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer ropeLine;

    [HideInInspector]
    public Vector2 startPoint;
    [HideInInspector]
    public Vector2 endPoint;

    private float speed = 200;
    // Start is called before the first frame update
    void Start()
    {
        ropeLine.SetPosition(0, startPoint);
        ropeLine.SetPosition(1, startPoint);
        StartCoroutine(MoveLineEndpoint());
    }

    IEnumerator MoveLineEndpoint()
    {
        Vector2 currentCoordinates = ropeLine.GetPosition(0);
        while(Vector2.Distance(currentCoordinates, endPoint) > 1)
        {
            currentCoordinates = ropeLine.GetPosition(0);
            Vector2 newPosition = Vector2.MoveTowards(currentCoordinates, endPoint, speed * Time.deltaTime);
            ropeLine.SetPosition(0, newPosition);
            yield return null;
        }
        GameStateEvents.RopeReachedDestination();
    }
}

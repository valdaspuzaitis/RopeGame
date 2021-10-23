using System.Collections;
using UnityEngine;

public class TemporaryScreen : MonoBehaviour
{
    [SerializeField] private float secondsToBeVisible = 2;

    private void Start()
    {
        GameEvents.OnLevelStart += ScreenNoLongerNeeded;
    }

    void OnEnable()
    {
        StartCoroutine(WaitUntillDissapear());
    }

    IEnumerator WaitUntillDissapear()
    {
        float waitedSeconds = 0;
        while(waitedSeconds < secondsToBeVisible)
        {
            waitedSeconds += Time.deltaTime;
            yield return null;
        }
        ScreenNoLongerNeeded();
    }

    private void ScreenNoLongerNeeded()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.OnLevelStart -= ScreenNoLongerNeeded;
    }
}

using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameStateEvents.LevelExit();
    }
}

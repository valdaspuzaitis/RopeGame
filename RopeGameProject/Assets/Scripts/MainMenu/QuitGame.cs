using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
        GameStateEvents.QuitGame();
    }
}

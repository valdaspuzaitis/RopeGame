using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChooseScreen : MonoBehaviour
{
    public void ChooseLevel()
    {
        GameStateEvents.ChooseLevel();
        Debug.Log("LevelChoose pressed");
    }
}

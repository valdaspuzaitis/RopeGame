using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelected : MonoBehaviour
{
    [HideInInspector]
    public int levelIndex;

    [SerializeField]
    private TextMeshProUGUI levelName;

    private void Start()
    {
        int levelIndexToDisplay = levelIndex + 1;
        levelName.text = "Level " + levelIndexToDisplay;
    }

    public void LoadLevel()
    {
        GameStateEvents.LevelSelected(levelIndex);
    }
}

using UnityEngine;
using TMPro;

public class LevelSelected : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;

    [HideInInspector] public int levelIndex;

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

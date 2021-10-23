using UnityEngine;
using TMPro;
using System;

public class ChangenName : MonoBehaviour
{
    [SerializeField] private string newName;
    [SerializeField] private TextMeshProUGUI nameToChange;

    public void EnactChange()
    {
        if (!string.IsNullOrEmpty(newName))
        {
            nameToChange.text = newName;
        }
        else
        {
            throw new ArgumentException("Name is empty.", "newName");
        }
    }
}

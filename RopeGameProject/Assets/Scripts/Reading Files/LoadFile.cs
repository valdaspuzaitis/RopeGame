using UnityEngine;
using System;

public class LoadFile : DataRetrieve
{
    public string fileName = "level_data";
    public override string ReadData()
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            string fullContent = Resources.Load<TextAsset>(fileName).ToString();
            return fullContent;
        }
        else
        {
            throw new ArgumentException("Path to file is empty.", "pathToFile");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadFile : DataRetrieve
{
    [SerializeField]
    private string pathToFile = "Assets/Level Load Data/level_data.json";
    public override string ReadData()
    {
        if (!string.IsNullOrEmpty(pathToFile))
        {
            StreamReader reader = new StreamReader(pathToFile);
            string fullContent = reader.ReadToEnd();
            reader.Close();
            return fullContent;
        }
        else
        {
            throw new ArgumentException("Path to file is empty.", "pathToFile");
        }        
    }
}

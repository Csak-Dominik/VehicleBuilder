using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Windows;

public class EditorInventoryManager : MonoBehaviour
{
    private const string DEFINITION_PATH = "Assets/Parts/Definitions/";

    private void Awake()
    {
        LoadParts();
    }

    private void LoadParts()
    {
        // Load xml files from DEFINITION_PATH
        foreach (string file in System.IO.Directory.GetFiles(DEFINITION_PATH))
        {
            // Load xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            // get part_data node
            XmlNode part_data = doc.SelectSingleNode("part_data");

            // read name and prefab
            string name = part_data.Attributes["name"].Value;
            string prefabName = part_data.Attributes["prefab"].Value;

            // get prefab
            GameObject prefab = Resources.Load<GameObject>("Parts/Prefabs/" + prefabName);
        }
    }
}
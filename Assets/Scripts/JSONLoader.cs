using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class JSONLoader : MonoBehaviour
{
    [SerializeField] static MyData[] arrayFromJson;
    [SerializeField] static Sprite[] imageFromResources;

    
    [ContextMenu("Save JSON")]
    static void SaveJSON()
    {
        string json = JsonUtility.ToJson(new Wrapper<MyData> { array = arrayFromJson });

        // Ensure the Resources folder exists
        string resourcesPath = Path.Combine(Application.dataPath, "Resources");
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        // Path to the JSON file within the Resources folder
        string filePath = Path.Combine(resourcesPath, "data.json");

        // Write JSON string to the file
        File.WriteAllText(filePath, json);

        Debug.Log("JSON saved to: " + filePath);
    }


    [ContextMenu("Load Images")]
    static void LoadImage()
    {
#if UNITY_EDITOR
        var temp = Resources.LoadAll<Texture2D>("Images");

        for (int i = 0; i < temp.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(temp[i]);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        imageFromResources = Resources.LoadAll<Sprite>("Images");

#endif
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using System.Diagnostics;
using System;
using System.IO;
using UnityEngine.UI;


public class LauncherManager : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] ButtonUI buttonPrefab;
    [SerializeField] Transform contentTf;

    [Header("Button")]
    [SerializeField] string youtubeUrl;
    [SerializeField] Button youtubeBtn;
    [SerializeField] string documentationUrl;
    [SerializeField] Button documentationBtn;
    [SerializeField] ButtonUI[] buttonList;
    [Header("Debugging")]
    [SerializeField] MyData[] arrayData;
    [SerializeField] List<Sprite> spriteList = new List<Sprite>();
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        /*LoadImage();
        LoadJSON();*/

        youtubeBtn.onClick.AddListener(YoutubeButtonClick);
        documentationBtn.onClick.AddListener(DocumentationButtonClick);

        if(buttonList.Length == 0)
        {
            var temp = contentTf.GetComponentsInChildren<ButtonUI>();
            buttonList = temp;
        }

        DOVirtual.DelayedCall(1f, () => {
            for (int i = 0; i < buttonList.Length; i++)
            {
                buttonList[i].Initialize(arrayData[i].GameTitle, arrayData[i].BuildPath);
            }
        });
        

        /*DOVirtual.DelayedCall(.5f, (()=> {
            for (int i = 0; i < arrayData.Length; i++)
            {
                ButtonUI temp = Instantiate(buttonPrefab, contentTf);
                temp.Initalize(arrayData[i].GameTitle, arrayData[i].GameTitle, spriteList[i]);
            }
        }));*/
    }

    private void DocumentationButtonClick()
    {
        Application.OpenURL(documentationUrl);
    }

    private void YoutubeButtonClick()
    {
        Application.OpenURL(youtubeUrl);
    }

    [ContextMenu("Load JSON")]
    void LoadJSON()
    {
        // Load the JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("data");

        
        if (jsonFile != null)
        {
            // Convert JSON back to array
            MyData[] dataArray = JsonUtility.FromJson<Wrapper<MyData>>(jsonFile.text).array;


            arrayData = dataArray;
        }
        else
        {
            //Debug.LogError("JSON file not found in Resources folder.");
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Load Image")]
    void LoadImage()
    {
        Texture2D[] texture;
        // Load the texture from the Resources folder
        texture = Resources.LoadAll<Texture2D>("Images");

        string path = Application.dataPath+Environment.CurrentDirectory + "Data";
        
        
        UnityEngine.Debug.Log("Path = " + path);

        foreach (var item in texture)
        {
            if (item != null)
            {
                // Optionally, log texture information
                //Debug.Log($"Loaded texture: {item.name}, Size: {item.width}x{item.height}");

                // Change texture settings if necessary
                item.wrapMode = TextureWrapMode.Clamp;
                item.filterMode = FilterMode.Bilinear;

                // Create a sprite from the texture
                Sprite sprite = Sprite.Create(
                    item,
                    new Rect(0, 0, item.width, item.height),
                    new Vector2(0.5f, 0.5f),
                    100.0f
                );

                // Use the sprite (for example, assign it to a SpriteRenderer)
                //Image spriteRenderer = gameObject.AddComponent<Image>();
                //spriteRenderer.sprite = sprite;
                //spriteRenderer.SetNativeSize();

                //Debug.Log("Sprite loaded and assigned successfully.");

                spriteList.Add(sprite);
            }
            else
            {
                //Debug.LogError("Failed to load the texture. Make sure the file is in the Resources folder and the name is correct.");
            }
        }
    }
#endif
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
[System.Serializable]
public class MyData
{
    public string GameTitle;
    public string BuildPath;
}

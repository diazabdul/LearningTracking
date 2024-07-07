using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public string textureFileName = "Images/"; // Name of the texture file without the extension
    [SerializeField] List<Sprite> spritesArray = new List<Sprite>();
    void Start()
    {

        // Load the texture from the Resources folder
        Texture2D[] texture = Resources.LoadAll<Texture2D>(textureFileName);

        foreach (var item in texture)
        {
            if (item != null)
            {
                // Optionally, log texture information
                Debug.Log($"Loaded texture: {item.name}, Size: {item.width}x{item.height}");

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

                Debug.Log("Sprite loaded and assigned successfully.");
                spritesArray.Add(sprite);
            }
            else
            {
                Debug.LogError("Failed to load the texture. Make sure the file is in the Resources folder and the name is correct.");
            }
        }       
        
    }

    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
    }
}

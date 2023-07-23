using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorSlot : MonoBehaviour
{
    private Button slotButton;
    private Image itemImage;
    private PartBuilder partBuilder;

    public void Awake()
    {
        slotButton = GetComponent<Button>();
        itemImage = transform.GetChild(0).GetComponent<Image>();

        partBuilder = Camera.main.GetComponent<PartBuilder>();
    }

    public void SetItem(Texture2D texture)
    {
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        itemImage.sprite = sprite;
    }

    public Texture2D GetTexture(GameObject gameObject)
    {
        return AssetPreview.GetMiniThumbnail(gameObject);
    }
}
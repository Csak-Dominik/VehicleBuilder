using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorSlot : MonoBehaviour
{
    [SerializeField]
    private Button slotButton;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private PartBuilder partBuilder;

    [SerializeField]
    private GameObject partPrefab;

    public void Awake()
    {
        slotButton = GetComponent<Button>();
        itemImage = transform.GetChild(0).GetComponent<Image>();

        partBuilder = Camera.main.GetComponent<PartBuilder>();

        if (partPrefab != null)
        {
            SetPartPrefab(partPrefab);
        }
    }

    public void ButtonClicked()
    {
        if (partPrefab != null)
        {
            partBuilder.SetSelectedPartPrefab(partPrefab);
        }
    }

    public void SetItemTexture(Texture2D texture)
    {
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        itemImage.sprite = sprite;
    }

    public Texture2D GetThumbnail(GameObject gameObject)
    {
        return AssetPreview.GetMiniThumbnail(gameObject);
    }

    public void SetPartPrefab(GameObject prefab)
    {
        partPrefab = prefab;
        // set the button's image to the part's thumbnail
        var texture = GetThumbnail(prefab);

        SetItemTexture(texture);
    }
}
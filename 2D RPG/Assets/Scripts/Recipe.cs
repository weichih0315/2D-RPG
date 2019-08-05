using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour {

    [SerializeField]
    private CraftingMaterial craftingItem;
    public CraftingMaterial CraftingItem
    {
        get
        {
            return craftingItem;
        }
    }

    [SerializeField]
    private CraftingMaterial[] craftingMaterials;
    public CraftingMaterial[] CraftingMaterials
    {
        get
        {
            return craftingMaterials;
        }
    }       
    
    [SerializeField]
    [TextArea]
    private string description;
    public string Description
    {
        get
        {
            return description;
        }
    }

    [SerializeField]
    private Text title;

    private Image highlight;

    private void Awake()
    {
        highlight = GetComponent<Image>();
    }

    private void Start()
    {
        title.text = craftingItem.Item.Name;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = .4f;
        highlight.color = c;
    }

    public void Deselect()
    {
        Color c = highlight.color;
        c.a = 0f;
        highlight.color = c;

    }
}

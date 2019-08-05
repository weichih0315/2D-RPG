using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftSlotUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{    
    [SerializeField]
    private Image icon;
    
    [SerializeField]
    private Text title;
    
    [SerializeField]
    private Text countText;

    private Item item;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateUI(Item item, int count, bool isMaterial)
    {
        this.item = item;
        icon.sprite = item.Icon;
        title.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[item.Quality], item.Name);
        countText.text = string.Empty;

        if (!isMaterial)
        {
            countText.text = count + "";
        }
        else
        {
            int materialCount = Inventory.Instance.GetItemCount(item.Name);

            countText.color = (materialCount >= count) ? Color.white : Color.red;
            countText.text = materialCount + "/" + count;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            TooltipUI.Instance.ShowItemTooltip(rectTransform, item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideItemTooltip();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour {

    private static TooltipUI instance;
    public static TooltipUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TooltipUI>();
            }

            return instance;
        }
    }

    [Header("Item Tooltip")]
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private RectTransform tooltipRectTransform;

    [SerializeField]
    private Text title, description;
    
    public void ShowTooltip(RectTransform targetRectTransform, Item item)
    {
        ShowItemTooltip(targetRectTransform, item);
    }

    public void ShowItemTooltip(RectTransform targetRectTransform, Item item)
    {
        title.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[item.Quality], item.Name) ;

        description.text = item.Description;

        float percentX = Input.mousePosition.x / Screen.width;
        float precentY = Input.mousePosition.y / Screen.height;

        targetRectTransform.pivot = new Vector2(percentX > 0.5f ? 0 : 1, precentY > 0.5f ? 1 : 0);
        tooltipRectTransform.pivot = new Vector2(percentX > 0.5f ? 1 :0, precentY > 0.5f ? 1 : 0);

        canvasGroup.alpha = 1;
        StartCoroutine(LastOneFramePosition(targetRectTransform));
    }
    //LayoutRebuilder.ForceRebuildLayoutImmediate沒用  目前晚一偵為最佳解
    //控制錨點當下  座標晚一偵才更新正確 unity本身順序問題      另解:可以改成不用錨點  使用圖片高度寬度修正座標
    IEnumerator LastOneFramePosition(RectTransform targetRectTransform)
    {
        yield return null;
        tooltipRectTransform.transform.position = targetRectTransform.position;
    }

    public void HideItemTooltip()
    {
        canvasGroup.alpha = 0;
    }
}

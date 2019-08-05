using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    public Bag Bag { get; set; }

    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AddSlots()
    {
        for (int i = 0; i < Bag.Slots.Count; i++)
        {
            SlotUI slotUI = Instantiate(slotPrefab, transform).GetComponent<SlotUI>();
            slotUI.Slot = Bag.Slots[i];
            Bag.Slots[i].SlotUI = slotUI;
        }
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}

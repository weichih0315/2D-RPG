using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler{      

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Bag bag;
    public Bag Bag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value == null)
            {
                icon.sprite = null;
                icon.color = Color.clear;
            }
            else
            {
                icon.sprite = value.ItemSetting.Icon;
                icon.color = Color.white;
            }
            bag = value;
        }
    }    

    public bool IsEmpty { get { return Bag == null; } }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandPickup.Instance.IsEmpty && !IsEmpty)       //Take
            {
                HandPickup.Instance.Item = Bag;
                RemoveBag();
            }
            else if (!HandPickup.Instance.IsEmpty && HandPickup.Instance.Item is Bag)
            {
                if (IsEmpty)
                {
                    PutBag();
                }
                else
                {
                    SwapBag();
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (bag != null)//If we have a bag equipped
            {
                //Open or close the bag
                bag.BagUI.OpenClose();
            }
        }
    }

    private void PutBag()
    {
        Inventory.Instance.AddBag(HandPickup.Instance.Item as Bag, this);
        HandPickup.Instance.Item = null;
    }

    private void SwapBag()
    {
        Bag tempBag = new Bag();
        tempBag = HandPickup.Instance.Item as Bag;

        HandPickup.Instance.Item = Bag;
        RemoveBag();

        Bag = tempBag;
        Inventory.Instance.AddBag(Bag, this);
    }

    public void RemoveBag()
    {
        Inventory.Instance.RemoveBag(Bag);
        Bag = null;
    }
}

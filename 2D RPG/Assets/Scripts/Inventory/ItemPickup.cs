using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour,IInteractable {

    [SerializeField]
    private SpriteRenderer icon;
    public SpriteRenderer Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }
    
    [SerializeField]
    private Item item;
    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            if (value != null)
                value.transform.SetParent(transform);
            item = value;
            UpdateUI();
        }
    }
    
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void RandomForceTime()
    {
        float x = (Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(50, 100);
        float y = Random.Range(200, 400);

        StartCoroutine(AddForcr(new Vector2(x, y), 1));
    }

    IEnumerator AddForcr(Vector2 force,float time)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.AddForce(force);
        float tempTime = 0;
        while (tempTime < time)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }

        rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void OnDrawGizmos()
    {
        if (Item != null)
        {
            Icon.color = Color.white;
            Icon.sprite = Item.Icon;
        }
    }

    public void Interact()
    {
        if (Item.StackSize > 1)
        {
            Item tempItem = Inventory.Instance.StackItem(Item);
            if (tempItem == null)
            {
                Destroy(gameObject);        //完全堆疊增加完
            }
            else
            {
                Item = tempItem;            //無法堆疊剩餘物品
            }
        }
        else if (Inventory.Instance.AddItem(Item))
        {
            Destroy(gameObject);
        }
        else
            Debug.Log("not pickup");            
       
    }

    public void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateUI()
    {
        int Count = Item == null ? 0 : Item.Count;
        if (Count > 1)
        {
            Icon.sprite = Item.Icon;
            Icon.color = Color.white;
        }
        else if (Count == 1)
        {
            Icon.sprite = Item.Icon;
            Icon.color = Color.white;
        }
        else if (Count == 0)
        {
            Icon.color = Color.clear;
        }
    }
}

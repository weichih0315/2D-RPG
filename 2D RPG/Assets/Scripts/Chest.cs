using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private List<Item> items = new List<Item>();

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    [SerializeField]
    private ItemPickup itemPickupPrefab;

    [SerializeField]
    private Transform spawnPoint;    

    private bool isOpen = false;
    
    public void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            foreach (Item item in items)
            {
                ItemPickup itemPickup = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
                itemPickup.Item = item;
                itemPickup.RandomForceTime();
            }

            items.Clear();
        }
    }

    public void StopInteract()
    {
        //暫無
    }
}

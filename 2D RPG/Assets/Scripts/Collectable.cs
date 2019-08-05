using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer image;

    [SerializeField]
    private Sprite defaultImage,emptyImage;

    [SerializeField]
    private GameObject miniMapIndicator;

    private LootTable lootTable;
    public LootTable LootTable
    {
        get
        {
            return lootTable;
        }

        set
        {
            lootTable = value;
        }
    }

    private Coroutine coroutine;

    private void Awake()
    {
        lootTable = GetComponent<LootTable>();
    }

    public void Interact()
    {
        if (!lootTable.IsEmpty)
        {
            Player.Instance.StartGather(this);
        }            
    }

    public void StopInteract()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(StopInteractHandle());
    }

    IEnumerator StopInteractHandle()
    {
        while (Player.Instance.IsCanInteract(transform.position))
        {
            if (lootTable.IsEmpty)
            {
                image.sprite = emptyImage;
                miniMapIndicator.SetActive(false);
            }
            yield return null;
        }

        lootTable.HideLootWindow();
    }
}

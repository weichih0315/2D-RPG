using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour {
    
    public static int sortingOrderBase = 0;
    public static int accuracy = 10;

    [SerializeField]
    private RendererSorter[] rendererSorters;
    [SerializeField]
    private bool runOnlyOnce = false;    

    private float timer;
    private float timerMax = 0.1f;

    private void Start()
    {
        sortingOrderBase = 0;
        accuracy = 100;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMax;

            for (int i = 0; i < rendererSorters.Length; i++)
            {
                rendererSorters[i].SpriteRenderer.sortingOrder = (int)(sortingOrderBase + rendererSorters[i].SortOrder - transform.position.y * accuracy);
            }

            if (runOnlyOnce)
                Destroy(this);
        }
    }
}

[System.Serializable]
public class RendererSorter
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }

        set
        {
            spriteRenderer = value;
        }
    }

    [SerializeField]
    private int sortOrder;
    public int SortOrder
    {
        get
        {
            return sortOrder;
        }

        set
        {
            sortOrder = value;
        }
    }
}

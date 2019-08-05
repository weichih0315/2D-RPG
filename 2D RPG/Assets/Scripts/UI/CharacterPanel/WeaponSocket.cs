using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : EquipSocket
{
    [SerializeField]
    private int sortOrder;

    private bool isBack = false;

    public void LateUpdate()
    {
        if (isBack)
            spriteRenderer.sortingOrder = parentsSpriteRenderer.sortingOrder - 1;        
        else
            spriteRenderer.sortingOrder = parentsSpriteRenderer.sortingOrder + sortOrder;
    }

    public override void DirectionAnimation(float x, float y)
    {
        base.DirectionAnimation(x, y);
        
        if (y != 0)
            isBack = (y == 1) ? true : false;
        else
            isBack = (x < 0) ? true : false;
    }
}

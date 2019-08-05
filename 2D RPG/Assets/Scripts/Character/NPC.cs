using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : LivingEntity {
    
    public UnityEvent OnInteract = new UnityEvent();

    public override LivingEntity Select()
    {
        if (Player.Instance.IsCanInteract(transform.position))
            Interact();

        return base.Select();
    }

    public override void DeSelect()
    {
        base.DeSelect();
    }

    public override void Interact()
    {
        OnInteract.Invoke();
    }    

    public override void StopInteract()
    {        
        base.StopInteract();
    }
}

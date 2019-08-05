using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSocket : MonoBehaviour
{
    public Animator MyAnimator { get; set; }

    protected SpriteRenderer parentsSpriteRenderer;
    protected SpriteRenderer spriteRenderer;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentsSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    public virtual void DirectionAnimation(float x, float y)
    {
        MyAnimator.SetFloat("DirectionX", x);
        MyAnimator.SetFloat("DirectionY", y);
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        animatorOverrideController["Player_Attack_Back"] = animations[0];
        animatorOverrideController["Player_Attack_Front"] = animations[1];
        animatorOverrideController["Player_Attack_Left"] = animations[2];
        animatorOverrideController["Player_Attack_Right"] = animations[3];

        animatorOverrideController["Player_Idle_Back"] = animations[4];
        animatorOverrideController["Player_Idle_Front"] = animations[5];
        animatorOverrideController["Player_Idle_Left"] = animations[6];
        animatorOverrideController["Player_Idle_Right"] = animations[7];

        animatorOverrideController["Player_Walk_Back"] = animations[8];
        animatorOverrideController["Player_Walk_Front"] = animations[9];
        animatorOverrideController["Player_Walk_Left"] = animations[10];
        animatorOverrideController["Player_Walk_Right"] = animations[11];
    }

    public void Dequip()
    {
        animatorOverrideController["Player_Attack_Back"] = null;
        animatorOverrideController["Player_Attack_Front"] = null;
        animatorOverrideController["Player_Attack_Left"] = null;
        animatorOverrideController["Player_Attack_Right"] = null;

        animatorOverrideController["Player_Idle_Back"] = null;
        animatorOverrideController["Player_Idle_Front"] = null;
        animatorOverrideController["Player_Idle_Left"] = null;
        animatorOverrideController["Player_Idle_Right"] = null;

        animatorOverrideController["Player_Walk_Back"] = null;
        animatorOverrideController["Player_Walk_Front"] = null;
        animatorOverrideController["Player_Walk_Left"] = null;
        animatorOverrideController["Player_Walk_Right"] = null;

        spriteRenderer.color = Color.clear;
    }
}
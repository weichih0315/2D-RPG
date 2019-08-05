using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IInteractable{

    [Header("LivingEntity")]

    [SerializeField]
    protected string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    [SerializeField]
    protected Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    [SerializeField]
    protected float maxHp;
    public float MaxHp { get { return maxHp; }
        set {
            maxHp = value;
            if (OnChangeState != null)
                OnChangeState();
        } }
    [SerializeField]
    protected float hp;
    public float Hp {
        get { return hp; }
        set {
            hp = Mathf.Clamp(value, 0, MaxHp);
            if (OnChangeState != null)
                OnChangeState();
        }
    }

    [SerializeField]
    protected float maxMp;
    public float MaxMp
    {
        get { return maxMp; }
        set { maxMp = value;
            if (OnChangeState != null)
                OnChangeState();
        } }
    [SerializeField]
    protected float mp;
    public float Mp {
        get { return mp; }
        set {
            mp = value;
            if (OnChangeState != null)
                OnChangeState();
        }
    }

    [SerializeField]
    protected float maxExp;
    public float MaxExp
    {
        get
        {
            return maxExp;
        }

        set
        {
            maxExp = value;
            if (OnChangeState != null)
                OnChangeState();
        }
    }
    [SerializeField]
    protected float exp;
    public float Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;                   
            if (OnChangeState != null)
                OnChangeState();
        }
    }

    [SerializeField]
    protected int level;
    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
            if (OnChangeState != null)
                OnChangeState();
        }
    }

    [SerializeField]
    protected Animator myAnimator;

    protected bool dead;
    public bool Dead { get { return dead; } }    

    public event Action OnChangeState;
    public event Action OnDead;
    
    protected virtual void Start()
    {
        Hp = maxHp;
        Mp = maxMp;
        MaxExp = Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f));
    }

    protected virtual void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;        

        if (Hp <= 0 && !dead)
            Die();
    }

    public virtual LivingEntity Select()
    {
        return this;
    }

    public virtual void DeSelect()
    {

    }

    public virtual void Interact()
    {

    }

    public virtual void StopInteract()
    {

    }

    public virtual void AddHealth(int health)
    {
        Hp += health;        
    }

    public virtual Vector3 CenterPos { get { return transform.position; } }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        dead = true;
        if (OnDead != null)
            OnDead();
    }
}

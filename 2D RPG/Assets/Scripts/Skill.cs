using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, IUseable{

    [SerializeField]
    private string name;
    public string Name { get { return name; } }

    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField]
    private float castTime;
    public float CastTime { get { return castTime; } }

    [SerializeField]
    private float mpCost;
    public float MpCost { get { return mpCost; } }    

    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    private Color barColor;
    public Color BarColor { get { return barColor; } }

    [SerializeField]
    private GameObject impactEffect;
    public GameObject ImpactEffect
    {
        get
        {
            return impactEffect;
        }

        set
        {
            impactEffect = value;
        }
    }

    [SerializeField]
    [TextArea]
    private string description;
    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }
       
    [SerializeField]
    private Quality quality;
    public Quality Quality
    {
        get
        {
            return quality;
        }

        set
        {
            quality = value;
        }
    }
    
    protected LivingEntity target;
    protected Vector3 mousePos;

    public virtual void Initialize(LivingEntity _target, Vector3 _mousePos)
    {
        target = _target;
        mousePos = _mousePos;
    }

    public virtual void Action()
    {

    }

    public virtual bool Use()
    {        
        return true;
    }
}

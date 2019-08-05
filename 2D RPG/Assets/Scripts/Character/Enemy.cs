using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : LivingEntity {

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float attackSpeed;

    [SerializeField]
    private float scannerRadius;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private LayerMask whatIsTarget;

    [Header("UI")]
    [SerializeField]
    private Image hpFill;

    private Vector2 direction;
    private int directionIndex = 1;
    private bool isAttacking;
    private Coroutine attackCoroutine;
    private Collider2D[] targetsInRadius;
    private LootTable lootTable;
    private BoxCollider2D collider2D;

    private void Awake()
    {
        lootTable = GetComponent<LootTable>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Dead)
            return;

        targetsInRadius = Physics2D.OverlapCircleAll(transform.position, scannerRadius, whatIsTarget);
        direction = Vector2.zero;
        if (targetsInRadius.Length > 0)
        {
            if (isAttacking)
                return;

            if (Vector2.Distance(transform.position, targetsInRadius[0].transform.position) < attackRadius)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
            else
            {
                direction = (targetsInRadius[0].transform.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, targetsInRadius[0].transform.position, moveSpeed * Time.deltaTime);
            }
        }            

        HandleLayers();
    }

    private void HandleLayers()
    {
        if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else if (IsMoveing)
        {
            myAnimator.speed = moveSpeed;
            ActivateLayer("WalkLayer");
            DirectionAnimation(direction.x, direction.y);
        }
        else
        {
            myAnimator.speed = 1;
            ActivateLayer("IdleLayer");
        }
    }

    private bool IsMoveing { get { return direction.x != 0 || direction.y != 0; } }

    private void DirectionAnimation(float x, float y)
    {
        myAnimator.SetFloat("DirectionX", x);
        myAnimator.SetFloat("DirectionY", y);
    }

    private int GetDirectionIndexFromTarget(float x, float y)
    {
        if (x >= 0 && y >= 0)
        {
            if (x >= y)
                return 3;
            else
                return 0;
        }
        else if (x <= 0 && y >= 0)
        {
            if (Mathf.Abs(x) >= y)
                return 2;
            else
                return 0;
        }
        else if (x <= 0 && y <= 0)
        {
            if (Mathf.Abs(x) >= Mathf.Abs(y))
                return 2;
            else
                return 1;
        }
        else if (x >= 0 && y >= 0)
        {
            if (x >= Mathf.Abs(y))
                return 3;
            else
                return 1;
        }

        return 1;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        myAnimator.SetBool("Attack", isAttacking);
        yield return null;
    }

    public void StopAttack()
    {
        isAttacking = false;
        myAnimator.SetBool("Attack", isAttacking);
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        Player.Instance.TakeDamage(damage);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PopupTextManager.Instance.CreateText(collider2D.bounds.center + new Vector3( 0,collider2D.size.y / 2,0), damage + "", PopupTextType.DAMAGE, false, PopupTextDirection.Top);
        hpFill.fillAmount = hp / maxHp;
    }

    public override void Die()
    {
        base.Die();
        ActivateLayer("DeathLayer");
        myAnimator.SetBool("Die", dead);
        GameManager.Instance.EnemyOnDaed(this);
        Player.Instance.AddExp(EXPManager.CalculateEXP(this));
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public override LivingEntity Select()
    {
        //暫無 顯示圈圈之類的
        if (dead && Player.Instance.IsCanInteract(transform.position))
        {            
            Interact();
        }            
        
        return base.Select();
    }

    public override void DeSelect()
    {
        base.DeSelect();
    }

    private Coroutine coroutine;
    public override void Interact()
    {
        if (!lootTable.IsEmpty)
        {
            lootTable.ShowLootWindow();
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(StopInteractHandle());
        }        
    }

    public override void StopInteract()
    {
        lootTable.HideLootWindow();
    }

    IEnumerator StopInteractHandle()
    {
        while (Player.Instance.IsCanInteract(transform.position))
        {
            yield return null;
        }
        StopInteract();
    }

    public override Vector3 CenterPos { get { return collider2D.bounds.center; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,.3f);
        Gizmos.DrawSphere(transform.position, scannerRadius);
    }
}

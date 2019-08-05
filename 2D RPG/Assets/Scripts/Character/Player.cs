using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {

    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    [Header("Player")]
    [SerializeField]
    private float moveSpeed;
    
    [Header("Attack")]
    public Transform[] weaponHolds;
    [SerializeField]
    private LayerMask whatIsCanHit;
    [SerializeField]
    private LivingEntity target;
    public LivingEntity Target { get { return target; } }

    [Header("Interactable")]
    [SerializeField]
    private float interactRadius;

    [Header("TabEnemy")]
    [SerializeField]
    private float tabRadius;
    [SerializeField]
    private LayerMask whatIsEnemy;
    public Collider2D[] EnemyTargets { get; private set; }

    [Header("Gather")]
    [SerializeField]
    private float GatherTime;

    [Header("Craft")]
    [SerializeField]
    private float CraftTime;

    [Header("Effect")]
    [SerializeField]
    private Animator levelUpEffect;

    [Header("Minimap")]
    [SerializeField]
    private GameObject minimapIcon;
    
    [SerializeField]
    private EquipSocket[] equipAnimators;

    private Vector2 direction;
    private int directionIndex = 1;
    private bool isAttacking, isGathering;
    private Coroutine actionCoroutine;
    private Vector3[] movePath;
    private int movePathIndex;
    private Coroutine followPathCoroutine;
    private bool isClickMove;
    private Vector3 clickMovePoint;

    private Rigidbody2D myRigbody2D;

    public event Action<LivingEntity> OnChangeTarget;

    private bool IsMoveing { get { return direction.x != 0 || direction.y != 0; } }

    public bool IsAction { get { return isAttacking || isGathering; } }

    public int DirectionIndex
    {
        get
        {
            return directionIndex;
        }

        set
        {
            if (value == 0)
            {
                minimapIcon.transform.eulerAngles = new Vector3(0,0,0);
            }
            else if (value == 1)
            {
                minimapIcon.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (value == 2)
            {
                minimapIcon.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (value == 3)
            {
                minimapIcon.transform.eulerAngles = new Vector3(0, 0, 270);
            }

            directionIndex = value;
        }
    }

    private void Awake()
    {
        myRigbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Dead)
            return;

        EnemyTargets = Physics2D.OverlapCircleAll(transform.position, tabRadius, whatIsEnemy);

        GetInput();
        HandleLayers();
    }

    private void FixedUpdate()
    {
        if (!isClickMove)
            myRigbody2D.velocity = direction.normalized * moveSpeed;
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, clickMovePoint, moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void ClickMove(Vector3 _position)
    {
        PathRequestManager.RequestPath(transform.position, _position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            movePath = newPath;
            movePathIndex = 0;
            StopClickMove();
            followPathCoroutine = StartCoroutine("FollowPath");
        }
    }

    private void StopClickMove()
    {
        isClickMove = false;
        if (followPathCoroutine != null)
            StopCoroutine(followPathCoroutine);
    }

    IEnumerator FollowPath()
    {
        isClickMove = true;
        Vector3 currentWaypoint = movePath[0];
        while (movePathIndex < movePath.Length)
        {
            if (transform.position == currentWaypoint)
            {
                movePathIndex++;
                if (movePathIndex >= movePath.Length)
                {
                    yield break;
                }
                currentWaypoint = movePath[movePathIndex];
            }
            clickMovePoint = currentWaypoint;
            LookAtTarget(currentWaypoint);
            yield return null;
        }

        StopClickMove();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (!isAttacking)
        {
            if (Input.GetKey(KeybindManager.Instance.Keybind["Up"]))
            {
                DirectionIndex = 0;
                direction += Vector2.up;
            }
            if (Input.GetKey(KeybindManager.Instance.Keybind["Down"]))
            {
                DirectionIndex = 1;
                direction += Vector2.down;
            }
            if (Input.GetKey(KeybindManager.Instance.Keybind["Left"]))
            {
                DirectionIndex = 2;
                direction += Vector2.left;
            }
            if (Input.GetKey(KeybindManager.Instance.Keybind["Right"]))
            {
                DirectionIndex = 3;
                direction += Vector2.right;
            }

            if (direction != Vector2.zero)
                StopClickMove();
        }

        //Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(50);
        }
    }

    private void HandleLayers()
    {
        if (!Dead)
        {
            myAnimator.speed = 1;            

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
                ActivateLayer("IdleLayer");
            }

            foreach (EquipSocket equipAnimator in equipAnimators)
            {
                equipAnimator.MyAnimator.speed = myAnimator.speed;
            }
        }
        else
        {
            ActivateLayer("DeadLayer");
        }
    }

    protected override void ActivateLayer(string layerName)
    {
        base.ActivateLayer(layerName);

        foreach (EquipSocket equipAnimator in equipAnimators)
        {
            equipAnimator.ActivateLayer(layerName);
        }
    }

    private void DirectionAnimation(float x , float y)
    {
        myAnimator.SetFloat("DirectionX", x);
        myAnimator.SetFloat("DirectionY", y);

        foreach (EquipSocket equipAnimator in equipAnimators)
        {
            equipAnimator.DirectionAnimation(x, y);
        }
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

    private void LookAtTarget(Vector3 position)
    {
        Vector3 targetDirection = (position - transform.position).normalized;
        DirectionAnimation(targetDirection.x, targetDirection.y);
        DirectionIndex = GetDirectionIndexFromTarget(targetDirection.x, targetDirection.y);
    }

    public void StartAttack(Skill skill)
    {        
        if (mp > skill.MpCost)
        {
            Mp -= skill.MpCost;
            actionCoroutine = StartCoroutine(Attack(skill));
        }
        else
        {
            Debug.Log("Need More Mp");
        }
    }

    private void AttackAnimation()
    {
        myAnimator.SetBool("Attack", isAttacking);
        foreach (EquipSocket equipAnimator in equipAnimators)
        {
            equipAnimator.MyAnimator.SetBool("Attack", isAttacking);
        }
    }

    private IEnumerator Attack(Skill skill)
    {
        isAttacking = true;
        AttackAnimation();

        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LivingEntity targetTemp = target;

        LookAtTarget((target != null)? target.transform.position : mouseWorldPoint);
        CastBarUI.Instance.PopCastBar(skill.Icon, skill.Name, skill.CastTime, skill.BarColor);        

        yield return new WaitForSeconds(skill.CastTime);
        
        Skill skilltemp = Instantiate(skill, weaponHolds[DirectionIndex].position, Quaternion.identity);
        skilltemp.Initialize(targetTemp, mouseWorldPoint);
        skilltemp.Action();

        StopAttack();
    }

    public void StopAttack()
    {
        isAttacking = false;
        AttackAnimation();

        if (actionCoroutine != null)
            StopCoroutine(actionCoroutine);

        CastBarUI.Instance.StopCating();
    }

    public void StartGather(Collectable collectable)
    {
        actionCoroutine = StartCoroutine(Gather(collectable));
    }

    private IEnumerator Gather(Collectable collectable)
    {
        LootTable lootTable = collectable.LootTable;
        Item item = lootTable.LootItems[0];
        isAttacking = true;                         //之後要改成isGathering
        AttackAnimation();
        LookAtTarget(collectable.transform.position);

        CastBarUI.Instance.PopCastBar(item.Icon, item.Name, GatherTime, Color.white);

        yield return new WaitForSeconds(GatherTime);
        lootTable.ShowLootWindow();
        collectable.StopInteract();
        StopAttack();
    }

    public IEnumerator StartCraft(Recipe recipe, int craftCount)
    {
        for (int i = 0; i < craftCount; i++)
        {
            yield return actionCoroutine = StartCoroutine(Craft(recipe));
        }    
    }

    private IEnumerator Craft(Recipe recipe)
    {
        Item item = recipe.CraftingItem.Item;

        CastBarUI.Instance.PopCastBar(item.Icon, item.Name, CraftTime, Color.yellow);
        yield return new WaitForSeconds(CraftTime);

        Item craftItem = Instantiate(item);
        craftItem.Count = recipe.CraftingItem.Count;

        if (Inventory.Instance.IsCanAddItem(craftItem))
        {
            if (craftItem.StackSize > 1)
            {
                Inventory.Instance.StackItem(craftItem);
            }
            else
            {
                Inventory.Instance.AddItem(craftItem);
            }

            foreach (CraftingMaterial craftingMaterial in recipe.CraftingMaterials)
            {
                Inventory.Instance.RemoveItem(craftingMaterial.Item.Name, craftingMaterial.Count);
            }            
        }
        else
        {
            Debug.Log("Inventory is full");
            Destroy(craftItem.gameObject);
        }
    }

    public void SetTarget(LivingEntity tempTarget)
    {
        if (target != null)
        {
            target.OnDead -= TargetOnDead;
        }            

        if (tempTarget != null)
        {
            target = tempTarget;
            target.OnDead += TargetOnDead;
        }
        else
            target = null;

        if (OnChangeTarget != null)
            OnChangeTarget(target);
    }

    public void TargetOnDead()
    {
        SetTarget(null);
    }

    public bool IsCanInteract(Vector2 interactablePosition) {
        return Vector2.Distance(interactablePosition,transform.position) < interactRadius;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PopupTextManager.Instance.CreateText(transform.position + new Vector3(0, 1.5f), damage + "", PopupTextType.DAMAGE, false, PopupTextDirection.Top);
    }

    public override void AddHealth(int health)
    {
        base.AddHealth(health);
        PopupTextManager.Instance.CreateText(transform.position + new Vector3(0,1.5f), health+"",PopupTextType.HEAL,false,PopupTextDirection.Top);
    }

    private Coroutine levelUpCoroutine;
    public void AddExp(int exp)
    {
        Exp += exp;
        PopupTextManager.Instance.CreateText(transform.position + new Vector3(0, 1.5f), exp + "", PopupTextType.EXP, false, PopupTextDirection.Top);

        if (Exp >= MaxExp)
        {
            if (levelUpCoroutine != null)
                StopCoroutine(levelUpCoroutine);
            levelUpCoroutine = StartCoroutine(LevelUp());
        }            
    }

    IEnumerator LevelUp()
    {
        while (Exp >= MaxExp)
        {
            if (StateUI.Instance.ExpIsFull)
            {
                levelUpEffect.SetTrigger("LevelUp");
                Exp -= MaxExp;
                Level++;
                MaxExp = Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f));
                StateUI.Instance.ExpReset();
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0, 0.3f);
        Gizmos.DrawSphere(transform.position, interactRadius);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, tabRadius);

        if (movePath != null)
        {
            for (int i = movePathIndex; i < movePath.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(movePath[i], Vector3.one * Grid.instance.NodeRadius);

                if (i == movePathIndex)
                {
                    Gizmos.DrawLine(transform.position, movePath[i]);
                }
                else
                {
                    Gizmos.DrawLine(movePath[i - 1], movePath[i]);
                }
            }
        }
    }
}

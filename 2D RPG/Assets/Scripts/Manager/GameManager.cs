using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }


    public int money = 1000;//Test

    [SerializeField]
    private float timeBetweenClicks;

    [SerializeField]
    private Player player;

    [SerializeField]
    private LayerMask whatIsCanClick;

    [SerializeField]
    private GameObject mouseClickEffect;

    private Camera mainCamera;

    private LivingEntity currentTarget;
    public LivingEntity CurrentTarget
    {
        get
        {
            return currentTarget;
        }

        set
        {
            if (currentTarget != null)
                currentTarget.DeSelect();
            currentTarget = value;
            if (value != null)
                currentTarget.Select();

            player.SetTarget(currentTarget);            
        }
    }

    private int tabTargetIndex = 0;

    private int clickCount;
    private float firstClickTime;
    private bool continueAllowed = true;
    private RaycastHit2D clickHit;

    public event Action<Enemy> EnemyOnDeadEvent;

    private Vector3 MouseWorldPosition {
        get {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = -mainCamera.transform.position.z;
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        SaveDataManager.Instance.Load();
    }    

    private void Update ()
    {
        Click();
        TabTarget();
    }

    private void Click()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            clickHit = Physics2D.Raycast(MouseWorldPosition, Vector2.zero, Mathf.Infinity, whatIsCanClick);

            if (clickHit.collider != null)
            {
                LivingEntity HitTargrt = clickHit.transform.GetComponent<LivingEntity>();

                if (HitTargrt != null) //有生命  有狀態可鎖定標記的
                {
                    CurrentTarget = HitTargrt;
                }
                else                    //一般互動物件
                {
                    IInteractable interactable = clickHit.collider.gameObject.GetComponent<IInteractable>();
                    if (Player.Instance.IsCanInteract(clickHit.transform.position))
                        interactable.Interact();
                }
            }
            else
            {
                CurrentTarget = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            clickCount += 1;
        }

        if (clickCount == 1 && continueAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Destroy(Instantiate(mouseClickEffect, MouseWorldPosition, Quaternion.identity), 0.3f);
            Player.Instance.ClickMove(MouseWorldPosition);
        }
    }

    private IEnumerator DoubleClickDetection()
    {
        continueAllowed = false;
        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCount == 2)
            {
                DoubleClick();
                break;
            }
            yield return null;
        }

        clickCount = 0;
        continueAllowed = true;
        OneClick();
    }

    private void OneClick()
    {       
        
    }

    private void DoubleClick()
    {
        if (clickHit.collider != null)
        {

        }
        else
        {
        }
    }

    public void TabTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (player.EnemyTargets.Length > 0)
            {                
                if (player.Target != null)
                {
                    Enemy playerTarget = player.Target.GetComponent<Enemy>();
                    for (int i = 0; i < player.EnemyTargets.Length; i++)
                    {
                        Enemy enemyTarget = player.EnemyTargets[i].GetComponent<Enemy>();
                        if (enemyTarget == playerTarget)
                        {
                            tabTargetIndex = (i + 1 < player.EnemyTargets.Length) ? i + 1 : 0;

                            CurrentTarget = player.EnemyTargets[tabTargetIndex].GetComponent<Enemy>();
                            return;
                        }
                    }

                    CurrentTarget = player.EnemyTargets[0].GetComponent<Enemy>();
                }
                else
                    CurrentTarget = player.EnemyTargets[0].GetComponent<Enemy>();
            }
        }
    }

    public void EnemyOnDaed(Enemy enemy)
    {
        if (EnemyOnDeadEvent != null)
        {
            EnemyOnDeadEvent.Invoke(enemy);
        }
    }
}

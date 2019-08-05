using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour {

    private static StateUI instance;
    public static StateUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StateUI>();
            }

            return instance;
        }
    }

    [Header("Player State UI")]
    [SerializeField]
    private Image playerIcon;
    [SerializeField]
    private Image playerHpBar;
    [SerializeField]
    private Image playerHpBarEffect;
    [SerializeField]
    private Text playerHpText;
    [SerializeField]
    private Image playerMpBar;
    [SerializeField]
    private Image playerMpBarEffect;
    [SerializeField]
    private Text playerMpText;
    [SerializeField]
    private Image playerExpBar;
    [SerializeField]
    private Text playerExpText;

    [SerializeField]
    private Text playerLevelText;

    [Header("Target State UI")]
    [SerializeField]
    private CanvasGroup targetStateCanvasGroup;
    [SerializeField]
    private Image targetIcon;
    [SerializeField]
    private Image targetHpBar;
    [SerializeField]
    private Image targetHpBarEffect;
    [SerializeField]
    private Text targetHpText;
    [SerializeField]
    private Image targetMpBar;
    [SerializeField]
    private Image targetMpBarEffect;
    [SerializeField]
    private Text targetMpText;

    [SerializeField]
    private Text targetLevelText;

    [Header("Effect Speed")]
    [SerializeField]
    private float lerpSpeed;

    private Player player;
    private LivingEntity target = null;
    private Coroutine playerEffectCoroutine, targetEffectCoroutine;

    public bool ExpIsFull { get { return playerExpBar.fillAmount == 1; } }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.OnChangeState += PlayerOnChangeState;
        player.OnChangeTarget += PlayerOnChangeTarget;
    }

    private void OnDisable()
    {
        player.OnChangeState -= PlayerOnChangeState;
        player.OnChangeTarget -= PlayerOnChangeTarget;
    }

    private void UpdatePlayerStateUI(Sprite icon,float hp, float maxHp, float mp, float maxMp, float exp, float maxExp, int level)
    {
        playerIcon.sprite = icon;
        playerHpBar.fillAmount = hp / maxHp;
        playerHpText.text = hp + " / " + maxHp;

        playerMpBar.fillAmount = mp / maxMp;
        playerMpText.text = mp + " / " + maxMp;
        playerExpText.text = exp + " / " + maxExp;
        playerLevelText.text = level + "";
    }

    private void PlayerOnChangeState()
    {
        UpdatePlayerStateUI(player.Icon, player.Hp, player.MaxHp, player.Mp, player.MaxMp, player.Exp, player.MaxExp, player.Level);

        if (playerEffectCoroutine != null)
            StopCoroutine(playerEffectCoroutine);
        playerEffectCoroutine = StartCoroutine(PlayerStateUIEffect());
    }

    private void UpdateTargetStateUI(Sprite icon, float hp, float maxHp, float mp, float maxMp,int level)
    {
        targetIcon.sprite = icon;
        targetHpBar.fillAmount = hp / maxHp;
        targetHpText.text = Mathf.Clamp(hp, 0, maxHp) + " / " + maxHp;
        targetMpBar.fillAmount = mp / maxMp;
        targetMpText.text = Mathf.Clamp(mp, 0, maxMp) + " / " + maxMp;        

        if (target.Level >= Player.Instance.Level + 5)              // >5
        {
            targetLevelText.color = Color.red;
        }
        else if (target.Level >= Player.Instance.Level + 3)         //3~4
        {
            targetLevelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.Level >= Player.Instance.Level - 2)         //-2~2
        {
            targetLevelText.color = Color.yellow;
        }
        else if (target.Level <= Player.Instance.Level - 3 && target.Level > EXPManager.CalculateGrayLevel())
        {
            targetLevelText.color = Color.green;
        }
        else
        {
            targetLevelText.color = Color.grey;
        }

        targetLevelText.text = level + "";
    }

    private void TargetOnChangeState()
    {
        UpdateTargetStateUI(target.Icon, target.Hp, target.MaxHp, target.Mp, target.MaxMp, target.Level);

        if (targetEffectCoroutine != null)
            StopCoroutine(targetEffectCoroutine);
        targetEffectCoroutine = StartCoroutine(TargetStateUIEffect());
    }

    private void PlayerOnChangeTarget(LivingEntity tempTarget)
    {
        if (target != null)
            target.OnChangeState -= TargetOnChangeState;

        if (tempTarget != null)
        {            
            target = tempTarget;
            target.OnChangeState += TargetOnChangeState;
            ShowTargetStateUI();
        }
        else
        {
            target = null;
            HideTargetStateUI();
        }
    }

    public void ShowTargetStateUI()
    {
        targetHpBarEffect.fillAmount = target.Hp / target.MaxHp;
        targetMpBarEffect.fillAmount = target.Mp / target.MaxMp;
        TargetOnChangeState();
        
        targetStateCanvasGroup.alpha = 1;
        targetStateCanvasGroup.blocksRaycasts = true;
    }

    public void HideTargetStateUI()
    {
        targetStateCanvasGroup.alpha = 0;
        targetStateCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator PlayerStateUIEffect()
    {
        while (playerHpBar.fillAmount != playerHpBarEffect.fillAmount || playerMpBar.fillAmount != playerMpBarEffect.fillAmount || playerExpBar.fillAmount != player.Exp / player.MaxExp)
        {
            playerHpBarEffect.fillAmount = Mathf.MoveTowards(playerHpBarEffect.fillAmount, playerHpBar.fillAmount, Time.deltaTime * lerpSpeed);     //Lerp: 快到慢 MoveTowards:等速
            playerMpBarEffect.fillAmount = Mathf.MoveTowards(playerMpBarEffect.fillAmount, playerMpBar.fillAmount, Time.deltaTime * lerpSpeed);
            playerExpBar.fillAmount = Mathf.MoveTowards(playerExpBar.fillAmount, player.Exp / player.MaxExp, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        playerEffectCoroutine = null;
    }

    private IEnumerator TargetStateUIEffect()
    {
        while (targetHpBar.fillAmount != targetHpBarEffect.fillAmount || targetMpBar.fillAmount != targetMpBarEffect.fillAmount)
        {
            targetHpBarEffect.fillAmount = Mathf.MoveTowards(targetHpBarEffect.fillAmount, targetHpBar.fillAmount, Time.deltaTime * lerpSpeed);
            targetMpBarEffect.fillAmount = Mathf.MoveTowards(targetMpBarEffect.fillAmount, targetMpBar.fillAmount, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        targetEffectCoroutine = null;
    }

    public void ExpReset()
    {
        playerExpBar.fillAmount = 0;
        if (playerEffectCoroutine != null)
            StopCoroutine(playerEffectCoroutine);
        playerEffectCoroutine = StartCoroutine(PlayerStateUIEffect());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupTextDirection {Top,Right,Left }

public class PopupText : MonoBehaviour {

    [SerializeField]
    private Vector2 froce;
    
    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private Color damageColor, healColor, critColor, expColor;

    private Text text;
    public string Text
    {
        get
        {
            return text.text;
        }

        set
        {
            text.text = value;
        }
    }

    private Rigidbody2D rigidbody2D;    

    public Color Color { get { return text.color; } set { text.color = value; } }
       
    private PopupTextDirection direction;
    public PopupTextDirection Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    private bool crit;
    public bool Crit
    {
        get
        {
            return crit;
        }

        set
        {
            crit = value;
        }
    }

    private PopupTextType type;
    public PopupTextType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }


    private void Awake()
    {
        text = GetComponent<Text>();
        rigidbody2D = GetComponent <Rigidbody2D> ();
    }
    
    public void Move()
    {
        string before = string.Empty;
        string after = string.Empty;

        if (type == PopupTextType.DAMAGE)
        {
            before = "-";
            if (Crit)
                Color = critColor;
            else
                Color = damageColor;
        }
        else if (type == PopupTextType.HEAL)
        {
            before = "+";
            Color = healColor;
        }
        else if (type == PopupTextType.EXP)
        {
            before = "+";
            Color = expColor;
            after = " EXP";
        }

        Text = before + Text + after;

        if (Direction == PopupTextDirection.Right)
            rigidbody2D.AddForce(froce);
        else if (Direction == PopupTextDirection.Left)
            rigidbody2D.AddForce(froce * new Vector2(-1, 1));
        else if (Direction == PopupTextDirection.Top)
            rigidbody2D.AddForce(froce * Vector2.up);

        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;
        Vector3 startScale = transform.localScale;
        float rate = 1.0f / lifeTime;
        float progress = 0f;

        while (progress < 1)
        {
            progress += rate * Time.deltaTime;
            Color tempColor = text.color;
            tempColor.a = Mathf.Lerp(startAlpha, 0.5f, progress);            
            text.color = tempColor;

            if (Crit)
                transform.localScale = Vector3.Lerp(startScale * 1.5f, startScale * 0.75f, progress);
            else
                transform.localScale = Vector3.Lerp(startScale, startScale * 0.5f, progress);
            yield return null;
        }

        Destroy(gameObject);
    }
}

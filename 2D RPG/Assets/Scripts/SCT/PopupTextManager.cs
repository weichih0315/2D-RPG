using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupTextType {DAMAGE, HEAL, EXP}

public class PopupTextManager : MonoBehaviour
{
    private static PopupTextManager instance;
    public static PopupTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PopupTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private PopupText popupTextPrefab;

    [SerializeField]
    private Transform popupTextTransform;

    private void Update()
    {
       /* Vector3 temp = Player.Instance.transform.position + new Vector3(0, 1.5f);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            CreateText(temp, "1000", PopupTextType.DAMAGE, false, PopupTextDirection.Right);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateText(temp, "1000", PopupTextType.HEAL, false, PopupTextDirection.Left);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateText(temp, "1000", PopupTextType.DAMAGE, true, PopupTextDirection.Top);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Player.Instance.AddExp(100000);
        }*/
    }

    public void CreateText(Vector2 position, string text, PopupTextType type, bool crit, PopupTextDirection direction)
    {
        PopupText popupText = Instantiate(popupTextPrefab, popupTextTransform);
        popupText.transform.position = position;
        popupText.Text = text;
        popupText.Type = type;
        popupText.Crit = crit;
        popupText.Direction = direction;
        
        popupText.Move();
    }
}

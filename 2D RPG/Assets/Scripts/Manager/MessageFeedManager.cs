using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour {

    private static MessageFeedManager instance;
    public static MessageFeedManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MessageFeedManager>();
            }

            return instance;
        }

    }

    [SerializeField]
    private GameObject messagePrefab;

    [SerializeField]
    private Transform TopArea;

    public void WriteMessage(string message)
    {

       GameObject go = Instantiate(messagePrefab, TopArea);

        go.GetComponent<Text>().text = message;

        //go.transform.SetAsFirstSibling();     //會有layerSort的BUG  物件重疊  暫時不知原因 待解

        Destroy(go, 2);
    }
}

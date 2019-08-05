using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindButton : MonoBehaviour {

    [SerializeField]
    private string key;

    private Button button;
    private bool readToBindKey = false;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        readToBindKey = true;
    }

    private void OnGUI()
    {
        if (readToBindKey)
        {
            Event e = Event.current;

            if (e.isMouse)
                readToBindKey = false;

            if (e.isKey) //If the event is a key, then we change the keybind
            {
                KeybindManager.Instance.BindKey(key, e.keyCode);
                readToBindKey = false;
            }
        }
    }
}

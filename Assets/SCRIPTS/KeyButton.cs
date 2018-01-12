using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    public string Hotkey;
    Button buttonMe;
    
    void Start()
    {
        buttonMe = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetButtonDown(Hotkey)) 
            buttonMe.onClick.Invoke();
    }
}
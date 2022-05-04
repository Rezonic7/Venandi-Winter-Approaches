using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationText : MonoBehaviour
{
    private Text text;
    public string textValue;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    public void ChangeText(string newText)
    {
        text.text = newText;
    }
}
   

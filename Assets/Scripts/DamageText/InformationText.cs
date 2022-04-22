using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationText : MonoBehaviour
{
    private Text text;
    public string textValue = "test";
    private void Start()
    {
        text = GetComponentInChildren<Text>();
    }
    private void Update()
    {
        text.text = textValue;
    }
}
   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HubButtonHiglights : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string buttonInfo;
    private GameObject highlightInfo;
    private Text infoText;

    public void Awake()
    {
        highlightInfo = GameObject.FindWithTag("HighlightInfo")?.gameObject;
        infoText = highlightInfo.transform.GetChild(0).GetComponent<Text>();
    }
    public void Start()
    {
        
        if(highlightInfo.activeSelf)
        {
            highlightInfo.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightInfo.SetActive(true);
        infoText.text = buttonInfo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoText.text = "";
        highlightInfo.SetActive(false);
    }
}

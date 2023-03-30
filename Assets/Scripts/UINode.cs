using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UINode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Node.Type type;
    bool hovering = false;
    bool dragging;
    Vector3 returnPos;
    RectTransform rt;
    TMP_Text text;
    ToolkitMain main;
    public void OnPointerEnter(PointerEventData data) {
        
        hovering=true;
    }

    public void OnPointerExit(PointerEventData data) {
        
        hovering=false;
    }

    void Start() {
        rt = GetComponent<RectTransform>();
        returnPos = rt.position;
        text = GetComponentInChildren<TMP_Text>();
        text.text = Node.name[type];
        main = GetComponentInParent<ToolkitMain>();
    }

    void Update() {
        if(hovering && Input.GetKeyDown(KeyCode.Mouse0) && !dragging) {
            
            dragging=true;
            PlayerPrefs.SetInt("HoveringUI",true?1:0);
            returnPos = rt.position;
        }

        if(dragging && Input.GetKeyUp(KeyCode.Mouse0)) {
            main.MakeNode(type, Camera.main.ScreenToWorldPoint(rt.position));
            rt.position = returnPos;
            dragging=false;
            PlayerPrefs.SetInt("HoveringUI",false?1:0);
        }

        if(dragging) {
            rt.position = Input.mousePosition;
        }
    }
}

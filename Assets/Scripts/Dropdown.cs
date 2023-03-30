using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;

public class Dropdown : MonoBehaviour
{
    [SerializeField] GameObject tabPrefab;
    [SerializeField] GameObject visibilityParent;
    [SerializeField] GameObject tabParent;
    List<GameObject> tabObjects = new List<GameObject>();
    public UnityEvent onCall;
    void Start() {
        //AddTab("Test").AddListener( ()=>{ print("Test"); } );
    }

    void Update() {
        
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            transform.position = GetNearestUnobscuredPosition(Input.mousePosition);
            
            
            
            if(IsOpen()) Close();
            else Open();
        }
    }

    Vector3 GetNearestUnobscuredPosition(Vector3 mousePos) {
        float height = GetComponent<RectTransform>().rect.height;
        float windowHeight = Screen.height;

        //print(mousePos.y - height);
        //print(windowHeight);

        if((mousePos.y - height) < 0) {
            //print("Offscreen");
            return mousePos - new Vector3(0, mousePos.y - height, 0);
        }

        return mousePos;

    }

    void LateUpdate() {
        if(IsPointerOverUIObject()) {
            //PlayerPrefs.SetInt("HoveringUI",1);
        }
        else {
            if(Input.GetKeyDown(KeyCode.Mouse0) && IsOpen()) Close();
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void Open() {
        onCall.Invoke();
        visibilityParent.SetActive(true);
    }

    public void Close(bool removeTabs=true) {
        if(removeTabs) RemoveAllTabs();
        visibilityParent.SetActive(false);
    }

    public bool IsOpen() {
        return visibilityParent.activeSelf;
    }

    public UnityEvent AddTab(string title) {
        GameObject tabObject = Instantiate(tabPrefab);
        tabObject.transform.SetParent(tabParent.transform);
        tabObject.name = title;
        tabObjects.Add(tabObject);

        TMP_Text tmpText = tabObject.GetComponentInChildren<TMP_Text>();
        tmpText.text = title;

        Button button = tabObject.GetComponent<Button>();
        button.onClick.AddListener(()=>{Close();});
        return button.onClick;

    }

    public void RemoveTab(string title) {

    }

    public void RemoveAllTabs() {
        foreach(GameObject tab in tabObjects.ToArray()) {
            DestroyImmediate(tab);
        }
        tabObjects.Clear();
    }
}

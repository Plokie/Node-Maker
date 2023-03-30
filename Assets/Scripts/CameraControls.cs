using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    Camera cam;

    Vector2 prevPos;
    // Start is called before the first frame update

    void Awake() {
        PlayerPrefs.SetInt("HoveringUI",false?1:0);
    }
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * 200f;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 20);

        if(Input.GetKey(KeyCode.Mouse0) && !HoveringElement()) {
            Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            transform.Translate((prevPos - mousePos) * 0.01f, Space.Self);
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        prevPos = Input.mousePosition;
    }

    bool HoveringElement() {
        return PlayerPrefs.GetInt("HoveringUI")>0;
    }
}

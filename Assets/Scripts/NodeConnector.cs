using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnector : MonoBehaviour
{
    [SerializeField] Material lineMaterial;
    public NodeElement parent;
    public NodeConnector target;
    SpriteRenderer sr;
    bool hovering;
    bool dragging;
    public bool connectedTo;
    LineRenderer line;
    List<Vector3> linePositions = new List<Vector3>(){};

    void OnMouseOver() {
        
        hovering=true;
        //print("over "+name);
    }

    void OnMouseExit() {
        
        hovering=false;
        //print("exit "+name);
    }

    void Start() {
        sr = GetComponent<SpriteRenderer>();

        linePositions = new List<Vector3>(){
            transform.position, transform.position
        };

        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount=2;
        line.SetPosition(0, linePositions[0]);
        line.SetPosition(1, linePositions[1]);
        //line.materials[0] = (Material)Resources.Load("Default-Line", typeof(Material));
        //line.materials[0] = lineMaterial;
        line.sharedMaterial = lineMaterial;
        line.startWidth = .2f; line.endWidth = .2f;

        
    }

    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z=0;
        float dist = Vector3.Distance(transform.position, mousePos);
        if(dist < .3f) {
            sr.enabled=true;
            // if(dist < .1f) {
            //     if(Input.GetKeyDown(KeyCode.Mouse0)) {

            //     }
            //     else if(Input.GetKeyUp(KeyCode.Mouse0)) {
                    
            //     }
            //     else if(Input.GetKey(KeyCode.Mouse0)){

            //     }
            // }
        }
        else
        {
            sr.enabled=false;
        }

        if(hovering && Input.GetKeyDown(KeyCode.Mouse0) && !connectedTo) {
            
            dragging=true;
            PlayerPrefs.SetInt("HoveringUI",true?1:0);
        }

        if(dragging && Input.GetKeyUp(KeyCode.Mouse0)) {
            //transform.position = returnPos;
            dragging=false;
            PlayerPrefs.SetInt("HoveringUI",false?1:0);

            foreach(GameObject node in GameObject.FindGameObjectsWithTag("NodeConnector")) {
                float distance = Vector3.Distance(node.transform.position, mousePos);
                if(distance < 0.15f) {
                    NodeConnector nc = node.GetComponent<NodeConnector>();
                    if(nc==this) {
                        //Disconnect
                        //print("Disconnected from all");
                        if(target!=null) {
                            target.connectedTo=false;
                            target.parent.inputCount--;
                            //parent.outputCount-=2;
                        }
                        parent.outputCount--;

                        target=null;
                    }
                    else
                    if(parent.AddOutputNode() && nc.parent.AddInputNode()){
                        //print("Connected to "+nc.parent.type);
                        nc.connectedTo = true;

                        target = nc;
                    }
                }
            }
        }

        line.SetPosition(0, transform.position);
        if(target == null) {
            line.SetPosition(1, transform.position);
        }
        else
        {
            line.SetPosition(1, target.transform.position);
        }

        if(dragging) {
            
            line.SetPosition(1, mousePos);
            //linePositions[1] = mousePos;

            //transform.position = mousePos;
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        
        
    }
}

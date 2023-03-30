using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeElement : MonoBehaviour
{
    [SerializeField] GameObject featureIconPrefab;
    [SerializeField] GameObject featureIconParent;
    public Node.Type type = Node.Type.None;
    public Dictionary<Node.Feature, bool> features = new Dictionary<Node.Feature, bool>();
    //public Node node;
    public List<Transform> exits;
    TMP_Text tmpName;
    bool hovering;
    bool dragging;
    Vector3 returnPos;
    public int inputCount;
    public int outputCount;
    public int id;
    List<GameObject> featureIcons = new List<GameObject>();

    void OnMouseOver() {
        
        hovering=true;
        //print("over "+name);
    }

    void OnMouseExit() {
        
        hovering=false;
        //print("exit "+name);
    }

    void Awake() {
        tmpName = GetComponentInChildren<TMP_Text>();

        for(int i=0; i<(int)Node.Feature.Count; i++) {
            if(!features.ContainsKey((Node.Feature)i))
            features[(Node.Feature)i] = false;
        }

        UpdateFeatureIcons();
    }

    void Start()
    {
        tmpName.text = Node.name[type];

        UpdateFeatureIcons();
    }

    void Delete() {
        if(type!=Node.Type.Spawn && type!=Node.Type.Exit) {
            foreach(GameObject connector in GameObject.FindGameObjectsWithTag("NodeConnector")) {
                NodeConnector nc = connector.GetComponent<NodeConnector>();
                if(nc.target != null)
                if(nc.target.parent == this) {
                    nc.parent.outputCount--;
                    nc.target = null;
                }

                if(nc.parent == this) {
                    if(nc.target!=null)
                    {
                        nc.target.parent.inputCount--;
                        nc.target.connectedTo=false;
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    void UpdateFeatureIcons() {
        foreach(GameObject icon in featureIcons) {
            Destroy(icon);
        }
        featureIcons.Clear();

        for(int i=0; i<(int)Node.Feature.Count; i++) {
            Node.Feature feat = (Node.Feature)i;
            if(features[feat]) {

                GameObject newIconObj = Instantiate(featureIconPrefab);
                newIconObj.transform.SetParent(featureIconParent.transform);
                newIconObj.transform.position = featureIconParent.transform.position;
                newIconObj.name = ((int)feat).ToString();
                featureIcons.Add(newIconObj);
            }
        }

        const float spacing = .5f;

        for(int i=0; i<featureIcons.Count; i++) {
            float x = (i*spacing) - (((featureIcons.Count-1)/2f)*spacing);
            //x = (i*spacing);
            //x = featureIcons[i].transform.position.x;
            //featureIcons[i].transform.position = new Vector3((i*spacing) - ((featureIcons.Count/2f)*spacing), featureIcons[i].transform.position.y, -1);
            featureIcons[i].transform.localPosition = new Vector3(x, 0, -1);
            Node.Feature feat = (Node.Feature)int.Parse(featureIcons[i].name);
            featureIcons[i].GetComponent<SpriteRenderer>().color = Node.featureIconColour[feat];
        }
    }

    void ToggleFeature(Node.Feature feature) {
        features[feature] = !features[feature];

        UpdateFeatureIcons();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z=0;

        if(hovering && Input.GetKeyDown(KeyCode.Mouse1)) {
            Dropdown dd = GameObject.FindGameObjectWithTag("Dropdown").GetComponent<Dropdown>();
            if(dd.IsOpen()) {
                if(type!=Node.Type.Spawn && type!=Node.Type.Exit) dd.AddTab("Delete").AddListener(()=>{Delete();});
                dd.AddTab(Node.inputAmount[type]+"->"+Node.outputAmount[type]);
                for(int i=0; i<(int)Node.Feature.Count; i++) {
                    Node.Feature feature = (Node.Feature)i;

                    dd.AddTab( (features[feature]?"Remove":"Add") +" "+feature.ToString()).AddListener(()=>ToggleFeature(feature));
                }
            }


            
        }

        if(hovering && Input.GetKeyDown(KeyCode.Mouse0)) {
            
            dragging=true;
            PlayerPrefs.SetInt("HoveringUI",true?1:0);
            returnPos = transform.position;
        }

        if(dragging && Input.GetKeyUp(KeyCode.Mouse0)) {
            dragging=false;
            PlayerPrefs.SetInt("HoveringUI",false?1:0);
        }

        if(dragging) {
            transform.position = mousePos;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public bool AddInputNode() {
        if(inputCount == Node.inputAmount[type]) return false;
        inputCount++;

        return true;
    }

    public bool AddOutputNode() {
        if(outputCount == Node.outputAmount[type]) return false;
        outputCount++;

        return true;
    }

    public void MakeConnectionTo(NodeElement target) {
        NodeConnector nearestTargetNode = null;
        float smallestDist = Mathf.Infinity;
        foreach(Transform connectorT in target.exits) {
            float dist = Vector3.Distance(transform.position, connectorT.position);
            if(dist < smallestDist) {
                smallestDist=dist;
                nearestTargetNode = connectorT.GetComponent<NodeConnector>();
            }
        }

        if(nearestTargetNode == null) {
            Debug.LogError("Beeg error");
            return;
        }

        NodeConnector nearestNodeToTarget = null;
        smallestDist = Mathf.Infinity;
        foreach(Transform connectorT in exits) {
            float dist = Vector3.Distance(target.transform.position, connectorT.position);
            if(dist < smallestDist) {
                smallestDist=dist;
                nearestNodeToTarget = connectorT.GetComponent<NodeConnector>();
            }
        }

        if(nearestNodeToTarget == null) {
            Debug.LogError("Beeg error 2");
            return;
        }

        nearestTargetNode.connectedTo=true;
        nearestTargetNode.parent.AddInputNode();

        nearestNodeToTarget.target = nearestTargetNode;
        AddOutputNode();
    }
}

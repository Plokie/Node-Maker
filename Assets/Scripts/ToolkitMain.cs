using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ToolkitMain : MonoBehaviour
{
    [SerializeField] GameObject iconPrefab;
    [SerializeField] GameObject nodeElementPrefab;

    void Start() {
        for(int i=3; i<(int)Node.Type.Count; i++) {
            if(Node.removedTypes.Contains((Node.Type)i)) continue;

            GameObject newObj = Instantiate(iconPrefab);
            newObj.transform.SetParent(transform);
            UINode un = newObj.GetComponent<UINode>();
            un.type = (Node.Type)i;
        }
    }

    public void MakeNode(Node.Type type, Vector3 pos) {
        pos.z=0;
        GameObject newObj = Instantiate(nodeElementPrefab);
        newObj.transform.position = pos;
        NodeElement ne = newObj.GetComponent<NodeElement>();
        ne.type=type;
        List<GameObject> objects = GameObject.FindGameObjectsWithTag("NodeElement").ToList();
        List<NodeElement> elements = new List<NodeElement>();
        foreach(GameObject obj in objects) {
            elements.Add(obj.GetComponent<NodeElement>());
        }
        objects.Clear();

        List<int> ids = elements.Select(e=>e.id).ToList();

        //Find a gap in the ids if there is one, otherwise itll return the length as a new ID 
        var gap = Enumerable.Range(ids.Min(), ids.Count).Except(ids).First();
        ne.id=gap;

        //FixIDGaps();
    }
}

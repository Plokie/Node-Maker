using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using SimpleFileBrowser;

[System.Serializable]
public struct GraphNode {
    public int id;
    public int type;
    public int[] branches;
    public int[] features;
    public float x,y;
}

public class FileManager : MonoBehaviour
{
    [SerializeField] GameObject nodeElementPrefab;
    public void Save() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Saves", ".txt"));

        FileBrowser.SetDefaultFilter(".txt");

        FileBrowser.ShowSaveDialog((path)=> {
            SavePath(path[0]);
        },
        ()=>{

        }, FileBrowser.PickMode.Files, false, Application.persistentDataPath, "name.txt", "Save", "Save");
    }

    public void Load() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Saves", ".txt"));

        FileBrowser.SetDefaultFilter(".txt");

        FileBrowser.ShowLoadDialog((path)=> {
            //SavePath(path[0]);
            //print(path[0]);
            ImportData(LoadPath(path[0]));
        },
        ()=>{

        }, FileBrowser.PickMode.Files, false, Application.persistentDataPath, "", "Load", "Load");

    }
    
    void SavePath(string path) {
        // path += "/export.dat";
        //print(path);
        //return;
        List<GraphNode> nodes = new List<GraphNode>();

        foreach(GameObject nodeObject in GameObject.FindGameObjectsWithTag("NodeElement")) {
            NodeElement ne = nodeObject.GetComponent<NodeElement>();
            
            GraphNode newNode = new GraphNode();
            newNode.id = ne.id;
            newNode.type = (int)ne.type;
            newNode.x = nodeObject.transform.position.x;
            newNode.y = nodeObject.transform.position.y;
            //newNode.branches = new List<int>();
            List<int> newBranches = new List<int>();

            foreach(Transform connectorT in ne.exits) {
                NodeConnector connector = connectorT.gameObject.GetComponent<NodeConnector>();
                if(connector.target!=null) {
                    newBranches.Add(connector.target.parent.id);
                }
            }

            newNode.branches = newBranches.ToArray();

            List<int> newFeatures = new List<int>();

            foreach(KeyValuePair<Node.Feature, bool> kvp in ne.features) {
                if(kvp.Value) {
                    newFeatures.Add((int)kvp.Key);
                }
            }

            newNode.features = newFeatures.ToArray();

            nodes.Add(newNode);
        }
        nodes = nodes.OrderBy(e=>e.id).ToList();

        GraphNode[] nodeArray = nodes.ToArray();

        // print("Before serialization:");
        // foreach(GraphNode node in nodeArray) {
        //     string branchString = "";
        //     foreach(int branch in node.branches) {
        //         branchString += "- "+branch + "\n";
        //     }
        //     print("ID: "+node.id + "\nType: " +  Node.name[(Node.Type)node.type] + "\n"+node.branches.Length+"  Connections:\n" + branchString );
        // }


        //string dest = Application.persistentDataPath + "/export.dat";
        FileStream file;

        if(File.Exists(path)) {
            file = File.OpenWrite(path);
        }
        else
        {
            file = File.Create(path);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, nodeArray);
        file.Close();

        //Load();
    }

    GraphNode[] LoadPath(string path) {
        //string dest = Application.persistentDataPath + "/export.dat";
        FileStream file;

        if(File.Exists(path)) {
            file = File.OpenRead(path);
        }
        else
        {
            Debug.LogError("File not found at "+path);
            return new GraphNode[]{};
        }

        BinaryFormatter formatter = new BinaryFormatter();
        GraphNode[] nodeArray = (GraphNode[])formatter.Deserialize(file);
        file.Close();
        return nodeArray;

    }

    void ImportData(GraphNode[] data) {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("NodeElement").ToList()) {
            DestroyImmediate(obj);
        }

        List<NodeElement> newElements = new List<NodeElement>();

        foreach(GraphNode node in data) {
            GameObject newObj = Instantiate(nodeElementPrefab, new Vector3(node.x, node.y, 0), Quaternion.identity);
            NodeElement ne = newObj.GetComponent<NodeElement>();
            ne.type = (Node.Type)node.type;
            ne.id = node.id;

            foreach(int featureID in node.features) {
                ne.features[(Node.Feature)featureID] = true;
            }

            newElements.Add(ne);
        }

        // foreach(GraphNode node in data) {
        //     string branchString = "";
        //     foreach(int branch in node.branches) {
        //         branchString += "- "+branch + "\n";
        //     }
        //     print("ID: "+node.id + "\nType: " +  Node.name[(Node.Type)node.type] + "\n"+node.branches.Length+"  Connections:\n" + branchString );
        // }

        int index=0;
        foreach(NodeElement element in newElements) {
            foreach(int id in data[index].branches) {
                //print(id);
                element.MakeConnectionTo(FindNodeByID(newElements, id));
            }
            index++;
        }
    }

    NodeElement FindNodeByID(List<NodeElement> elements, int id) {
        foreach(NodeElement element in elements) {
            if(element.id == id) return element;
        }
        return null;
    }
}

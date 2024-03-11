using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class FallingPlatGestion : MonoBehaviour
{
    private List<List<GameObject>> ListPaths = new List<List<GameObject>>();
    public List<GameObject> ListPath1;
    public List<GameObject> ListPath2;
    public List<GameObject> ListPath3;
    public List<GameObject> ListPath4;
    public List<GameObject> ListPath5;
    public List<GameObject> ListPath6;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0,6);
        ListPaths.Add(ListPath1);
        ListPaths.Add(ListPath2);
        ListPaths.Add(ListPath3);
        ListPaths.Add(ListPath4);
        ListPaths.Add(ListPath5);
        ListPaths.Add(ListPath6);
        List<GameObject> path = ListPaths[rand];
        foreach (var obj in path)
        {
            fallingPlatSnow script = obj.GetComponent<fallingPlatSnow>();
            script.onPath = true;
        }
    }
}

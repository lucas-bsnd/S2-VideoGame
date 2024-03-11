using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlat : MonoBehaviour
{
    public GameObject[] PlatList;
    // Start is called before the first frame update
    void Start()
    {
        FallPlatDesert fp = PlatList[Random.Range(0, 3)].GetComponent<FallPlatDesert>();
        fp.isactive = false;
    }

}

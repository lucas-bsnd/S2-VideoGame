using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaqueTournante : MonoBehaviour
{
    public float speed = 2.0f;
    private Material mat;
    private float rotation;
    private Vector2 offset = Vector2.zero;
    private Vector3 tiling = new Vector3(1, 1, 1);

    private void Start()
    {
        //mat = transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0,1,0)*Time.deltaTime*speed, Space.World);
    }
    
    //pour que quand on est sur le mur, il nous déplace
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "IA")
            other.gameObject.transform.parent.gameObject.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "IA")
            other.gameObject.transform.parent.gameObject.transform.parent = null;
    }
}

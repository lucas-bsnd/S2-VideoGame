using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatSnow : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer mesh;
    public Material red;
    public Material green;
    public Material snow;
    public bool onPath = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        mesh.sharedMaterial = snow;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" /*|| collision.gameObject.tag == "IA"|| collision.gameObject.tag == "IAPrefab"*/)
        {
            if (onPath)
            {
                mesh.sharedMaterial = green;
            }
            else
            {
                mesh.sharedMaterial = red;
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}

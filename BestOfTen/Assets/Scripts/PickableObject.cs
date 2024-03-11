using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public float throwforce = 500;
    private bool Prend = false;
    public static bool Pris = false;
    private GameObject GoName;

    void Update()
    {
        if (Prend && Input.GetKeyDown(KeyCode.E) && GoName != null)
        {
            Pris = true;
            GoName.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            GoName.transform.position = GameObject.Find("Eject").transform.position;
            GoName.gameObject.transform.parent = GameObject.Find("Eject").transform;
            GoName.gameObject.GetComponent<PhotonView>().enabled = false;
        }
        if(Input.GetMouseButtonDown(0) && GoName != null)
        {
            Pris = false;
            Lancer();
        }
        if (Input.GetButtonDown("Fire2") && GoName != null)
        {
            Pris = false;
            GoName.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            GoName.gameObject.transform.parent = null;
            GoName.gameObject.GetComponent<PhotonView>().enabled = true;
        }
    }

    void Lancer()
    {
        GoName.gameObject.transform.parent = null;
        GoName.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        GoName.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward * throwforce));
        GoName.gameObject.GetComponent<PhotonView>().enabled = true;
    }
    private void OnTriggerEnter(Collider Col)
    {
        if(Col.gameObject.tag == "Projectil")
        {
            GoName = Col.gameObject;
            Prend = true;
        }
    }
    private void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.tag == "Projectil")
        {
            Prend = false;
        }
    }
}

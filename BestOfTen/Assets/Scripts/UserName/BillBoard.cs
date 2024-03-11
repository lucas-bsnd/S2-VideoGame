using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Camera cam;

    // Permet de tourner le nom des autres joueurs vers la cam du joueur

    void FixedUpdate()
    {
        if (cam == null) // Trouve une camera
        {
            cam = transform.parent.parent.GetComponentInChildren<Camera>();
            //cam = FindObjectOfType<Camera>();
        }
        if (cam == null)
            return;

        transform.LookAt(cam.transform); // Tourne l'objet
        transform.Rotate(Vector3.up * 180);
    } 
}

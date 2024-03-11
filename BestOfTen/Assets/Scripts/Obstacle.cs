using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /*Component[] cs = (Component[])other.GetComponents(typeof(Component));
        foreach (Component c in cs)
        {
            Debug.Log("name " + c.name + " type " + c.GetType() + " basetype " + c.GetType().BaseType);
        }*/

        if (other.CompareTag("IA"))
        {
            IAControls contr = (IAControls) other.gameObject.GetComponent(typeof(IAControls));
            contr.obstacle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IA"))
        {
            IAControls contr = (IAControls) other.gameObject.GetComponent(typeof(IAControls));
            contr.obstacle = false;
        }
    }
}

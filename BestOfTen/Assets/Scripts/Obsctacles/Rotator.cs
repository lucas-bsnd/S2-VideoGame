using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed = 3f;
    public float sens = 1f;
    public bool vertical = false;


    // Update is called once per frame
    void Update()
    {
        if (vertical)
        {
            transform.Rotate(0f, sens * speed * Time.deltaTime / 0.01f, 0f, Space.Self);
        }
        else
        {
            transform.Rotate(0f, 0f, sens * speed * Time.deltaTime / 0.01f, Space.Self);
        }
	}
}

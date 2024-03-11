using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;
using Random = System.Random;

public class Bounce : MonoBehaviour
{
	public float force = 10f; //Force 10000f
	public float stunTime = 0.5f;
	private Vector3 hitDir;
	private Ragdoll ragdoll;

	void OnCollisionEnter(Collision collision)
    {
	    if (collision.gameObject.tag == "Player")
	    {
		    Random yes = new Random();
		    int val = yes.Next() % 2;
		    if (val == 0)
		    {
			    collision.gameObject.GetComponent<Ragdoll>().setanim = true;
		    }
		    else if (!collision.gameObject.GetComponent<CharacterControls>().etourdi)
		    {
			    foreach (ContactPoint contact in collision.contacts)
			    {
				    Debug.DrawRay(contact.point, contact.normal, Color.white);
				    if (collision.gameObject.tag == "Player")
				    {
					    hitDir = contact.normal;
					    collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir * force, stunTime);
					    return;
				    }
			    }
		    }
	    }

	    /*if (collision.relativeVelocity.magnitude > 2)
	    {
		    if (collision.gameObject.tag == "Player")
		    {
			    //Debug.Log("Hit");
			    collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir*force, stunTime);
		    }
		    //audioSource.Play();
	    }*/
	}
}

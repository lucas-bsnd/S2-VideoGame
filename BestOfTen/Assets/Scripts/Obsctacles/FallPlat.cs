using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FallPlat : MonoBehaviour
{
	

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player")
			{
				StartCoroutine(Fall());
			}
		}
	}

	IEnumerator Fall()
	{
		yield return new WaitForSeconds(gameObject.transform.parent.gameObject.GetComponent<FallPlat2>().fallTime);
		gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
	
}

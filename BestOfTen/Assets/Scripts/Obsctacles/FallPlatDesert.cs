using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatDesert : MonoBehaviour
{
	public float fallTime = 0.5f;
	public bool isactive = true;

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (isactive && collision.gameObject.tag == "Player")
			{
				StartCoroutine(Fall());
			}
		}
	}

	IEnumerator Fall()
	{
		yield return new WaitForSeconds(fallTime);
		gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
}

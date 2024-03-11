using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class SavePos : MonoBehaviour
{
	public Transform checkPoint;
	public List<GameObject> nextPoint;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<CharacterControls>().checkPoint = checkPoint.position;
		}

		if (col.gameObject.tag == "IA")
		{
			col.gameObject.GetComponent<IAControls>().checkPoint = checkPoint.position;
			col.gameObject.GetComponent<IAControls>().targets = nextPoint;
		}
	}
}

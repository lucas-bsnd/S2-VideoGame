using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
		}
        
        if (col.gameObject.tag == "IAPrefab" || col.gameObject.tag == "IA")
        {
	        col.gameObject.GetComponent<IAControls>().LoadCheckPoint();
        }
    }
    
    void OnCollisionEnter(Collision col)
    {
	    //pour la rivière
	    if (col.gameObject.tag == "Player")
	    {
		    Debug.Log("Kill!");

		    col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
	    }

	    if (col.gameObject.tag == "IAPrefab" || col.gameObject.tag == "IA")
	    {
		    Debug.Log("Kill!");
		    col.gameObject.GetComponent<IAControls>().LoadCheckPoint();
	    }
    }
}

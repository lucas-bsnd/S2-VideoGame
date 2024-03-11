using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public int time;
    public float multiplier;
    private float prevspeed;
    private float prevrotatespeed;
    private bool isActive = false;
    private IEnumerator OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!isActive)
        {
            if (other.gameObject.tag == "IA" || other.gameObject.tag == "IAPrefab")
            {
                Debug.Log("Getting speed boost!");
                IAControls ia = other.GetComponent<IAControls>();
                
                prevspeed = ia.movementSpeed;
                ia.movementSpeed *= multiplier;
                isActive = true;
                yield return new WaitForSeconds(time);
                ia.movementSpeed = prevspeed;
                isActive = false;
            }

            CharacterControls player = other.GetComponentInChildren<CharacterControls>();
            prevspeed = player.speed;
            prevrotatespeed = player.rotateSpeed;
            player.speed *= multiplier;
            player.rotateSpeed *= multiplier;
            isActive = true;
            yield return new WaitForSeconds(time);
            player.speed = prevspeed;
            player.rotateSpeed = prevrotatespeed;
            isActive = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapisRoulant2 : MonoBehaviour
{
    public float speed = 5.0f;
    
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player" || collisionInfo.gameObject.tag == "IA")
            collisionInfo.gameObject.transform.Translate(0,0,Time.deltaTime * speed, transform);
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "Player")
            other.gameObject.GetComponent<CharacterControls>().IsGrounded = true;
    }
}

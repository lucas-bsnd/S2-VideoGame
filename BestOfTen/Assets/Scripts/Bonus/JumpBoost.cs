using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    public int time;
    public float multiplier;
    private float prevjump;
    private bool isActive = false;
    private IEnumerator OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!isActive)
        {
            CharacterControls player = other.GetComponentInChildren<CharacterControls>();
            prevjump = player.jumpHeight;
            player.jumpHeight *= multiplier;
            isActive = true;
            yield return new WaitForSeconds(time);
            player.jumpHeight = prevjump;
            isActive = false;
        }
    }
}

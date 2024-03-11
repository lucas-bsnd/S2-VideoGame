using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public bool setanim = false;
    private Animator anim;
    public int nbpiece;
    public int time;
    public float multiplier;
    private float prevspeed;
    private float prevrotatespeed;
    private bool isActivespeed = false;
    private bool isActivejump = false;
    private float prevjump;
    private CharacterControls oui;
    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        nbpiece = 0;
        oui = GetComponentInParent<CharacterControls>();
    }
    void Update()
    {
        StartCoroutine(UpDating());
    }
    
    IEnumerator UpDating()
    {
        if (setanim && !oui.etourdi)
        {
            setanim = false;
            oui.etourdi = true;
            oui.EtourdiSon();
            if (anim != null)
            {
                anim.SetBool(Animator.StringToHash("impact"), true);
            }
            yield return new WaitForSeconds(0.5F);
            if (anim != null)
                anim.SetBool(Animator.StringToHash("impact"), false);
            yield return new WaitForSeconds(6);
            oui.etourdi = false;
        }
    }


    public IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BonusTime")
        {
            Destroy(other.gameObject);
            nbpiece++;
        }
        if (other.gameObject.tag == "BonusSpeed")
        {
            if (!isActivespeed)
            {
                other.gameObject.SetActive(false);
                CharacterControls player = gameObject.transform.parent.GetComponentInChildren<CharacterControls>();
                prevspeed = player.speed;
                prevrotatespeed = player.rotateSpeed;
                player.speed *= multiplier;
                player.rotateSpeed *= multiplier;
                isActivespeed = true;
                yield return new WaitForSeconds(time);
                player.speed = prevspeed;
                player.rotateSpeed = prevrotatespeed;
                isActivespeed = false;
            }
        }
        if (other.gameObject.tag == "BonusJump")
        {
            if (!isActivejump)
            {
                other.gameObject.SetActive(false);
                CharacterControls player = gameObject.transform.parent.GetComponentInChildren<CharacterControls>();
                prevjump = player.jumpHeight;
                player.jumpHeight *= multiplier;
                isActivejump = true;
                yield return new WaitForSeconds(time);
                player.jumpHeight = prevjump;
                isActivejump = false;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectil")
        {
            if(!collision.gameObject.GetComponent<IsGround>().ground)
                setanim = true;
        }
    }

}

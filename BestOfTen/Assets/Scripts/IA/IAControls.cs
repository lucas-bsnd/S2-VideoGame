using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class IAControls : MonoBehaviour
{ 
    public Rigidbody rb;
    
    public bool obstacle;
    public bool isGrounded;
    
    public float jumpForce = 7.0f;
    public float movementSpeed = 25f;
    
    public Vector3 target;
    public bool isSearching;

    public Vector3 checkPoint;
    public List<GameObject> targets;

    private bool isArrived;
    private Animator anim;

    public int nbPieces;
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        nbPieces = 0;
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        obstacle = false;
        isSearching = true;

        List<GameObject> next = GameObject.Find("StartIA").GetComponent<SavePos>().nextPoint;
        target = next[Random.Range(0, next.Count)].transform.position;
        target = new Vector3(target.x, transform.position.y, target.z);

        isArrived = false;
        checkPoint = transform.position;

        anim = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Obstacle")
        {
            isGrounded = true;
        }
	    
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ground"|| col.gameObject.tag == "Obstacle")
            isGrounded = false;
    }

    void FixedUpdate ()
    {
        if (!isArrived)
        {
            if (isGrounded)
            {
                anim.SetBool(Animator.StringToHash("Ground"), true);
                anim.SetBool(Animator.StringToHash("Jump"), false);
            }
            else
            {
                anim.SetBool(Animator.StringToHash("Ground"), false);
            }
            
            if (isGrounded && obstacle) // saute
            {
                anim.SetBool(Animator.StringToHash("Jump"), true);
                anim.SetBool(Animator.StringToHash("Ground"), false);
                rb.AddForce(-transform.up * 10f);
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(20 * jumpForce), rb.velocity.z);
            }

            if (isSearching) // marche vers le checkpoint target
            {
                // anims
                if (rb.velocity != Vector3.zero)
                    anim.SetFloat(Animator.StringToHash("Speed"), movementSpeed);

                float Speed = movementSpeed * Time.deltaTime;
                Vector3 actualTarget = new Vector3(target.x, transform.position.y, target.z);
                Vector3 mt = Vector3.MoveTowards(transform.position, actualTarget, Speed);
                transform.rotation = Quaternion.LookRotation(actualTarget - transform.position);

                transform.position = mt;

                isSearching = !(Vector3.Distance(transform.position, actualTarget) <= 0.5f);
            }

            if (targets.Count != 0 && !isSearching)
            {
                if (!(targets[0].name == "FinishIA"))
                {
                    target = targets[Random.Range(0, targets.Count)].transform.position;
                    isSearching = true;
                }
                else
                {
                    target = targets[0].transform.position;
                    isSearching = true;
                }
            }
            
            Debug.Log("Speed" + movementSpeed);
            Debug.Log("Jump" + jumpForce);

        }

    }
    

    public IEnumerator OnTriggerEnter(Collider collider)
    {
        if (collider.name == "FinishIA")
        {
            anim.SetBool(Animator.StringToHash("Ground"), true);
            anim.SetBool(Animator.StringToHash("Jump"), true);
            anim.SetFloat(Animator.StringToHash("Speed"), 0);
            isArrived = true;
        }

        if (collider.tag == "BonusTime")
        {
            collider.gameObject.SetActive(false);
            nbPieces++;
        }

        if (collider.tag == "BonusSpeed")
        {
            collider.gameObject.SetActive(false);
            Debug.Log("Getting speed boost!");
            
            float prevspeed = movementSpeed;
            movementSpeed *= 1.7f;
            yield return new WaitForSeconds(5);
            movementSpeed = prevspeed;
        }
        
        if (collider.gameObject.tag == "BonusJump")
        {
            collider.gameObject.SetActive(false);
            //CharacterControls player = gameObject.transform.parent.GetComponentInChildren<CharacterControls>();
            float prevjump = jumpForce;
            jumpForce *= 1.7f;
            yield return new WaitForSeconds(5);
            jumpForce = prevjump;
        }

    }

    public void LoadCheckPoint()
    {
        transform.position = checkPoint;
        isGrounded = true;
        
        if (targets.Count != 0)
        {
            if (!(targets[0].name == "FinishIA"))
            {
                Debug.Log("Changing targets!");
                target = targets[Random.Range(0, targets.Count)].transform.position;
                isSearching = true;
            }
            else
            {
                target = targets[0].transform.position;
                isSearching = true;
            }
        }
    }
}

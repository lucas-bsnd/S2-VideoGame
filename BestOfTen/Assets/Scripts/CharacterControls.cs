using System;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls : MonoBehaviour {

	public float size = 1;
	public float speed = 5;
	public float airVelocity = 8f; //pouvoir avancer en l'air
	public float maxVelocityChange = 10.0f; //en gros vitesse de déplacement max
	public float jumpHeight = 2.0f;
	public float rotateSpeed = 25f;
	//valeurs temporaire de vitesse pour la course
	private float tempspeed;
	private float temprotatespeed;
	private Vector3 moveDir;
	public GameObject cam;
	private Rigidbody rb;
	private Animator anim;

	public bool finishedTimer = false;
	private Canvas DebugCanvas;

	public bool canMove = true;
	public bool etourdi = false;
	private bool isStuned = false;
	private bool wasStuned = false;
	public bool isgrounded;
	
	public AudioClip jumpSound;
	public AudioClip pieceSound;
	public AudioClip etourdissementSound;
	private AudioSource speaker;
	public bool IsGrounded
	{
		get => isgrounded;
		set { isgrounded = value; }
	}

	private float pushForce;
	private Vector3 pushDir;

	public Vector3 checkPoint;

	void  Start ()
	{
		anim = GetComponent<Animator>();
		tempspeed = speed;
		temprotatespeed = rotateSpeed;
		DebugCanvas = GameObject.FindGameObjectWithTag("DebugCanvas").GetComponentInChildren<Canvas>(includeInactive:true);
		speaker = GetComponent<AudioSource>();
	}
	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		checkPoint = transform.position;
		Cursor.visible = false;

		// reglages size:
		rb.transform.localScale = new Vector3(size,size,size);
		speed *= size;
		airVelocity *= size;
		maxVelocityChange *= size;
		jumpHeight *= size;
		rotateSpeed *= size;
	}

	private void OnCollisionStay(Collision col)
    {
	    if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Obstacle")
	    {
		    isgrounded = true;
	    }
    }

    void OnCollisionExit(Collision col)
    {
	    //dès qu'on quitte le sol
		if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Obstacle")
			isgrounded = false;
    }
    IEnumerator OnTriggerEnter(Collider col)
    {
	    if ((col.gameObject.tag == "BonusTime" || col.gameObject.tag == "BonusSpeed" || col.gameObject.tag == "BonusJump"))
	    {
			if(speaker != null)
				speaker.PlayOneShot(pieceSound);
	    }
	    //permet de lancer l'animation et les particules de fin
	    //la danse se lance que si on est au sol sinon l'animation se joue en l'air...
	    //je n'arrive pas à attendre que l'on soit au sol, un while(!isgrounded)
	    if (col.gameObject.tag == "Finish")
		{
			finishedTimer = true;
			col.GetComponent<ParticleSystem>().Play();
			StartCoroutine(WaitGround());
			while(!isgrounded) yield return new WaitForSeconds(0.1f);
			if (isgrounded)
			{
				yield return new WaitForSeconds(0.5f);
				if(anim != null)
					anim.SetBool(Animator.StringToHash("EndDance"), true);
				canMove = false;
				yield return new WaitForSeconds(2);
				canMove = true;
				if(anim != null)
					anim.SetBool(Animator.StringToHash("EndDance"), false);
				isgrounded = true;
			}
			string playername = gameObject.GetComponentInChildren<UserNameDisplay>().text.text;
            if (!RoomManager.PlayerArrived.Contains(playername))
            {
	            RoomManager.PlayerArrived.Add(playername);
            }
		}
		if (col.gameObject.tag == "Projectil")
		{
			try
			{
				if (col.gameObject.GetComponent<PhotonView>().Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
				{
					//Debug.Log("transfering to " + name);
					PhotonView view = col.gameObject.GetComponent<PhotonView>();
					view.TransferOwnership(GetComponent<PhotonView>().Owner);
				}
			}
			catch (NullReferenceException)
			{
				yield break;
			}
		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		{
			try
			{
				if (collision.gameObject.GetComponent<PhotonView>().Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
				{
					//Debug.Log("transfering to " + name);
					PhotonView view = collision.gameObject.GetComponent<PhotonView>();
					view.TransferOwnership(GetComponent<PhotonView>().Owner);
				}
			}
			catch (NullReferenceException)
			{
				return;
			}
		}
	}

	IEnumerator WaitGround()
	{
		if(!isgrounded)
			yield return new WaitForSeconds(0.1f);
		yield break;
	}

	void FixedUpdate () {

		if (PauseMenu.isOn)
        {
            if (!Cursor.visible)
            {
				Cursor.visible = true;
            }
			return;
        }
		if (Cursor.visible)
		{
			Cursor.visible = false;
		}
		
		if (DebugCanvas.isActiveAndEnabled)
			canMove = false;
		else if (!isStuned)
			canMove = true;
		//on recupère l'état des touches de déplacements (entre -1 et 1)et calcule la
		//direction global (moveDir) où l'on doit aller en accord avec l'orientation de la caméra
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		Vector3 h2 = h * cam.transform.right;
		Vector3 v2 = v * cam.transform.forward;
		moveDir = (v2 + h2).normalized; //normalized donne une norme de 1 en conservant la direction

		if (canMove && !finishedTimer)
		{
			//gestion de la course quand shift gauche est appuyé
			tempspeed = speed;
			temprotatespeed = rotateSpeed;
			if (Input.GetKey("left shift"))
            {
				tempspeed *= 1.8f;
				temprotatespeed *= 1.8f;
			}
				
			if (moveDir.x != 0 || moveDir.z != 0)
			{
				//gestion de la rotation du personne, la classe Quaternion représente les rotations
				//l'axe y est l'altitude
				Vector3 targetDir = moveDir;
				targetDir.y = 0;
				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion goal = Quaternion.LookRotation(targetDir);//rotation qu'il doit effectuer
				//le quaternion.slerp sert à atteindre la rotation goal petit à petit
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, goal, Time.deltaTime * temprotatespeed);
				transform.rotation = targetRotation;
			}

			if (isgrounded)
			{
				// La vélocité représente la vitesse de changement de position
				//ex: on est au point (0,0,0) et notre velocity sur y (axe horizontal vers l'avant) est 10
				//alors on attendra le point (0,100,0) au bout de 10 secondes
				Vector3 targetVelocity = moveDir;
				targetVelocity *= tempspeed;

				//gestion des animations
				anim.SetBool(Animator.StringToHash("Ground"), true);
				Physics.gravity = new Vector3(0, -50, 0);
				if (targetVelocity != Vector3.zero)
					anim.SetFloat(Animator.StringToHash("Speed"), tempspeed);
				else
					anim.SetFloat(Animator.StringToHash("Speed"), 0);

				//un peu de magie
				Vector3 velocity = rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude)
				{
					targetVelocity = velocity;
					rb.velocity /= 1.1f;
				}

				Vector3 velocityChange = (targetVelocity - velocity);
				//le mathf.clamp(value, min, max) renvoie la value si elle est compris entre min et max
				//renvoie min si value<min et max si value>max
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
				if (Mathf.Abs(rb.velocity.magnitude) < speed)
				{
					rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}

				// Jump
				if (isgrounded && Input.GetButton("Jump"))
				{
					speaker.PlayOneShot(jumpSound);
					Physics.gravity = new Vector3(0, -30, 0); //determine the speed of jump
					anim.SetBool(Animator.StringToHash("Jump"), true);
					anim.SetBool(Animator.StringToHash("Ground"), false);
					rb.velocity = new Vector3(velocity.x, Mathf.Sqrt(20 * jumpHeight), velocity.z);
				}
			}

			else
			{
				anim.SetBool(Animator.StringToHash("Jump"), false);
				//un peu de magie quand on est en l'air
				/*Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
				Vector3 velocity = rb.velocity;
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				rb.AddForce(velocityChange, ForceMode.VelocityChange);*/
			}
		}
		else
		{
			//quand on ne peut pas bouger, uniquement quand on s'est pris un obstacle et qu'il y a l'effet "rebond"
			rb.velocity = pushDir * pushForce;
		}
	}

	public void HitPlayer(Vector3 velocityF, float time)
	{
		//gère l'effet "rebond" des obstacles, méthode appelé quand on rentre en concact avec l'un des obstacles ayant cet effet
		rb.velocity = velocityF;
		pushForce = velocityF.magnitude;
		pushDir = Vector3.Normalize(velocityF);
		//startcoroutine permet l'exécution d'une fonction en "arrière-plan" sans attendre qu'elle soit terminée
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	private IEnumerator Decrease(float value, float duration)
	{
		//value est la norme de la force du rebond, pouvait être fixe mais là ça permet de la modifier en fonction des obstacles
		//duration est la durée où l'on est repoussé
		if (isStuned)
			wasStuned = true;
		isStuned = true;
		canMove = false;

		float delta = value / duration;
		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
			pushForce -= Time.deltaTime * delta;
			pushForce = pushForce < 0 ? 0 : pushForce;
		}

		if (wasStuned)
			wasStuned = false;
		else
		{
			isStuned = false;
			canMove = true;
		}
	}
	
	public void LoadCheckPoint()
	{
		transform.position = checkPoint;
	}

	public void EtourdiSon()
	{
		if(speaker != null)
			speaker.PlayOneShot(etourdissementSound);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public float followSpeed = 15; //Speed at which the camera follows us
	public float mouseSpeed = 3; //Speed at which we rotate the camera with the mouse
	public float cameraDist = 7; //Distance to which the camera is located
	public Transform target; //Player the camera follows

	[HideInInspector]
	public Transform pivot; //Pivot on which the camera rotates(distance that we want between the camera and our character)
	[HideInInspector]
	public Transform camTrans; //Camera position

	float turnSmoothing = .1f; //Smooths all camera movements (Time it takes the camera to reach the rotation indicated with the joystick)
	public float minAngle = -5; //Minimum angle that we allow the camera to reach
	public float maxAngle = 80; //Maximum angle that we allow the camera to reach

	float smoothX;
	float smoothY;
	float smoothXvelocity;
	float smoothYvelocity;
	public float vueDuCôtéAngle = 0; //Angle the camera has on the Y axis
	public float vueDuDessusAngle = 20; //Angle the camera has up / down

	void Awake()
	{
		camTrans = Camera.main.transform;
		pivot = camTrans.parent;
	}

	void FollowTarget(float d)
	{ //Function that makes the camera follow the player
		float speed = d * followSpeed; //Set speed regardless of fps
		Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed); //Bring the camera closer to the player interpolating with the velocity(0.5 half, 1 everything)
		transform.position = targetPosition; //Update the camera position
	}

	void HandleRotations(float d, float v, float h, float targetSpeed)
	{ //Function that rotates the camera correctly
		if (turnSmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing); //Gradually change a value toward a desired goal over time.
			smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
		}
		else
		{
			smoothX = h;
			smoothY = v;
		}

		vueDuDessusAngle -= smoothY * targetSpeed; //Update the angle at which the camera will move
		vueDuDessusAngle = Mathf.Clamp(vueDuDessusAngle, minAngle, maxAngle); //Limits with respect to the maximum and minimum
		pivot.localRotation = Quaternion.Euler(vueDuDessusAngle, 0, 0); //Modify the up / down angle

		vueDuCôtéAngle += smoothX * targetSpeed; //Updates the rotation angle in y smoothly
		transform.rotation = Quaternion.Euler(0, vueDuCôtéAngle, 0); //Apply the angle

	}

	private void FixedUpdate()
	{//Function that correctly rotates the camera based on the joystick / mouse and follows the player (the delta time is sent to be independent of the fps)
        if (PauseMenu.isOn)
	        return;
        
        float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");
		float targetSpeed = mouseSpeed;
		camTrans.localPosition = new Vector3(0, 0, -cameraDist);
		FollowTarget(Time.deltaTime); //Follow player
		HandleRotations(Time.deltaTime, v, h, targetSpeed); //Rotates camera
	}
}

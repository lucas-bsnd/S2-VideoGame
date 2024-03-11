using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stalactite : MonoBehaviour
{
    public float stopTime;
    public float transitionSpeed;
    public float distance;
	public float offset = 0;

	private float posYDown;
	private float posYUp;
	private bool isGoingUp = true;
	private bool isWaiting = false;
	// Start is called before the first frame update
	void Awake()
    {
		posYDown = transform.position.y;
		posYUp = posYDown + distance;
        transform.position = transform.position + new Vector3(0, offset, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (isWaiting)
			return;
		if ((isGoingUp && transform.position.y>posYUp )|| (!isGoingUp && transform.position.y<posYDown))
        {
			StartCoroutine(WaitToChange());
		}
		if (isGoingUp)
        {
			transform.position += Vector3.up * Time.deltaTime * transitionSpeed;
		}
		else
		{
			transform.position -= Vector3.up * Time.deltaTime * transitionSpeed;
		}
	}

	IEnumerator WaitToChange()
	{
		isWaiting = true;
		yield return new WaitForSeconds(stopTime);
		isWaiting = false;
		isGoingUp = !isGoingUp;
	}
}

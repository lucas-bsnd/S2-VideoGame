using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIinGame : MonoBehaviour
{
    public Text timerText;
    public Text Placement;
    public float distance;
    public GameObject checkpoint;
    private string name;

    private void Start()
    {
        //arrived = false;
        checkpoint = GameObject.FindGameObjectWithTag("Finish");
        name = "Player Name";
    }
    private void Update()
    {
        if(name == "Player Name")
            name = gameObject.transform.parent.GetComponentInChildren<UserNameDisplay>().text.text;
        else
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            if (scene == 2 || scene == 3 ||
                scene == 4)
            {
                distance = Vector3.Distance(checkpoint.transform.position,
                    gameObject.transform.parent.GetChild(1).transform.position);
                Placement.text = RoomManager.GetPlace(name);
                if (transform.parent.GetComponentInChildren<CharacterControls>().finishedTimer)
                {
                    timerText.color = Color.red;
                }
                timerText.text = RoomManager.GetTime(name);
            }
        }
        
    }
}
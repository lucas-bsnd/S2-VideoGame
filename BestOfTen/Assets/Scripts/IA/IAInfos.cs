using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IAInfos : MonoBehaviour
{
    public float distance;
    private GameObject checkpoint;
    public bool finished;
    public string time;
    public string username;
    
    private void Start()
    {
        checkpoint = GameObject.FindGameObjectWithTag("Finish");
        finished = false;
    }
    private void FixedUpdate()
    {
        distance = Vector3.Distance(checkpoint.transform.position, gameObject.transform.position);
        time = RoomManager.GetTime(username);
    }

    void OnTriggerEnter(Collider col)
    {
        //permet de lancer l'animation et les particules de fin
        //la danse se lance que si on est au sol sinon l'animation se joue en l'air...
        //je n'arrive pas à attendre que l'on soit au sol, un while(!isgrounded)
        if (col.gameObject.tag == "Finish")
        {
            finished = true;
            double t = PhotonNetwork.Time - double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            string minutes = ((int)t / 60).ToString();
            string secondes = (t % 60).ToString("f2");
            time = minutes + ":" + secondes;
            /*string playername = gameObject.GetComponentInChildren<UserNameDisplay>().text.text;
            if (!RoomManager.PlayerArrived.Contains(playername))
            {
                RoomManager.PlayerArrived.Add(playername);
            }*/
        }
    }
}

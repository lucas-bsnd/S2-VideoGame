using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Console
{
    [CreateAssetMenu(fileName = "TP command", menuName = "DebugCommand/TP")]
    public class TPCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            switch (logText)
            {
                case "end":
                    player.transform.position =
                        GameObject.FindGameObjectWithTag("Finish").transform.position /* - new Vector3(0,0,15)*/;
                    player.GetComponent<CharacterControls>().checkPoint = player.transform.position;
                    return true;
                case "start":
                    player.transform.position =
                        GameObject.FindGameObjectWithTag("Start").transform.position;
                    player.GetComponent<CharacterControls>().checkPoint = player.transform.position;
                    return true;
                case "river":
                    player.transform.position =
                        GameObject.FindGameObjectWithTag("River").transform.position - new Vector3(70,0,5);
                    player.GetComponent<CharacterControls>().checkPoint = player.transform.position;
                    return true;
                case "pont":
                    player.transform.position = new Vector3(4.49f,14.9f,-27.9f);
                    player.GetComponent<CharacterControls>().checkPoint = player.transform.position;
                    return true;
                case "foret":
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel(2);
                    }

                    return true;
                case "desert":
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel(3);
                    }
                    return true;
                case "banquise":
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel(4);
                    }
                    return true;
            }
            return false;
        }
    }
}

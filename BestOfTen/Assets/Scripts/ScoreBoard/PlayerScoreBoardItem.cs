using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour
{
    [SerializeField]
    Text userNameText;

    [SerializeField]
    Text placeText;

    [SerializeField]
    Text timerText;

    public void Setup(GameObject player)
    {
        userNameText.text = player.GetComponentInChildren<UserNameDisplay>().text.text;
        if (player.GetComponent<CharacterControls>().finishedTimer)
        {
            timerText.text = player.transform.parent.GetComponentInChildren<UIinGame>().timerText.text;
        }
        placeText.text = player.transform.parent.GetComponentInChildren<UIinGame>().Placement.text;
    }

    public void Setup(OurPlayer player)
    {
        Debug.Log(player.placement + "  " + player.username + "  " + player.timesum + " ourplayers final infos");
        userNameText.text = player.username;
        float total = player.timesum;
        string minutes = ((int)total / 60).ToString();
        string secondes = (total % 60).ToString("f2");
        string totalstr =  minutes + ":" + secondes;
        timerText.text = totalstr;
        placeText.text = player.placement;
    }

    public void Setup(string infos)
    {
        string name = infos.Split(' ')[3];
        userNameText.text = name;
        timerText.text = RoomManager.GetTime(name);
        placeText.text = RoomManager.GetPlace(name);
    }
}

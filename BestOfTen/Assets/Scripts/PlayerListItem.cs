using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks


{
    [SerializeField] TMP_Text text;

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer && gameObject.tag == "Player")
        {
            Destroy(gameObject);
            //Destroy(PlayerManager.pauseMenuInstance);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
        //Destroy(PlayerManager.pauseMenuInstance);
    }
}

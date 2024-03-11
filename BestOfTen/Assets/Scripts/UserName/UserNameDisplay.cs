using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class UserNameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    public TMP_Text text;

    void Start()
    {
        if (transform.parent.tag == "Player")
        {
            text.text = playerPV.Owner.NickName;
        }
        else
        {
            text.text = gameObject.transform.parent.gameObject.GetComponent<IAInfos>().username;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class UserNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            userNameInput.text = PlayerPrefs.GetString("username").Replace(" ","");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username").Replace(" ","");
        }
        else
        {
            userNameInput.text = "Player" + Random.Range(0, 10000);
            OnUsernameInputValueChanged();
        }
    }
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = userNameInput.text.Replace(" ","");
        PlayerPrefs.SetString("username", userNameInput.text.Replace(" ",""));
    }
}

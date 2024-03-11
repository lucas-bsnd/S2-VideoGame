using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScore : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject startGameButton;
    public void Start()
    {
        Cursor.visible = true;
        StartCoroutine(Oui());
    }

    public IEnumerator Oui()
    {
        yield return new WaitForSeconds(0.5f);
        if (PhotonNetwork.IsMasterClient)
        {
            string third = "";
            string second = "";
            string first = "";
            List<string> noms = new List<string>();
            foreach (var nom in RoomManager.scoreBoard)
            {
                noms.Add(nom.Split(' ')[3]);
            }
            for (int i = 0; i < noms.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        first = RoomManager.OurPlayers[noms[i]].skin;
                        break;
                    case 1:
                        second = RoomManager.OurPlayers[noms[i]].skin;
                        break;
                    case 2:
                        third = RoomManager.OurPlayers[noms[i]].skin;
                        break;
                }
            }
            if (first != "")
            {
                PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", first), new Vector3(988.6F, 526.9F, -796.3F), Quaternion.Euler(0, 205, 0));
                //premier.gameObject.transform.SetParent(GameObject.Find("First").transform);
                //premier.transform.localPosition = new Vector3(20.6F, 31.5F, 45.5F);
                //premier.transform.rotation = Quaternion.Euler(0, 205, 0);
            }
            if (second != "")
            {
                PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", second), new Vector3(966.3F,519.3F,-796.5F), Quaternion.Euler(0,185.4F,0));
                //deux.gameObject.transform.SetParent(GameObject.Find("Second").transform);
                //deux.transform.localPosition = new Vector3(3.9F, 22.8F, 44.6F);
                //deux.transform.rotation = Quaternion.Euler(0, 185.4F, 0);
            }
            if (third != "")
            {
                PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", third), new Vector3(1011.9F, 513.3F, -796.5F), Quaternion.Euler(0, -140, 0));
                //trois.gameObject.transform.SetParent(GameObject.Find("Third").transform);
                //trois.transform.localPosition = new Vector3(37.2F, 18.8F, 48.2F);
                //trois.transform.rotation = Quaternion.Euler(0, -140.8F, 0);
            }
        }
    }
    public void DisconnectPlayer()
    {
        if(PhotonNetwork.PlayerList.Length != 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        }
        Destroy(RoomManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
    private void FixedUpdate()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        if (RoomManager.gamedScene.Count == 3)
        {
            startGameButton.SetActive(false);
        }
    }
    public void OnClick()
    {
        RoomManager.OnClickChangerDeScene();
    }

}

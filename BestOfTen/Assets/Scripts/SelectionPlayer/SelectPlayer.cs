using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SelectPlayer : MonoBehaviour
{
    GameObject Spotlight;
    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Spotlight = GameObject.Find("Spotlight").gameObject;
        Anim = GetComponent<Animator>();
        Anim.SetBool("Choose", true);
    }

    private void OnMouseEnter()
    {
        Spotlight.GetComponent<AudioSource>().Play();
        Spotlight.transform.position = new Vector3(transform.position.x, Spotlight.transform.position.y, Spotlight.transform.position.z);
    }
    private void OnMouseDown()
    {
        PlayerPrefs.SetString("Player", gameObject.name);
        ReturnMenu();
    }
    public void ReturnMenu()
    {
        Destroy(RoomManager.Instance.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }
    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}

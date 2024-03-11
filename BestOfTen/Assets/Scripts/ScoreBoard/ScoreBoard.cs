using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    GameObject playerScoreBoardItem;

    [SerializeField]
    Transform playerScoreBoardList;

    private void OnEnable()
    {
        StartCoroutine(OUi());
    }

    private IEnumerator OUi()
    {
        if(SceneManager.GetActiveScene().buildIndex == 5)
            yield return new WaitForSeconds(0.1f);
        var currentRoomCustomProperty = PhotonNetwork.CurrentRoom.CustomProperties["ScoreBoard"];
        string[] scoreboardForAll = (string[]) currentRoomCustomProperty;
        foreach (var player in scoreboardForAll)
        {
            GameObject itemGO = Instantiate(playerScoreBoardItem, playerScoreBoardList);
            PlayerScoreBoardItem item = itemGO.GetComponent<PlayerScoreBoardItem>();
            if(item != null)
                item.Setup(player);
        }
    }

    private void OnDisable()
    {
        foreach  (Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }
}

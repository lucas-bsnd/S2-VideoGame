using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher InstanceLaunch;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    public static bool offline;
    public static bool onegame;
    private string map;



    void Awake()
    {
        InstanceLaunch = this;
    }

    //Join the Lobby on start -----------------------------------

    void Start()
    {
        //MenuManager.Instance.OpenMenu("loading");
        //MenuManager.Instance.OpenMenu("mode");
        MenuManager.Instance.OpenMenu("title");
        offline = true;
        Cursor.visible = true;
    }
    
    public void Connect()
    {
        PhotonNetwork.OfflineMode = false;
        offline = false;
        MenuManager.Instance.OpenMenu("loading");
        if (PhotonNetwork.IsConnected)
        {
            //Destroy(RoomManager.Instance.gameObject);
            StartCoroutine(DisconnectAndLoad());
            //SceneManager.LoadScene(0);
            Cursor.visible = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    IEnumerator DisconnectAndLoad()
    {
        /*if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LeaveRoom();
        else*/
            PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (offline)
        {
            PhotonNetwork.CreateRoom("Offline mode");
            LoadOfflineScene(map);
        }
        else
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    public override void OnJoinedLobby()
    {
        //MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    //-----------------------------------------------------------




    //Create a room ---------------------------------------------

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        if (!offline)
        {
            MenuManager.Instance.OpenMenu("room");
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;


            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform child in PlayerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Length; i++)
            {
                Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }

            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    //-----------------------------------------------------------

    public void StartGame()
    {
        int sceneNB = 3;
        //int sceneNB = Random.Range(2,5);
        RoomManager.gamedScene.Add(sceneNB);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(sceneNB);
    }
    public void StartForet()
    {
        RoomManager.gamedScene.Add(2);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(2);
    }
    public void StartDesert()
    {
        RoomManager.gamedScene.Add(3);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(3);
    }

    public void StartBanquise()
    {
        RoomManager.gamedScene.Add(4);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(4);
    }

    //Leave the room --------------------------------------------

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }


    // Find a room ----------------------------------------------

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
           
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }



    //-----------------------------------------------------------

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void OnClickSelectCharacter()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator Room2(string room)
    {
        MenuManager.Instance.OpenMenu("loading");
        Connect();
        yield return new WaitUntil( () => PhotonNetwork.InLobby);
        MenuManager.Instance.OpenMenu(room);
    }
    public void CreateRoomOnline()
    {
        StartCoroutine(Room2("create room"));
    }

    
    public void FindRoomOnline()
    {
        StartCoroutine(Room2("find room"));
    }

    private IEnumerator OfflineSub()
    {
        //need a subfunction to be sure that we are not connected after playing an online game for example
        if (PhotonNetwork.IsConnected)
            yield return StartCoroutine(DisconnectAndLoad());
        offline = true;
        PhotonNetwork.OfflineMode = true;
    }
    public void PlayOffline(string mapname)
    {
        map = mapname;
        StartCoroutine(OfflineSub());
    }

    private void LoadOfflineScene(string mapname)
    {
        switch (mapname)
        {
            case "foret":
                onegame = true;
                StartForet();
                break;
            case "desert":
                onegame = true;
                StartDesert();
                break;
            case "banquise":
                onegame = true;
                StartBanquise();
                break;
            case "game":
                onegame = false;
                StartGame();
                break;
        }
    }
}

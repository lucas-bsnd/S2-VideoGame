using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using Photon.Realtime;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public static List<int> gamedScene;
    public static GameObject[] playersList;
    public static GameObject[] IAList;
    public static List<string> PlayerArrived;
    public static List<GameObject> AllplayersSorted;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public double startTime;
    public static bool finalsceneloaded = false;
    public static Dictionary<string, OurPlayer> OurPlayers;
    public static bool infossaved = false;
    public static string[] scoreBoard; //(place, name, time)
    public static bool scoreboardprint = false;
    public static List<string> IAnames;
    void Awake()
    {
        if(Instance) //Verifie si un roomManager existe et le supprime en conséquence
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);//créé un roomManager
        Instance = this;
        PlayerArrived = new List<string>();
        AllplayersSorted = new List<GameObject>();
        gamedScene = new List<int>();
        OurPlayers = new Dictionary<string, OurPlayer>();
        finalsceneloaded = false;
        infossaved = false;
        scoreBoard = new string[0];
        CustomeValue = new ExitGames.Client.Photon.Hashtable();
        IAnames = new List<string>();
    }
    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            if (scene == 2 || scene == 3 || scene == 4)
            {
                playersList = GameObject.FindGameObjectsWithTag("Player");
                IAList = GameObject.FindGameObjectsWithTag("IA");
                if (playersList.Length /*+ IAList.Length*/ == PlayerArrived.Count)
                    ChangeScene();
                else
                    PlacePlayer();

                CustomeValue.Remove("ScoreBoard");
                CustomeValue.Add("ScoreBoard", scoreBoard);
                PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
            }

            if (scene == 5 && !scoreboardprint)
            {
                scoreboardprint = true;
                List<OurPlayer> sortedPlayers = new List<OurPlayer>();
                foreach (var player in scoreBoard)
                {
                    sortedPlayers.Add(OurPlayers[player.Split(' ')[3]]);
                }

                Sort(sortedPlayers);
                scoreBoard = new string[sortedPlayers.Count];
                for (int i = 0; i < sortedPlayers.Count; i++)
                {
                    sortedPlayers[i].placement = (i + 1) + " / 10";
                    string minutes = ((int)sortedPlayers[i].timesum / 60).ToString();
                    string secondes = (sortedPlayers[i].timesum % 60).ToString("f2");
                    string timestr = minutes + ":" + secondes;
                    scoreBoard[i] = sortedPlayers[i].placement + " " + sortedPlayers[i].username + " " + timestr;
                }

                CustomeValue.Remove("ScoreBoard");
                CustomeValue.Add("ScoreBoard", scoreBoard);
                PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        scoreboardprint = false;
        if (scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"),Vector3.zero,Quaternion.identity);
            //il y a un RoomManager par joueur mais le InstantiateRoomObject ne fonctionne que pour le créateur de la salle
            //donc on a bien 10 joueurs
            //et comme les IA sont instantiés en tant qu'objet de la scène et pas objet du joueur, elle reste vivante
            //même quand l'un de joueurs quitte la partie
            if (!Launcher.offline && (scene.buildIndex == 2 || scene.buildIndex == 3 || scene.buildIndex == 4) && PhotonNetwork.IsMasterClient) //Si on est dans la scene foret
            {
                int i = PhotonNetwork.PlayerList.Length;
                if (IAnames.Count == 0)
                {
                    for (int j = 0; j < 10-i; j++)
                    {
                        IAnames.Add("Player" + Random.Range(1000, 10000));
                    }
                }
                int k = 0;
                GameObject start = GameObject.FindGameObjectWithTag("Start");
                while (i < 10)
                {
                    float x = Random.Range(start.transform.position.x - 5, start.transform.position.x + 5);
                    GameObject oui = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "IAPrefab"),
                        new Vector3(x, start.transform.position.y, start.transform.position.z),
                        Quaternion.identity);
                    oui.GetComponentInChildren<IAInfos>().username = IAnames[k];
                    i++;
                    k++;
                }
            }
        }
    }

    public void ChangeScene()
    {
        if (!infossaved)
        {
            //call only one time at the beginning of the game
            FirstlySaveInfos();
            infossaved = true;
        }
        //tous les joueurs ont finis
        if (Launcher.onegame)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
        else if(!finalsceneloaded)
        {
            UpInfos();
            finalsceneloaded = true;
            PhotonNetwork.LoadLevel(5);
        }
    }

    private static bool firstgame = true;
    public static void OnClickChangerDeScene()
    {
        int sceneNB;
        if (firstgame)
        {
            sceneNB = 4;
            firstgame = false;
        }
        else
        {
            sceneNB = 2;
        }
        gamedScene.Add(sceneNB);
        PlayerArrived.Clear();
        finalsceneloaded = false;
        PhotonNetwork.LoadLevel(sceneNB);
    }
    private void TriDesDistances(Tuple<float, GameObject>[] distances)
    {
        int n = distances.Length;
        for (int i = 1; i < n; ++i) {
            Tuple<float, GameObject> key = distances[i];
            int j = i - 1;
            while (j >= 0 && distances[j].Item1 > key.Item1)
            {
                distances[j + 1] = distances[j];
                j = j - 1;
            }
            distances[j + 1] = key;
        }
    }
    
    private void PlacePlayer()
    {
        int lgt = playersList.Length + IAList.Length;
        Tuple<float, GameObject>[] distances = new Tuple<float, GameObject>[lgt];

        // copie la liste des players en ajoutant leur distances
        for (int i = 0; i < playersList.Length; i++)
        {
            distances[i] = new Tuple<float, GameObject>(playersList[i].transform.parent.GetComponentInChildren<UIinGame>().distance,playersList[i]); 
        }

        for (int i = 0; i < IAList.Length; i++)
        {
            distances[i+playersList.Length] = new Tuple<float, GameObject>(IAList[i].GetComponent<IAInfos>().distance,IAList[i]); 
        }
        
        TriDesDistances(distances);
        
        AllplayersSorted.Clear();
        scoreBoard = new string[distances.Length];
        
        //same timer for all
        double t;
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime"))
            t = PhotonNetwork.Time - double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        else
            t = 0;
        string minutes = ((int)t / 60).ToString();
        string secondes = (t % 60).ToString("f2").Replace('.',',');
        string timestr = minutes + ":" + secondes;
        
        for (int i = 0; i < distances.Length; i++)
        {
            string temptime = timestr;
            UIinGame realplayer = distances[i].Item2.transform.parent.GetComponentInChildren<UIinGame>();
            if (realplayer != null)
            {
                if (realplayer.timerText.color == Color.red)
                {
                    temptime = realplayer.timerText.text;
                }
                scoreBoard[i] =(i+1) + " / 10 " + distances[i].Item2.transform.parent.GetComponentInChildren<UserNameDisplay>().text.text + " " + temptime;
            }
            else
            {
                IAInfos ia = distances[i].Item2.GetComponent<IAInfos>();
                if (ia.finished)
                {
                    temptime = ia.time;
                }
                scoreBoard[i] =(i+1) + " / 10 " + ia.username + " " + temptime;
            }
            AllplayersSorted.Add(distances[i].Item2);
        }
    }

    private void FirstlySaveInfos()
    {
       for (int i = 0; i < AllplayersSorted.Count; i++)
       {
           string name = AllplayersSorted[i].GetComponentInChildren<UserNameDisplay>().text.text;
           OurPlayers[name] = new OurPlayer(AllplayersSorted[i].gameObject.name, name, GetPlace(name));
       }
    }

    private void UpInfos()
    {
        for (int i = 0; i < AllplayersSorted.Count; i++)
        {
            string name1 = AllplayersSorted[i].GetComponentInChildren<UserNameDisplay>().text.text;
            Ragdoll time = AllplayersSorted[i].GetComponent<Ragdoll>();
            IAControls iaTime = AllplayersSorted[i].GetComponent<IAControls>();
            if (time != null)
                OurPlayers[name1].AddTime(GetTime(name1), time.nbpiece);
            else
            {
                if (iaTime != null)
                    OurPlayers[name1].AddTime(GetTime(name1), iaTime.nbPieces);
                else
                    OurPlayers[name1].AddTime(GetTime(name1), 0);
            }

            
            
        }
    }

    public static string GetPlace(string name)
    {
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("ScoreBoard"))
        {
            var currentRoomCustomProperty = PhotonNetwork.CurrentRoom.CustomProperties["ScoreBoard"];
            string[] scoreboard = (string[]) currentRoomCustomProperty;
            List<string> myinfos = scoreboard.ToList().FindAll(obj => obj.Split(' ')[3] == name);
            if (myinfos.Count == 0)
                return "";
            string[] split = myinfos[0].Split(' ');
            return split[0] + " " + split[1] + " " + split[2];
        }
        return "";
    }

    public static string GetTime(string name)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("ScoreBoard"))
        {
            var currentRoomCustomProperty = PhotonNetwork.CurrentRoom.CustomProperties["ScoreBoard"];
            string[] scoreboard = (string[]) currentRoomCustomProperty;
            List<string> myinfos = scoreboard.ToList().FindAll(obj => obj.Split(' ')[3] == name);
            if (myinfos.Count == 0)
                return "0:0,00";
            string[] split = myinfos[0].Split(' ');
            return split[4];
        }
        return "0:0,00";
    }
    
    private void Sort(List<OurPlayer> arr)
    {
        int n = arr.Count;
        for (int i = 1; i < n; ++i) {
            OurPlayer key = arr[i];
            int j = i - 1;
 
            // Move elements of arr[0..i-1],
            // that are greater than key,
            // to one position ahead of
            // their current position
            while (j >= 0 && arr[j].timesum > key.timesum) {
                arr[j + 1] = arr[j];
                j = j - 1;
            }
            arr[j + 1] = key;
        }
    }
}
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField]
    //private GameObject pauseMenuPrefab;
    //public static GameObject pauseMenuInstance;


    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        GameObject start = GameObject.FindGameObjectWithTag("Start");
        float x;
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex == 2) //foret
        {
            x = Random.Range(start.transform.position.x-10, start.transform.position.x+10);
        }
        else
        {
            x = start.transform.position.x;
        }

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerPrefs.GetString("Player", "Player1")),
                new Vector3(x,start.transform.position.y,start.transform.position.z),Quaternion.identity);


        //pauseMenuInstance = Instantiate(pauseMenuPrefab);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject foret;
    public GameObject desert;
    public GameObject banquise;
    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int sceneNB = SceneManager.GetActiveScene().buildIndex;
            
            switch (sceneNB)
            {
                case 2:
                    PhotonNetwork.InstantiateRoomObject(Path.Combine("Obstacles", foret.name), foret.transform.position,
                        foret.transform.rotation);
                    if (Launcher.offline)
                    {
                        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Phantom"), new Vector3(1.2f, 15, -110.5f),
                            Quaternion.identity);
                    }
                    break;
                case 3:
                    PhotonNetwork.InstantiateRoomObject(Path.Combine("Obstacles", desert.name), desert.transform.position,
                        desert.transform.rotation);
                    if (Launcher.offline)
                    {
                        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Phantom"), new Vector3(66.3f, 4, 22.7f),
                            Quaternion.identity);
                    }
                    break;
                case 4:
                    PhotonNetwork.InstantiateRoomObject(Path.Combine("Obstacles", banquise.name), banquise.transform.position,
                        banquise.transform.rotation);
                    if (Launcher.offline)
                    {
                        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Phantom"), new Vector3(-678.6f, 75.5f, 68.2f),
                            Quaternion.identity);
                    }
                    break;
                default:
                    return;
            }
        }
    }
}

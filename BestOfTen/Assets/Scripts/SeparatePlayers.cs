
using UnityEngine;
using Photon.Pun;

public class SeparatePlayers : MonoBehaviour
{
    PhotonView PV;

    [SerializeField] Behaviour[] componentsToDisable;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (!PV.IsMine)
        {
            //desactives les components des autres joueurs

            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }
    
}

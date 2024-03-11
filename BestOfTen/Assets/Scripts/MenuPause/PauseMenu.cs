using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    public static bool disconnecting = false;
    public static RoomManager Instance;
    public static bool isOn = false;
    public static PauseMenu PM;
    public GameObject[] Settings;
    public GameObject[] Buttons;
    public Dropdown DResolution;
    public Slider slider;
    public GameObject timer;
    public GameObject placement;
    private AudioSource audiosource;
    private Canvas DebugCanvas;
    
    private void Start()
    {
        audiosource = GameObject.Find("Sons").GetComponent<AudioSource>();
        isOn = false;
        disconnecting = false;
        SliderChanges();
        DebugCanvas = GameObject.FindGameObjectWithTag("DebugCanvas").GetComponentInChildren<Canvas>(includeInactive:true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = false;
            gameObject.transform.parent.GetComponentInChildren<CharacterControls>().canMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            isOn = true;
            gameObject.transform.parent.GetComponentInChildren<CharacterControls>().canMove = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !DebugCanvas.isActiveAndEnabled)
        {
            TogglePauseMenu();
        }

    }
    public void OnClickSettings()
    {
        foreach (var Setting in Settings)
        {
            Setting.SetActive(true);
        }
        foreach (var Button in Buttons)
        {
            Button.SetActive(false);
        }
    }
    public void ReturnMenu()
    {
        foreach (var Setting in Settings)
        {
            Setting.SetActive(false);
        }
        foreach (var Button in Buttons)
        {
            Button.SetActive(true);
        }
    }
    public void SetResolution()
    {
        switch (DResolution.value)
        {
            case 0:
                Screen.SetResolution(640, 360, true);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, true);
                break;
        }
    }
    public void SliderChanges()
    {
        audiosource.volume = slider.value;
    }
    public void TogglePauseMenu()
    {
        if (disconnecting) return;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        placement.SetActive(!placement.activeSelf);
        timer.SetActive(!timer.activeSelf);
        isOn = pauseMenu.activeSelf;
        if (isOn)
            gameObject.transform.parent.GetComponentInChildren<CharacterControls>().canMove = true;
        else
            gameObject.transform.parent.GetComponentInChildren<CharacterControls>().canMove = false;
    }
    public void DisconnectPlayer()
    {
        disconnecting = true;
        /*Destroy(RoomManager.Instance.gameObject);
        DisconnectAndLoad();
        //SceneManager.LoadScene(0);*/
        if(PhotonNetwork.PlayerList.Length != 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        }
        Destroy(RoomManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}

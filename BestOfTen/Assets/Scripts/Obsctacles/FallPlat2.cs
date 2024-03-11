using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlat2 : MonoBehaviour
{
    private GameObject[] me;
    public float respawnTime = 5;
    public float fallTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        me = new []
        {
            gameObject.transform.GetChild(0).gameObject,
            gameObject.transform.GetChild(1).gameObject
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!me[0].activeSelf)
            StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        foreach (var oui in me)
        {
            oui.SetActive(true);
        }
    }
}

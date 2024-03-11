using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Canon : MonoBehaviour
{
    public GameObject ObjetALancer;
    public Transform PointDeLancer;
    public float force = 600f;
    public float TimeBeforeDestroy = 10f;
    public bool Israndomthrow = true;
    public int timeBetween = 4;

    private void Start()
    {
        StartCoroutine(Lancer());
    }

    IEnumerator Lancer()
    {
        GameObject ball = Instantiate(ObjetALancer, PointDeLancer.position, PointDeLancer.rotation);
        ball.GetComponent<Rigidbody>().AddForce(PointDeLancer.forward * force * Time.deltaTime * 200000);
        
        if (Israndomthrow)
            yield return new WaitForSeconds(Random.Range(2, 8));
        else
            yield return new WaitForSeconds(timeBetween);
        Destroy(ball, TimeBeforeDestroy);
        StartCoroutine(Lancer());
    }
}

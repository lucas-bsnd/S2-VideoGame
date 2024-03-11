using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeObstacle : MonoBehaviour
{
    public List<GameObject> possiblePoints;
    public Obstacle4IA obstacle;
    public bool waiting;
    public bool jump;
    public BeforeObstacle nextObstacle;

    private void Awake()
    {
        jump = obstacle.jump;
    }
    
    private void Update()
    {
        waiting = obstacle.wait;
    }
}

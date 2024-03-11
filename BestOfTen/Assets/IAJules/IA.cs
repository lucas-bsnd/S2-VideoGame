using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    private BeforeObstacle obstacle;
    public IASystem iASystem;
    private bool arrived = false;
    private GameObject target;
    private bool jump = false;

    public void Awake()
    {
        obstacle = iASystem.FirstObstacle;
        ChoosePoint();
    }
    private void Update()
    {
        if (arrived)
        {
            if (obstacle.waiting)
            {
                return;
            }
            
            jump = obstacle.jump;
            obstacle = obstacle.nextObstacle;
            ChoosePoint();
            arrived = false;
            if (jump)
            {
                Jump();
            }
            return;
        }

        GoTo(); //target---
    }
    private void ChoosePoint()
    {
        int rand = Random.Range(0, obstacle.possiblePoints.Count);
        target = obstacle.possiblePoints[rand];
    }
    private void Jump()
    {

    }
    private void GoTo()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurPlayer : MonoBehaviour
{
    public List<string> alltimes;
    public string skin;
    public string username;
    public string placement;
    public float timesum;

    public OurPlayer(string skin, string username, string place)
    {
        alltimes = new List<string>();
        this.skin = skin;
        this.username = username;
        this.placement = place;
    }

    public void AddTime(string time, int nbpiece)
    {
        alltimes.Add(time);
        timesum += Parse(time) - nbpiece*2;
    }
    public static float Parse(string time)
    {
        string[] first = time.Split(':');
        string[] second = first[1].Split(',');
        float seconds = Int32.Parse(second[1]);
        return Int32.Parse(first[0]) * 60 + Int32.Parse(second[0]) + seconds/100;
    }
}

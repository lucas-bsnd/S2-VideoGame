using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreData : MonoBehaviour
{
    public CharacterControls script;
    private List<List<string>> posData;

    // Start is called before the first frame update
    void Start()
    {
        if (!Launcher.offline)
        {
            this.enabled = false;
        }
        posData = new List<List<string>>();
    }

    private void FixedUpdate()
    {
        if (!script.finishedTimer)
        {
            List<string> currentData = new List<string>();

            currentData.Add((Input.GetKey("left shift") ? script.speed * 1.8f : script.speed).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            currentData.Add(gameObject.transform.position.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            currentData.Add(gameObject.transform.position.y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            currentData.Add(gameObject.transform.position.z.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            
            posData.Add(currentData);
        }
        else
        {
            SaveCSV(posData.Select(elt => elt.ToArray()).ToArray());
        }
            
    }

    void SaveCSV(string[][] output)
    {
        string filePath = @"./Assets/" + SceneManager.GetActiveScene().name + "_" + OurPlayer.Parse(script.gameObject.transform.parent.gameObject.GetComponentInChildren<UIinGame>().timerText.text) + ".csv";
        string delimiter = ",";

        /*string[][] output = new string[][]
        {
            new string[] { "4:1", "2:1", "3:1" },
            new string[] { "1:2", "2:2", "3:2" },
        };*/

        int length = output.GetLength(0);
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length -1; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }
        
        sb.Append(string.Join(delimiter, output[length - 1]));
        
        File.WriteAllText(filePath, sb.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    [CreateAssetMenu(fileName = "Echo command", menuName = "DebugCommand/Echo")]
    public class EchoCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);
            Debug.Log(logText);
            return true;
        }
    }
}


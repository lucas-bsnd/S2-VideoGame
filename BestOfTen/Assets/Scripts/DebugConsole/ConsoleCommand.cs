using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public abstract class ConsoleCommand : ScriptableObject, DebugConsole
    {
        [SerializeField] private string commandWord = String.Empty;
        public string CommandWord => commandWord;
        public abstract bool Process(string[] args);
    }
}

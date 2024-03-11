using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public interface DebugConsole
    {
        string CommandWord { get; }
        bool Process(string[] args);
    }
}

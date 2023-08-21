using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    [System.Serializable]
    public class GameData
    {
        public bool[] isActive = new bool[100];

        public GameData()
        {
            isActive[0] = true;
        }
    }
}

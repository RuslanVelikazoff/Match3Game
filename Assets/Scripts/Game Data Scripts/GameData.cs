using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    [System.Serializable]
    public class GameData
    {
        public bool[] isActive = new bool[5];

        public int selectedLevel;

        public GameData()
        {
            selectedLevel = 0;
            isActive[0] = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData 
{
    [System.Serializable]
    public struct LevelData 
    {
        public string _name;
        public int score;
        public bool completed;
        public bool unlocked;

        public LevelData(string name)
        {
            _name = name;
            score = 0;
            completed = false;
            unlocked = false;
        }
    }
    public LevelData[] levels;

    //the values defined in this constructor will be default values
    //the game starts with when theres no save data to load
    public GameData()
    {
        levels = new LevelData[]
        {
            new LevelData("This is would be the main menu but is needed to have scene index line up nicely"),
            new LevelData("Tutorial"),
            new LevelData("Level1"),
            new LevelData("Level2"),
            new LevelData("Level3"),
            new LevelData("Level4"),
            new LevelData("Level5"),
            new LevelData("Level6"),
            new LevelData("Level7"),
            new LevelData("Level8"),
        };
        levels[0].completed = true;
        levels[0].unlocked = true;
        levels[1].unlocked = true;
    }

    public void CompleteLevel(int buildIndex, int score)
    {
        levels[buildIndex].score = score;
        levels[buildIndex].completed = true;
        levels[buildIndex].unlocked = true;
        if (buildIndex + 1 < levels.Length)
        {
            levels[buildIndex + 1].unlocked = true;
        }
    }

    public int GetLastCompletedBuildIndex()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            if (!levels[i].completed) return i;
        }
        return levels.Length-1;
    }
   
}

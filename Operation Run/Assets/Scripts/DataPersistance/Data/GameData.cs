using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData 
{
    public int PlayerScore;

    //the values defined in this constructor will be default values
    //the game starts with when theres no save data to load
    public GameData()
    {
        this.PlayerScore=0;
    }
   
}

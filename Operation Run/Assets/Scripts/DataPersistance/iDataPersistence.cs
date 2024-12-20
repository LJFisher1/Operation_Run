using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iDataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
   
    void SaveSettings(ref GameData data);
}

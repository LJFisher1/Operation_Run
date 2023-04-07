using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuButtonFunctions : MonoBehaviour
{
   public void NewGame()
   {
        SceneManager.LoadScene("templvl1");
   }
}

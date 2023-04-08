using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct TutPop
    {
        public String name;
        public string text;
        public bool completed;
    }
    public float TutorialDisplayTime;
    public TutPop[] tutoriualPopups;

    public int GetTutPopIndex(string name)
    {
        for(int i=0; i < tutoriualPopups.Length; i++)
        {
            if (name == tutoriualPopups[i].name)
            {
                return i;
            }
        }
        return 0;
    }
    public bool CheckCompleted(string name)
    {
        //Debug.Log(tutoriualPopups[GetTutPopIndex(name)].completed);
        //Debug.Log(tutoriualPopups[GetTutPopIndex(name)].text);
        return tutoriualPopups[GetTutPopIndex(name)].completed;
    }
    public void SetTutorialCompletion(string name, bool completed)
    {
        tutoriualPopups[GetTutPopIndex(name)].completed = completed;
    }
    public string GetTutText(string name)
    {
        return tutoriualPopups[GetTutPopIndex(name)].text;
    }
}
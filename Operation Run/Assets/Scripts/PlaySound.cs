using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
   public AudioSource soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer.Play();
        //audio= GetComponent<AudioSource>();
    }

   public void PlaySFX()
   {
        soundPlayer.Play();
   }
}

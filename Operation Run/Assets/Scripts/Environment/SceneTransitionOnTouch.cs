using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionOnTouch : MonoBehaviour
{
    /// <summary>
    /// You must add the scene to the build settings for this to work
    /// </summary>
    [SerializeField] string scene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // can make this prompt the UI first or something later/incorporate it with the win box.
            SceneManager.LoadScene(sceneName:scene);
        }
    }
}

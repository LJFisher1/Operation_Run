using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionOnTouch : MonoBehaviour
{
    [SerializeField] string scene;

    AssetBundle myLoadedAssetBundle;
    private void Start()
    {
       // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            SceneManager.LoadScene("Assets/Scenes/templvl1");
        }
    }
}

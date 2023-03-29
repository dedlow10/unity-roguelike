using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] SceneAsset DestinationScene;

    bool isSceneLoading = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && !isSceneLoading)
        {
            isSceneLoading = true;
            SceneManager.LoadScene(DestinationScene.name);
        }
    }
}

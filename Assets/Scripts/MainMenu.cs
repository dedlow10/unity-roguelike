using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SceneAsset startScene;
    public void NewGame_OnClick()
    {
        SceneManager.LoadScene(startScene.name);
    }

    public void Exit_OnClick()
    {
        Application.Quit();
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    Player Player;
    public WeaponPanel WeaponPanel;
    public GameObject PowerupPanel;
    public GameObject GameOverPanel;

    [SerializeField] HealthBar healthBar;
    public static GameManager instance { get; private set; }

    private bool IsMenuOpen;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySFX("BackgroundNoise-Hum", GetPlayer().transform.position, loop: true);
        AudioManager.instance.PlayMusic("AmbientMusic", loop: true);
        DialogueManager.instance.ShowText
            ("I see you're finally awake. \nYou have no idea what kind of trouble you're in. \nI would start with a weapon, would hate to be unarmed around here...");
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        var spawnPoint = GameObject.Find("SpawnPoint");
        var vcam = GameObject.Find("VirtualCamera");
        var player = GetPlayer();
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
        
        if (vcam != null)
        {
            var camera = vcam.GetComponent<CinemachineVirtualCamera>();
            camera.Follow = player.transform;
        }

    }

    public Player GetPlayer()
    {
        return Player.instance;
    }

    public bool IsGamePaused()
    {
        return Time.timeScale == 0 || DialogueManager.instance.isTextShowing || IsMenuOpen;
    }

    public void SetIsMenuOpen(bool isOpen)
    {
        AudioManager.instance.PlaySFX("MenuOpen", GetPlayer().transform.position);
        Time.timeScale = isOpen ? 0 : 1;
        IsMenuOpen = isOpen;
    }

    public bool GetIsMenuOpen()
    {
        return IsMenuOpen;
    }

    public void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar();
    }

    public void ShowGameOverScreen()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnGameOverContinue()
    {
        SceneManager.LoadScene(0);
    }
}

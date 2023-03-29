using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject heart;
    private void Start()
    {
        RenderHealthBar();
    }
    public void UpdateHealthBar()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        } 
        RenderHealthBar();
    }

    void RenderHealthBar()
    {
        var hitpoints = GameManager.instance.GetPlayer().GetCurrentHitpoints();
        for (int x = 0; x < hitpoints; x++)
        {
            Instantiate(heart, transform);
        }
    }
}

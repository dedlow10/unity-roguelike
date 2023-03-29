using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] int CoinCount;
    [SerializeField] TextMeshProUGUI textDisplay;
    public static CoinManager instance { get; private set; }


    public bool isTextShowing = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        textDisplay.text = CoinCount.ToString();
    }


    public void AddCoins(int quantity)
    {
        CoinCount += quantity;

        textDisplay.text = CoinCount.ToString();
    }

    public void SpendCoins(int quantity)
    {
        CoinCount -= quantity;

        textDisplay.text = CoinCount.ToString();
    }

    public int GetCurrentCount()
    {
        return CoinCount;
    }
}

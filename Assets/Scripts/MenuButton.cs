using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    TextMeshProUGUI text;
    Color originalColor;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = text.color;
    }
    public void OnPointerEnter()
    {
        text.color = hoverColor;
    }

    public void OnPointerExit()
    {
        text.color = originalColor;
    }
}

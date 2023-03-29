using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField] Image WeaponImage;
    [SerializeField] TextMeshProUGUI AmmoRemainingText;

    public void SetActiveWeapon(Sprite img)
    {
        if (!WeaponImage.enabled)
        {
            WeaponImage.enabled = true;
        }
        WeaponImage.sprite = img;
    }

    public void UpdateAmmoRemaining(string ammoText)
    {
        AmmoRemainingText.text = ammoText;
    }
}

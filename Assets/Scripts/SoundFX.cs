using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public void PlayGrass1()
    {
        //AudioManager.instance.PlaySFX("StepGrass1", gameObject.transform.position, .2f);
    }
    public void PlayGrass2()
    {
        //AudioManager.instance.PlaySFX("StepGrass2", gameObject.transform.position, .25f);
    }
    public void PlayGrass3()
    {
        //AudioManager.instance.PlaySFX("StepGrass3", gameObject.transform.position, .2f);
    }

    public void PlaySwordHit()
    {
        AudioManager.instance.PlaySFX("HitGore1", gameObject.transform.position);
    }

    public void PlayClubHit()
    {
        AudioManager.instance.PlaySFX("HitGore3", gameObject.transform.position);
    }
    public void PlayCharge()
    {
        AudioManager.instance.PlaySFX("Charge", gameObject.transform.position);
    }
    public void PlayThunder()
    {
        AudioManager.instance.PlaySFX("Thunder", gameObject.transform.position);
    }
}

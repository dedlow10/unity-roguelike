using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Fighter : MonoBehaviour
{
    [SerializeField] int HitpointsMax;

    protected int Hitpoints { get; set; }

    protected virtual void Awake()
    {
        Hitpoints = HitpointsMax;
    }

    public virtual void ReceiveDamage(int damage)
    {
        Hitpoints -= damage;

        if (this.Hitpoints <= 0)
        {
            On_Death();
        }
    }

    public virtual void RestoreHealth(int health)
    {
        Hitpoints = Math.Min(HitpointsMax, Hitpoints + health);
    }

    protected void On_Death()
    {
        DropItem dropItemComponent;
        if (TryGetComponent(out dropItemComponent))
        {
            dropItemComponent.Drop();
        }
        
        Destroy(gameObject);
    }

    public int GetCurrentHitpoints()
    {
        return Hitpoints;
    }
}

using UnityEngine;

public abstract class BaseItem: MonoBehaviour
{
    public abstract string GetName();
    public abstract void ReceiveItem();
}
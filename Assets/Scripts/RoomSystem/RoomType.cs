using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class RoomType: ScriptableObject
{
    public float chanceWeighted;
    public abstract void GenerateRoom(int roomLevel, Transform transform);
}

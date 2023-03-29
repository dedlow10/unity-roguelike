using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class KeyItemSO : ItemSO
    {
        [field: SerializeField]
        public AudioClip actionSFX {get; private set;}
    }
}
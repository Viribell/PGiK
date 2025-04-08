using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Item/Consumable" )]
public class ConsumableSO : ItemSO {
    [Header( "Consumable Info" )]
    [Range( 1.0f, 50.0f )]
    [field: SerializeField] public float useAmount;
}

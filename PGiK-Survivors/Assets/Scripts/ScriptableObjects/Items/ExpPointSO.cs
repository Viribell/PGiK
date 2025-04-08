using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Item/Exp Point" )]
public class ExpPointSO : ItemSO {
    [Header("Exp Info")]
    [Range(1.0f, 50.0f)]
    [field: SerializeField] public float amount;
}

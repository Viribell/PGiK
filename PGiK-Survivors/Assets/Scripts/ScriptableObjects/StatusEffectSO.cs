using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Status Effect" )]
public class StatusEffectSO : ScriptableObject {
    [Header( "General Info" )]
    [field: SerializeField] public string effectName;
    [field: SerializeField] public float tickValue;
    //particle system

    [Header("Duration Info")]
    [Range( 0.0f, 60.0f )]
    [field: SerializeField] public float tickSpeed; // tick per X seconds
    [Range(0.0f, 60.0f)]
    [field: SerializeField] public float durationSeconds;

    [Header("Additional Effects")]
    [field: SerializeField] public float movementPenalty;
}

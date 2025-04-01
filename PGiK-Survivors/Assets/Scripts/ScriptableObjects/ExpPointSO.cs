using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Exp Point" )]
public class ExpPointSO : ScriptableObject {
    [field: SerializeField] public float amount;
    [field: SerializeField] public Sprite sprite;
    [field: SerializeField] public GameObject prefab;
}

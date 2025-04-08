using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Objects/Destructible" )]
public class DestructibleSO : ScriptableObject {
    [field: SerializeField] public string objectName;
    [field: SerializeField] public Sprite sprite;
    [field: SerializeField] public GameObject prefab;
}

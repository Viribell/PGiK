using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource")]
public class Resource : ScriptableObject {

    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Transform Prefab { get; private set; }
}

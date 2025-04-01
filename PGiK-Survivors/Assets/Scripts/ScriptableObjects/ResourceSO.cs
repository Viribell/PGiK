using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO" )]
public class ResourceSO : ScriptableObject {

    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Transform Prefab { get; private set; }
}

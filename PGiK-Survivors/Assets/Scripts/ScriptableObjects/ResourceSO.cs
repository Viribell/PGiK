using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Item/Resource" )]
public class ResourceSO : ItemSO {
    [Header( "Interactable Material Info" )]
    [field: SerializeField] public bool isMaterial = false;
    [field: SerializeField] public Sprite defaultVisual;
    [field: SerializeField] public Sprite depletedVisual;
}

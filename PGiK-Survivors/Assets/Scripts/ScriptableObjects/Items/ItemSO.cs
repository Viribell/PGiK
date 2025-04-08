using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Magnet,
    Consumable,
    Material,
    Gold,
    BossDrop,
    SkillBook,
    PowerUp,
    ExpPoint
}

public abstract class ItemSO : ScriptableObject {
    [Header( "General Info" )]
    [field: SerializeField] public string itemName;
    [field: SerializeField] public ItemType itemType;
    [field: SerializeField] public Sprite sprite;
    [field: SerializeField] public GameObject prefab;

    [Header( "Drop Info" )]
    [field: SerializeField] public bool usesDropChance;
    [Range(0.0f, 1.0f)]
    [field: SerializeField] public float dropChance;
}

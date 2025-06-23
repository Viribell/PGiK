using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType {
    Sunflower,
    Cake,
    Syrup,
    Stew,
    Hotpot
}

[CreateAssetMenu( menuName = "Scriptable Objects/Food" )]
public class FoodSO : ScriptableObject {
    [Header( "Info" )]
    [field: SerializeField] public string foodName;
    [field: SerializeField] public FoodType foodType;
    [field: SerializeField] public int maxLevels;
    [TextArea( 2, 10 )][field: SerializeField] public string description;
    [field: SerializeField] public Sprite image;

    [Header( "Price" )]
    [field: SerializeField] public SerializableDictionary<ResourceSO, int> price = new SerializableDictionary<ResourceSO, int>();
}

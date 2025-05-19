using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {
    Undefined,
    Universal,
    Class,
    Skill,
    Special
}

[CreateAssetMenu( menuName = "Scriptable Objects/Cards/Card" )]
public class CardSO : ScriptableObject {
    [Header("Basic Info")]
    [field: SerializeField] public string cardName;
    [TextArea(3, 5)][field: SerializeField] public string cardContent;
    [field: SerializeField] public CardType cardType;
    [field: SerializeField] public int cardLevel;
    [field: SerializeField] public int cardMaxLevel;
    [field: SerializeField] public int unlockLevel;

    [Header( "Card Info" )]
    [field: SerializeField] public CardSO[] nextCards;
    [field: SerializeField] public CardSO[] cardsToLock;
    //...related ability when abilities are made


    [field: Header("Mods")]
    [field: SerializeField] public List<StatModifier> Mods { get; private set; }

    public List<StatModifier> GetMods() {
        if ( Mods.Count == 0 ) return null;

        List<StatModifier> modsList = new List<StatModifier>();

        foreach ( StatModifier mod in Mods ) {
            StatModifier newMod = new StatModifier( mod.value, mod.type, mod.affectedStat );

            modsList.Add( newMod );
        }

        return modsList;
    }
}

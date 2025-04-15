using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "Scriptable Objects/Entity/Class" )]
public class ClassSO : EntitySO {
    [field: Header( "Player Entity LvlUp Mods" )]
    [field: SerializeField] public List<StatModifier> levelUpMods { get; private set; }

    public List<StatModifier> GetLevelUpMods() {
        if ( levelUpMods.Count == 0 ) return null;

        List<StatModifier> modsList = new List<StatModifier>();

        foreach(StatModifier mod in levelUpMods) {
            StatModifier newMod = new StatModifier( mod.value, mod.type, mod.affectedStat );

            modsList.Add( newMod );
        }

        return modsList;
    }
}

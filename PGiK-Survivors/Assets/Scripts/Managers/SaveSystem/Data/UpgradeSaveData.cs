using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeSaveData {
    public string id;
    public int currentLevel;
    public int maxLevels;
    public StatModifier upgradeMod;

    public int basicPrice;
    public float priceIncreasePercentage;

    public void UpdateMod() {
        if ( upgradeMod == null ) return;

        upgradeMod.UpdateDefaultValue();
        upgradeMod.value = upgradeMod.basicValue * currentLevel;
    }
}

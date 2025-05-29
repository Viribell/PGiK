using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesControl : MonoBehaviour, IPersistentData {
    public static UpgradesControl Instance { get; private set; }

    [field: SerializeField] public List<UpgradeSO> defaultCurrencyUpgrades;

    [field: SerializeField] public SerializableDictionary<string, UpgradeSaveData> upgradesData;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of UpgradesControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );
    }

    public bool HasUpgrade( UpgradeType upgrade ) {
        if ( upgradesData.TryGetValue( upgrade.ToString(), out UpgradeSaveData data ) ) {
            return data.currentLevel > 0;
        }

        return false;
    }

    public UpgradeSaveData GetCurrencyUpgrade( string id ) {
        if ( upgradesData.TryGetValue( id, out UpgradeSaveData upgrade ) ) return upgrade;
        else return null;
    }

    public List<StatModifier> GetStatMods() {
        List<StatModifier> mods = new List<StatModifier>();
        
        foreach(KeyValuePair<string, UpgradeSaveData> entry in upgradesData) {
            UpgradeSaveData value = entry.Value;

            if ( value.currentLevel == 0 || value.upgradeMod == null || value.upgradeMod.affectedStat == StatType.Undefined ) continue;

            value.UpdateMod();

            mods.Add( value.upgradeMod );
        }

        return mods;
    }

    public void AddLevel( string id ) {
        upgradesData[id].currentLevel++;
    }

    public void SubtractLevel( string id ) {
        upgradesData[id].currentLevel--;
    }

    private void InitUpgrades( SaveData saveData ) {
        foreach( UpgradeSO upgrade in defaultCurrencyUpgrades ) {
            UpgradeSaveData newData = new UpgradeSaveData();

            newData.id = upgrade.upgrade.ToString();
            newData.currentLevel = 0;
            newData.maxLevels = upgrade.maxLevels;
            newData.upgradeMod = upgrade.upgradeMod;
            newData.upgradeMod.UpdateBasicValue();
            newData.basicPrice = upgrade.basicPrice;
            newData.priceIncreasePercentage = upgrade.priceIncreasePercentage;

            newData.UpdateMod();

            upgradesData.Add( upgrade.upgrade.ToString(), newData );
            saveData.upgrades.Add( newData );
        }
    }

    public void LoadData( SaveData data ) {
        if( data.upgrades == null  || data.upgrades.Count <= 0 ) { InitUpgrades( data ); return; }

        upgradesData.Clear();

        List<UpgradeSaveData> upgrades = data.upgrades;

        foreach(UpgradeSaveData upgrade in upgrades) {
            upgradesData.Add( upgrade.id, upgrade );
        }
    }

    public void SaveData( SaveData data ) {
        
    }
}

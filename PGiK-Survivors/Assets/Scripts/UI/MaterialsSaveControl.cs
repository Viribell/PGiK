using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsSaveControl : MonoBehaviour, IPersistentData {
    public static MaterialsSaveControl Instance { get; private set; }

    [field: SerializeField] public List<FoodSO> defaultFoods;

    [field: SerializeField] public SerializableDictionary<string, MaterialSaveData> upgradesData;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of MaterialsSaveControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );
    }

    public bool HasUpgrade( UpgradeType upgrade ) {
        if ( upgradesData.TryGetValue( upgrade.ToString(), out MaterialSaveData data ) ) {
            return data.currentLevel > 0;
        }

        return false;
    }

    public FoodSO GetFood( FoodType type ) {
        foreach ( FoodSO food in defaultFoods ) {
            if ( food.foodType == type ) return food;
        }

        return null;
    }

    public FoodSO GetFood( string name ) {
        foreach ( FoodSO food in defaultFoods ) {
            if ( food.foodType.ToString() == name ) return food;
        }

        return null;
    }

    public MaterialSaveData GetCurrencyUpgrade( string id ) {
        if ( upgradesData.TryGetValue( id, out MaterialSaveData upgrade ) ) return upgrade;
        else return null;
    }

    public void AddLevel( string id ) {
        upgradesData[id].currentLevel++;
    }

    public void SubtractLevel( string id ) {
        upgradesData[id].currentLevel--;
    }

    private void InitUpgrades( SaveData saveData ) {
        foreach ( FoodSO upgrade in defaultFoods ) {
            MaterialSaveData newData = new MaterialSaveData();

            newData.name = upgrade.foodType.ToString();
            newData.currentLevel = 0;
            newData.maxLevels = upgrade.maxLevels;

            upgradesData.Add( upgrade.foodType.ToString(), newData );
            saveData.cooking.Add( newData );
        }
    }

    public void LoadData( SaveData data ) {
        if ( data.cooking == null || data.cooking.Count <= 0 ) { InitUpgrades( data ); return; }

        upgradesData.Clear();

        List<MaterialSaveData> upgrades = data.cooking;

        foreach ( MaterialSaveData upgrade in upgrades ) {
            upgradesData.Add( upgrade.name, upgrade );
        }
    }

    public void SaveData( SaveData data ) {

    }
}

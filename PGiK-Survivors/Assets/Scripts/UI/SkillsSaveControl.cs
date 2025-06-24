using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsSaveControl : MonoBehaviour, IPersistentData {
    public static SkillsSaveControl Instance { get; private set; }

    [field: SerializeField] public List<SkillSO> defaultSkills;

    [field: SerializeField] public SerializableDictionary<string, SkillSaveData> upgradesData;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of UpgradesControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );
    }

    public List<SkillSO> GetUnlockedSkills() {
        List<SkillSO> skills = new List<SkillSO>();

        foreach( KeyValuePair<string, SkillSaveData> entry in upgradesData ) {
            if( entry.Value.wasBought ) {
                skills.Add( GetSkill( entry.Key ) );
            }
        }

        return skills;
    }

    public bool HasUpgrade( SkillType upgrade ) {
        if ( upgradesData.TryGetValue( upgrade.ToString(), out SkillSaveData data ) ) {
            return data.wasBought;
        }

        return false;
    }

    public SkillSO GetSkill(string name) {
        foreach( SkillSO skill in defaultSkills ) {
            if ( skill.skillType.ToString() == name ) return skill;
        }

        return null;
    }

    public SkillSaveData GetUpgrade( string id ) {
        if ( upgradesData.TryGetValue( id, out SkillSaveData upgrade ) ) return upgrade;
        else return null;
    }

    private void InitUpgrades( SaveData saveData ) {
        foreach ( SkillSO upgrade in defaultSkills ) {
            SkillSaveData newData = new SkillSaveData();

            newData.name = upgrade.skillType.ToString();
            newData.wasBought = false;

            upgradesData.Add( upgrade.skillType.ToString(), newData );
            saveData.skills.Add( newData );
        }
    }

    public void LoadData( SaveData data ) {
        if ( data.skills == null || data.skills.Count <= 0 ) { InitUpgrades( data ); return; }

        upgradesData.Clear();

        List<SkillSaveData> upgrades = data.skills;

        foreach ( SkillSaveData upgrade in upgrades ) {
            upgradesData.Add( upgrade.name, upgrade );
        }
    }

    public void SaveData( SaveData data ) {

    }
}

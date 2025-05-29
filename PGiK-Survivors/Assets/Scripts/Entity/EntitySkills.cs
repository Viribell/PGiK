using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySkills : MonoBehaviour, IEntityComponent {
    private PlayerController playerController;

    [field: SerializeField] private SerializableDictionary<SkillType, SkillSO> cachedSkills;

    private void Awake() {
        cachedSkills = new SerializableDictionary<SkillType, SkillSO>();
    }

    private void Update() {
        if ( PauseControl.IsGamePaused ) { return; }

        UpdateSkills();
    }


    private void UpdateSkills() {
        foreach ( KeyValuePair<SkillType, SkillSO> entry in cachedSkills ) {
            entry.Value.Tick( (EntityController)playerController, Time.deltaTime );
        }
    }

    public void AddSkill( SkillSO skill ) {
        SkillType type = skill.skillType;

        if ( !CreateSkill( type, skill) ) {
            Debug.Log( "Skill: " + type.ToString() + " already in use." );
        }
    }

    private bool CreateSkill( SkillType type, SkillSO skillTemplate ) {
        if ( !cachedSkills.ContainsKey( type ) ) {
            cachedSkills[type] = Instantiate( skillTemplate );
            cachedSkills[type].Init();
            cachedSkills[type].Activate();
            return true;
        }

        return false;
    }

    #region EntityFunc
    public void LoadEntityController( EntityController controller ) {
        playerController = ( PlayerController )controller;
    }

    public void ReloadEntityData() {}
    #endregion
}

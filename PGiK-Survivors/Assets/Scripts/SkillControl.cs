using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour {
    public static SkillControl Instance { get; private set; }

    [field: SerializeField] private List<SkillSO> allSkills;

    [field: SerializeField] public List<SkillSO> availableSkills;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of SkillControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        InitAvailableSkills();

    }

    private void Start() {
        LoadUnlockedSkills();
    }

    private void LoadUnlockedSkills() {
        List<SkillSO> unlockedSkills = SkillsSaveControl.Instance.GetUnlockedSkills();

        if ( unlockedSkills == null || unlockedSkills.Count <= 0 ) return;

        foreach(SkillSO skill in unlockedSkills) {
            AddSkill( skill );
        }
    }

    private void AddSkill( SkillSO skill ) {
        availableSkills.Add( skill );
    }

    private void InitAvailableSkills() {
        availableSkills = new List<SkillSO>();

        foreach(SkillSO skill in allSkills) {
            availableSkills.Add( skill );
        }
    }

    public void TakeSkill(SkillSO skill) {
        availableSkills.Remove( skill );
    }
}

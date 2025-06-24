using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookUI : MonoBehaviour {
    public static SkillBookUI Instance { get; private set; }

    [Header( "UI Elements" )]
    [field: SerializeField] private GameObject skillBookUI;
    [field: SerializeField] private GameObject skillsArea;
    [field: SerializeField] private SkillBoxUI skillBoxPrefab;

    [field: SerializeField] private EntityController choosingEntity;

    private bool isUiActive = false;
    private List<SkillBoxUI> skillBoxes;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of SkillBookUI. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        if ( skillBookUI.activeSelf ) Hide();

        skillBoxes = new List<SkillBoxUI>();
    }

    public void OnSkillClicked(SkillSO skill) {
        SkillControl.Instance.TakeSkill( skill );
        choosingEntity.EntitySkills.AddSkill( skill );
        Deactivate();
    }

    private void LoadSkills() {
        foreach ( SkillSO skill in SkillControl.Instance.availableSkills ) {
            SkillBoxUI newBox = Instantiate( skillBoxPrefab, skillsArea.transform );
            newBox.Init( skill );

            skillBoxes.Add( newBox );
        }
    }

    private void UnloadSkills() {
        foreach ( SkillBoxUI box in skillBoxes ) {
            Destroy( box.gameObject );
        }

        skillBoxes.Clear();
    }

    public void Activate(EntityController source) {
        choosingEntity = source;
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
        PauseControl.SetPause( true );
        skillBookUI.SetActive( true );
        isUiActive = true;
        LoadSkills();
    }

    public void Deactivate() {
        UnloadSkills();
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        skillBookUI.SetActive( false );
        PauseControl.SetPause( false );
        isUiActive = false;
    }

    private void Show() { skillBookUI.SetActive( true ); }
    private void Hide() { skillBookUI.SetActive( false ); }
}

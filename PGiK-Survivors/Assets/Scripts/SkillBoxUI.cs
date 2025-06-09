using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxUI : MonoBehaviour {
    [field: SerializeField] private TextMeshProUGUI skillNameText;

    [field: SerializeField] private Button skillButton;
    [field: SerializeField] private SkillSO skill;

    public void Init(SkillSO skill) {
        this.skill = skill;

        SetSkillNameText( skill.skillName );
    }

    public void OnSkillCliked() {
        SkillBookUI.Instance.OnSkillClicked( skill );
    }

    private void SetSkillNameText( string text ) { skillNameText.text = text; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookInteractable : Interactable {
    [field: SerializeField] private SkillSO givenSkill; //DELETE LATER

    protected override void Interact() {
        if ( !CanInteract() ) return;

        RefCacheControl.Instance.Player.EntitySkills.AddSkill( givenSkill ); //DELETE LATER, NEEDS TO OPEN SKILL CHOICE UI

        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class NPCRescuceInteractable : Interactable {
    [SerializeField] private NPCRescueSO rescueData;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject depletedVisual;

    private DialogueSO currDialogue;
    private NPCSO npcToRescue;

    private bool isRescued = false;

    private void Awake() {
        if( rescueData != null ) Init();
    }

    public void RescueNPC() {
        GameState.Instance.RescueNPC( npcToRescue );

        isRescued = true;

        SwitchVisual();
    }

    private void SwitchVisual() {
        visual.SetActive( false );
        depletedVisual.SetActive( true );
    }

    protected override void Interact() {
        if ( !CanInteract() ) return;

        DialogueControl.Instance.StartDialogue( currDialogue, npcToRescue.npcName );
    }

    private void Init() {
        if ( rescueData.defaultSprite != null ) {  visual.GetComponent<SpriteRenderer>().sprite = rescueData.defaultSprite; }
        if ( rescueData.depletedSprite != null ) { depletedVisual.GetComponent<SpriteRenderer>().sprite = rescueData.depletedSprite; }

        npcToRescue = rescueData.npc;
        currDialogue = rescueData.startDialogue;
    }

    protected override bool CanInteract() { return !isRescued; }

    public void SetCurrDialogue( DialogueSO newDialogue ) { currDialogue = newDialogue; }
}

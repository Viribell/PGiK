using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : Interactable {
    [SerializeField] public NPCSO npcData;
    [SerializeField] public bool isAvailable;

    private void Awake() {
        if ( npcData != null ) Init();
    }

    private void Start() {
        CheckIfAvailable();

        SetActive( isAvailable );
    }

    protected override void Interact() {
        if ( !CanInteract() ) return;

        DialogueControl.Instance.StartDialogue( npcData );
    }

    private void Init() {
        if ( npcData.sprite != null ) {
            gameObject.transform.Find( "Sprite" ).GetComponent<SpriteRenderer>().sprite = npcData.sprite;
        }
    }

    protected override bool CanInteract() { return isAvailable; }
    public void CheckIfAvailable() { isAvailable = GameState.Instance.IsAvailable( npcData ); }
    public void SetActive( bool active ) { gameObject.SetActive( active ); }
    public void RefreshActive() { CheckIfAvailable(); SetActive( isAvailable ); }
}

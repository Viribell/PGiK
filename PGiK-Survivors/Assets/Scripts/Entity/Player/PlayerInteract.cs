using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteract : MonoBehaviour, IEntityComponent {
    public GameObject interactIcon;

    private Interactable interactableInRange = null;
    private PlayerController playerController;

    private void Start() {
        interactIcon.SetActive( false );
    }

    private void Update() {
        if ( PauseControl.IsGamePaused ) return;
        Interact();
    }

    private void Interact() {
        if ( interactableInRange == null ) return;

        if ( GameInput.Instance.myInputActions.Player.Interact.triggered ) {
            interactableInRange.BaseInteract();

            if ( interactableInRange != null && !interactableInRange.BaseCanInteract() ) interactIcon.SetActive( false );
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if( collision.TryGetComponent(out Interactable interactable) && interactable.BaseCanInteract() ) {
            interactableInRange = interactable;
            interactIcon.SetActive( true );
        }
    }

    private void OnTriggerExit2D( Collider2D collision ) {
        if ( collision.TryGetComponent( out Interactable interactable ) && interactable == interactableInRange ) {
            interactableInRange = null;
            interactIcon.SetActive( false );
        }
    }

    public void LoadEntityController( EntityController controller ) {
        playerController = (PlayerController)controller;
    }

    public void ReloadEntityData() {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteract : MonoBehaviour {
    public GameObject interactIcon;

    private Interactable interactableInRange = null;

    private void Start() {
        interactIcon.SetActive( false );
    }

    private void Update() {
        Interact();
    }

    private void Interact() {
        if ( interactableInRange == null ) return;

        if ( GameInput.Instance.myInputActions.Player.Interact.triggered ) {
            interactableInRange.BaseInteract();
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
}

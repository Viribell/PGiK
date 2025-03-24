using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public void BaseInteract() {
        Interact();
    }

    public bool BaseCanInteract() {
        return CanInteract();
    }

    protected virtual void Interact() { }
    protected virtual bool CanInteract() { return true; }
}

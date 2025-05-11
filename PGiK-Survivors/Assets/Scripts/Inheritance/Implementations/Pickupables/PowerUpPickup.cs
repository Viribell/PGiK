using UnityEngine;

public class PowerUpPickup : Pickupable {
    [SerializeField] private StatusEffectSO effectToApply;

    protected override void Interact( PlayerController player ) {
        effectToApply.Apply( player.gameObject );
    }

    protected override bool CanInteract() { return true; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupGravity : MonoBehaviour {
    [SerializeField] private CircleCollider2D pickupArea;
    [field: SerializeField] public float GravitySpeed { get; private set; }

    private List<MagneticBehaviour> pickupables;
    private float pickupRadius;

    private void Awake() {
        pickupables = new List<MagneticBehaviour>();
    }

    private void Start() {
        UpdatePickupRange();
    }

    private void FixedUpdate() {
        if ( PauseControl.IsGamePaused ) { return; }

        foreach ( MagneticBehaviour pickup in pickupables ) {
            pickup.Gravitate( GravitySpeed );
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( collision.TryGetComponent( out MagneticBehaviour mag ) ) {
            pickupables.Add( mag );
        }
    }

    private void OnTriggerExit2D( Collider2D collision ) {
        if ( collision.TryGetComponent( out MagneticBehaviour mag ) ) {
            pickupables.Remove( mag );
        }
    }

    public void UpdatePickupRange() {
        pickupRadius = PlayerControl.Instance.Player.GetStatTotal( StatType.PickupRange );
        ChangeRadius( pickupRadius );
    }

    public void ChangeRadius(float radius) {
        if ( radius <= 0 ) return;

        pickupArea.radius = radius;
    }
}

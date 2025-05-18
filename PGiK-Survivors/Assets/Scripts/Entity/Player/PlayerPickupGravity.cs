using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupGravity : MonoBehaviour, IEntityComponent {
    [SerializeField] private CircleCollider2D pickupArea;
    [field: SerializeField] public float GravitySpeed { get; private set; }

    private PlayerController playerController;

    //private List<MagneticBehaviour> pickupables;
    private float pickupRadius;
    

    //private void Awake() {
    //    pickupables = new List<MagneticBehaviour>();
    //}

    private void Start() {
        UpdatePickupRange();
    }

    private void FixedUpdate() {
        if ( PauseControl.IsGamePaused ) { return; }

        //foreach ( MagneticBehaviour pickup in pickupables ) {
        //    pickup.Gravitate( playerController.transform, GravitySpeed );
        //}
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( collision.TryGetComponent( out MagneticBehaviour mag ) ) {
            //pickupables.Add( mag );
            mag.TurnOn( playerController.transform );
        }
    }

    private void OnTriggerExit2D( Collider2D collision ) {
        if ( collision.TryGetComponent( out MagneticBehaviour mag ) ) {
            //pickupables.Remove( mag );
            mag.TurnOff();
        }
    }

    public void UpdatePickupRange() {
        pickupRadius = playerController.EntityStats.GetStatTotal( StatType.PickupRange );
        ChangeRadius( pickupRadius );
    }

    public void ChangeRadius(float radius) {
        if ( radius <= 0 ) return;

        pickupArea.radius = radius;
    }

    public void LoadEntityController( EntityController controller ) {
        playerController = (PlayerController)controller;
    }

    public void ReloadEntityData() {
        UpdatePickupRange();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public static PlayerControl Instance { get; private set; }

    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public PlayerMove PlayerMove { get; private set; }
    [field: SerializeField] public PlayerPickupGravity PlayerPickup { get; private set; }
    [field: SerializeField] public PlayerLevel PlayerLevel { get; private set; }
    [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
    [field: SerializeField] public PlayerStatuses PlayerStatuses { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; }
        else {
            Debug.Log( "There is more than one instance of PlayerControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }
    }
}

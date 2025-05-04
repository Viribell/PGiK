using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {
    [field: Header("Player Entity Config")]
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public PlayerPickupGravity PlayerPickup { get; private set; }
    [field: SerializeField] public PlayerLevel PlayerLevel { get; private set; }

    [HideInInspector] public ClassSO PlayerData { get { return ( ClassSO )EntityData; } }

    protected override void UploadControllerToComponents() {
        Player.LoadEntityController( this );
        PlayerPickup.LoadEntityController( this );
        PlayerLevel.LoadEntityController( this );
    }

    public override Vector2 GetMoveVector() {
        return GameInput.Instance.GetMovementVectorNormalized();
    }

    #region Events
    public override void OnEntityDeath() {
        Debug.Log( "Player death" );
        Player.Die();
    }

    public override void OnHealthChanged() {
        
    }

    public override void OnDamaged( float value ) {
        if ( value < 0 ) return;

        QuestControl.Instance.UpdateValuesQuests( TrackedType.DamageTaken, value );
    }

    public override void OnHealed( float value ) {
        if ( value < 0 ) return;
    }
    #endregion
}

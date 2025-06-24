using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {
    [field: Header("Player Entity Config")]
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public PlayerPickupGravity PlayerPickup { get; private set; }
    [field: SerializeField] public PlayerLevel PlayerLevel { get; private set; }
    [field: SerializeField] public PlayerInteract PlayerInteract { get; private set; }

    [HideInInspector] public ClassSO PlayerData { get { return ( ClassSO )EntityData; } }
    [HideInInspector] public ClassSO DefaultPlayerData { get { return ( ClassSO )DefaultEntityData; } }

    [field: SerializeField] private EventChannelSO classChange;
    [field: SerializeField] private EventChannelSO onHealthChanged;
    [field: SerializeField] private EventChannelSO onMaxHealthChanged;

    private bool wasRevived = false;

    private void Start() {
        if( GameState.Instance.chosenPlayerClass != null ) {
            UploadEntityData( GameState.Instance.chosenPlayerClass );
            classChange?.Raise();
        }

        wasRevived = false;

        EntityStats.AddStatMod( UpgradesControl.Instance.GetStatMods(), StatModHandlingOptions.NoDuplicateModAdd );
        if ( BasicUiControl.Instance != null ) BasicUiControl.Instance.Init();
    }

    protected override void UploadControllerToComponents() {
        Player.LoadEntityController( this );
        PlayerPickup.LoadEntityController( this );
        PlayerLevel.LoadEntityController( this );
        PlayerInteract.LoadEntityController( this );
    }

    protected override void ReloadData() {
        Player.ReloadEntityData();
        PlayerPickup.ReloadEntityData();
        PlayerLevel.ReloadEntityData();
        PlayerInteract.ReloadEntityData();
        if( BasicUiControl.Instance != null ) BasicUiControl.Instance.Init();
    }

    public override Vector2 GetMoveVector() {
        return GameInput.Instance.GetMovementVectorNormalized();
    }

    #region Events
    public override void OnEntityDeath() {
        Debug.Log( "Player death" );

        if( UpgradesControl.Instance.HasUpgrade( UpgradeType.PlusLife ) && !wasRevived ) {
            Debug.Log( "Extra life usage... Player Revived!" );
            Player.Revive();
            wasRevived = true;
            return;
        }
 
        LevelControl.Instance.EndScreen( EndScreenType.DeathScreen );

        Player.Die();
    }

    public override void OnHealthChanged() {
        onMaxHealthChanged?.Raise();
    }

    public override void OnDamaged( float value ) {
        if ( value < 0 ) return;

        QuestControl.Instance.UpdateValuesQuests( TrackedType.DamageTaken, value );
        onHealthChanged?.Raise();
    }

    public override void OnHealed( float value ) {
        if ( value < 0 ) return;

        onHealthChanged?.Raise();
    }

    public override void OnStatEdgeCase( StatType type ) {
        if ( type == StatType.PickupRange ) PlayerPickup.UpdatePickupRange();
    }
    #endregion
}

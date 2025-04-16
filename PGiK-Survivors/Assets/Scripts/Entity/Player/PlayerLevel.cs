using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, IEntityComponent {
    [Header("Player Level State")]
    [field: SerializeField] private int level = 1;
    [field: SerializeField] private float currXP;
    [field: SerializeField] private float requiredXP;

    [Header("Event Channels")]
    [field: SerializeField] private EventChannelSO OnXPGain;
    [field: SerializeField] private EventChannelSO OnLevelUp;

    [Header("RequiredXP Scaling Factors")]
    [Range( 1.0f, 400.0f )]
    [field: SerializeField] private float addFactor = 100.0f;
    [Range( 2.0f, 5.0f )]
    [field: SerializeField] private float powerFactor = 2.0f;
    [Range( 1.0f, 20.0f )]
    [field: SerializeField] private float divFactor = 7.0f;
    [Range( 1.0f, 10.0f )]
    [field: SerializeField] private float finalDivFactor = 3.0f;

    private PlayerController playerController;

    private List<StatModifier> classLvlMods;

    private void Start() {
        if ( level <= 0 ) level = 1;

        requiredXP = CalculateRequiredXP();

        classLvlMods = playerController.PlayerData?.GetLevelUpMods();
    }

    private void OnEnable() {
        GameInput.Instance.myInputActions.Player.TestButton.performed += TestButton_performed;
    }

    private void OnDisable() {
        GameInput.Instance.myInputActions.Player.TestButton.performed -= TestButton_performed;
    }

    private void Update() {
        if ( PauseControl.IsGamePaused ) { return; }

        if ( HasEnoughXP() ) LevelUp();
    }

    private void TestButton_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj ) {
        GainXP( 20 );
        AudioManager.Instance.PlayPoolSound( playerController.EntityAudio?.GetSoundData( EntitySoundType.Undefined ), transform.position ); // TEMP_TEST_TO_ERASE
        playerController.EntityHealth.Damage( 10 );
    }

    public void LevelUp() {
        level++;
        currXP = currXP - requiredXP; // optionally maybe add some rounding
        requiredXP = CalculateRequiredXP();

        playerController.EntityStats.AddStatMod( classLvlMods, StatModHandlingOptions.NoDuplicateModAdd );

        OnLevelUp?.Raise(); // player level up is part of it for now, maybe it should be just called here individually
    }

    public void GainXP(float xpGained) {
        currXP += CalculateTotalXP( xpGained );

        OnXPGain?.Raise();
    }

    private float CalculateTotalXP(float xpGained) {
        float totalXPGained = xpGained;

        totalXPGained *= playerController.EntityStats.GetStatTotal( StatType.ExpGain );

        return totalXPGained;
    }

    private int CalculateRequiredXP() {
        int totalRequiredXP = 0;

        totalRequiredXP = level * (int)Mathf.Floor( Mathf.Pow( level, 2 ) + (addFactor * (level / divFactor)) * Mathf.Pow( powerFactor, level / divFactor ) );
        totalRequiredXP = (int)(totalRequiredXP / finalDivFactor);

        return totalRequiredXP;
    }

    public bool HasEnoughXP() { return currXP >= requiredXP; }
    public float Level() { return level; }
    public float CurrXP() { return currXP; }
    public float RequiredXP() { return requiredXP; }

    public void LoadEntityController( EntityController controller ) {
        playerController = (PlayerController)controller;
    }
}

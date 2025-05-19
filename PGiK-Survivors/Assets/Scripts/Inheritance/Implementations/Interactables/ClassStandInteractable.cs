using UnityEngine;

public class ClassStandInteractable : Interactable {
    [Header("Basic Info")]
    [field: SerializeField] private ClassSO playerClass;
    [field: SerializeField] private SpriteRenderer spriteRenderer;
    [field: SerializeField] private EventChannelSO eventChannel;

    [Header("State")]
    [field: SerializeField] private bool isUnlocked = true;
    [field: SerializeField] private bool isChosen = false;

    private void Awake() {
        if ( playerClass != null ) Init();

        gameObject.SetActive( isUnlocked );
        gameObject.SetActive( !isChosen );
    }

    public void Init() {
        if ( spriteRenderer != null ) spriteRenderer.sprite = playerClass.sprite;
    }

    protected override void Interact() {
        if ( !CanInteract() ) return;

        GameState.Instance.chosenPlayerClass = playerClass;

        RefCacheControl.Instance.Player.UploadEntityData( playerClass );

        isChosen = true;

        eventChannel?.Raise();
    }

    protected override bool CanInteract() {
        return true;
    }

    public void UpdateState() {
        if ( !isUnlocked ) return;

        if( RefCacheControl.Instance.Player.PlayerData == playerClass ) {
            isChosen = true;
            Deactivate();

        } else {
            isChosen = false;
            Activate();
        }

    }

    public void Activate() { gameObject.SetActive( true ); }
    public void Deactivate() { gameObject.SetActive( false ); }
}

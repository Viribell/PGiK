using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillsUpgradesControl : MonoBehaviour {
    public static SkillsUpgradesControl Instance { get; private set; }

    [Header( "Base Info" )]
    [field: SerializeField] private GameObject upgradesUI;
    [field: SerializeField] private GameObject upgradeArea;
    [field: SerializeField] private SkillUpgradeBox upgradeBoxPrefab;

    [Header( "Currency Source" )]
    [field: SerializeField] public ResourceSO slimeResource;
    [field: SerializeField] public ResourceSO shardResource;
    [field: SerializeField] public ResourceSO ceramicsResource;
    [field: SerializeField] public ResourceSO furResource;
    [field: SerializeField] public ResourceSO bloodResource;

    [Header("Currency Text")]
    [field: SerializeField] private TextMeshProUGUI slimeText;
    [field: SerializeField] private TextMeshProUGUI shardText;
    [field: SerializeField] private TextMeshProUGUI ceramicsText;
    [field: SerializeField] private TextMeshProUGUI furText;
    [field: SerializeField] private TextMeshProUGUI bloodText;

    [Header( "State" )]
    [field: SerializeField] private bool isUiActive = false;

    private List<SkillUpgradeBox> upgradeBoxes;


    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of SkillsUpgradesControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        upgradeBoxes = new List<SkillUpgradeBox>();
    }

    private void Start() {
        if ( upgradesUI.activeSelf ) Deactivate();

        LoadUpgrades();
        UpdateCurrencyStates();
    }

    private void Update() {
        if ( !isUiActive ) return;

        if ( GameInput.Instance.myInputActions.Dummy.Exit.WasPressedThisFrame() ) {
            Deactivate();
        }
    }

    private void LoadUpgrades() {
        foreach ( KeyValuePair<string, SkillSaveData> entry in SkillsSaveControl.Instance.upgradesData ) {
            SkillUpgradeBox newBox = Instantiate( upgradeBoxPrefab, upgradeArea.transform );
            newBox.Init( entry.Value, SkillsSaveControl.Instance.GetSkill( entry.Key ) );

            upgradeBoxes.Add( newBox );
        }
    }

    public int GetResourceAmount( ResourceSO resource ) {
        return GameResources.Instance.GetCount( resource );
    }

    public void PayAmount( ResourceSO resource, int amount ) {
        GameResources.Instance.SubtractFromResource( resource, amount );
    }

    public void UpdateCurrencyStates() {
        SetSlimeText( GetResourceAmount( slimeResource ).ToString() );
        SetShardText( GetResourceAmount( shardResource ).ToString() );
        SetCeramicsText( GetResourceAmount( ceramicsResource ).ToString() );
        SetFurText( GetResourceAmount( furResource ).ToString() );
        SetBloodText( GetResourceAmount( bloodResource ).ToString() );
    }

    public void UpdateUpgradeBoxes() {
        foreach ( SkillUpgradeBox box in upgradeBoxes ) {
            box.UpdateState();
        }
    }

    private void ActivateDelayed() {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
    }

    public void SetSlimeText( string text ) { slimeText.text = text; }
    public void SetShardText( string text ) { shardText.text = text; }
    public void SetCeramicsText( string text ) { ceramicsText.text = text; }
    public void SetFurText( string text ) { furText.text = text; }
    public void SetBloodText( string text ) { bloodText.text = text; }

    public void Activate() {
        Invoke( "ActivateDelayed", 0.5f );
        upgradesUI.SetActive( true );
        isUiActive = true;
        UpdateCurrencyStates();
    }

    public void Deactivate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        upgradesUI.SetActive( false );
        isUiActive = false;
    }
}

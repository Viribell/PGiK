using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialsUpgradesControl : MonoBehaviour {
    public static MaterialsUpgradesControl Instance { get; private set; }

    [Header( "Base Info" )]
    [field: SerializeField] private GameObject upgradesUI;
    [field: SerializeField] private GameObject upgradeArea;
    [field: SerializeField] private MaterialUpgradeBox upgradeBoxPrefab;

    [Header( "Currency Source" )]
    [field: SerializeField] public ResourceSO sunflowerResource;
    [field: SerializeField] public ResourceSO truffleResource;
    [field: SerializeField] public ResourceSO carrionResoruce;
    [field: SerializeField] public ResourceSO milkResource;
    [field: SerializeField] public ResourceSO carrotResource;
    [field: SerializeField] public ResourceSO lifeResource;

    [Header( "Currency Text" )]
    [field: SerializeField] private TextMeshProUGUI sunflowerText;
    [field: SerializeField] private TextMeshProUGUI truffleText;
    [field: SerializeField] private TextMeshProUGUI carrionText;
    [field: SerializeField] private TextMeshProUGUI milkText;
    [field: SerializeField] private TextMeshProUGUI carrotText;
    [field: SerializeField] private TextMeshProUGUI lifeText;

    [Header( "State" )]
    [field: SerializeField] private bool isUiActive = false;

    private List<MaterialUpgradeBox> upgradeBoxes;


    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of MaterialsUpgradesControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        upgradeBoxes = new List<MaterialUpgradeBox>();
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
        foreach ( KeyValuePair<string, MaterialSaveData> entry in MaterialsSaveControl.Instance.upgradesData ) {
            MaterialUpgradeBox newBox = Instantiate( upgradeBoxPrefab, upgradeArea.transform );
            newBox.Init( entry.Value, MaterialsSaveControl.Instance.GetFood( entry.Key ) );

            upgradeBoxes.Add( newBox );
        }
    }

    public void UpdateCurrencyStates() {
        SetSunflowerText( GetResourceAmount( sunflowerResource ).ToString() );
        SetTruffleText( GetResourceAmount( truffleResource ).ToString() );
        SetCarrionText( GetResourceAmount( carrionResoruce ).ToString() );
        SetMilkText( GetResourceAmount( milkResource ).ToString() );
        SetCarrotText( GetResourceAmount( carrotResource ).ToString() );
        SetLifeText( GetResourceAmount( lifeResource ).ToString() );
    }

    public int GetResourceAmount( ResourceSO resource ) {
        return GameResources.Instance.GetCount( resource );
    }

    public void PayAmount( ResourceSO resource, int amount ) {
        GameResources.Instance.SubtractFromResource( resource, amount );
    }

    public void UpdateUpgradeBoxes() {
        foreach ( MaterialUpgradeBox box in upgradeBoxes ) {
            box.UpdateState();
        }
    }

    public void SetSunflowerText( string text ) { sunflowerText.text = text; }
    public void SetTruffleText( string text ) { truffleText.text = text; }
    public void SetCarrionText( string text ) { carrionText.text = text; }
    public void SetMilkText( string text ) { milkText.text = text; }
    public void SetCarrotText( string text ) { carrotText.text = text; }
    public void SetLifeText( string text ) { lifeText.text = text; }

    private void ActivateDelayed() {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
    }

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

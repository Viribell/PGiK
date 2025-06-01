using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUpgradesControl : MonoBehaviour {
    public static CurrencyUpgradesControl Instance { get; private set; }

    [Header( "Base Info" )]
    [field: SerializeField] private GameObject upgradesUI;
    [field: SerializeField] private GameObject upgradeArea;
    [field: SerializeField] private CurrencyUpgradeBox upgradeBoxPrefab;
    [field: SerializeField] private TextMeshProUGUI goldText;
    [field: SerializeField] private ResourceSO gold;

    [Header( "State" )]
    [field: SerializeField] private bool isUiActive = false;

    private List<CurrencyUpgradeBox> upgradeBoxes;
    

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of CurrencyUpgradesControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        upgradeBoxes = new List<CurrencyUpgradeBox>();
    }

    private void Start() {
        if ( upgradesUI.activeSelf ) Deactivate();

        LoadUpgrades();
        UpdateGoldState();
    }

    private void Update() {
        if ( !isUiActive ) return;

        if ( GameInput.Instance.myInputActions.Dummy.Exit.WasPressedThisFrame() ) {
            Deactivate();
        }
    }

    private void LoadUpgrades() {
        foreach( KeyValuePair<string, UpgradeSaveData> entry in UpgradesControl.Instance.upgradesData ) {
            CurrencyUpgradeBox newBox = Instantiate( upgradeBoxPrefab, upgradeArea.transform );
            newBox.Init( entry.Value );

            upgradeBoxes.Add( newBox );
        }
    }

    public void UpdateGoldState() {
        SetGoldText( GetGoldAmount().ToString() );
    }

    public int GetGoldAmount() {
        return GameResources.Instance.GetCount( gold );
    }

    public void PayGoldAmount( int amount ) {
        GameResources.Instance.SubtractFromResource( gold, amount );
    }
    
    private void SetGoldText( string text ) { goldText.text = text; }

    public void UpdateUpgradeBoxes() {
        foreach( CurrencyUpgradeBox box in upgradeBoxes ) {
            box.UpdateState();
        }
    }

    private void ActivateDelayed() {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
    }

    public void Activate() {
        Invoke( "ActivateDelayed", 0.5f );
        upgradesUI.SetActive( true );
        isUiActive = true;
        UpdateGoldState();
    }

    public void Deactivate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        upgradesUI.SetActive( false );
        isUiActive = false;
    }
}

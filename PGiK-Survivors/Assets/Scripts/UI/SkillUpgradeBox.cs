using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradeBox : MonoBehaviour {
    [field: SerializeField] private TextMeshProUGUI upgradeNameText;
    [field: SerializeField] private TextMeshProUGUI currentLevelText;
    [field: SerializeField] private TextMeshProUGUI maxLevelText;

    [field: SerializeField] private Button buyButton;
    [field: SerializeField] private SkillSaveData upgradeData;
    [field: SerializeField] private SkillSO skill;

    [Header("Currency Text")]
    [field: SerializeField] private TextMeshProUGUI slimeText;
    [field: SerializeField] private TextMeshProUGUI shardText;
    [field: SerializeField] private TextMeshProUGUI ceramicsText;
    [field: SerializeField] private TextMeshProUGUI furText;
    [field: SerializeField] private TextMeshProUGUI bloodText;

    public void Init( SkillSaveData data, SkillSO skill ) {
        this.skill = skill;
        upgradeData = data;
        UpdateState();
        UpdateIfMaxLevel();
    }

    public void OnBuyClicked() {
        if ( DoesntHaveEnough() ) OnBuyFailed();
        else OnBuySuccess();
    }

    public void UpdateState() {
        SetNameText( upgradeData.name );
        SetCurrentLevelText( "0" );
        SetMaxLevelText( "1" );
        SetPrices();
    }

    private bool DoesntHaveEnough() {
        foreach( KeyValuePair<ResourceSO, int> entry in skill.price ) {
            if ( SkillsUpgradesControl.Instance.GetResourceAmount( entry.Key ) < entry.Value ) return true;
        }

        return false;
    }

    private void SetPrices() {
        foreach ( KeyValuePair<ResourceSO, int> entry in skill.price) {
            ResourceSO key = entry.Key;

            if ( key == SkillsUpgradesControl.Instance.slimeResource ) SetSlimeText( entry.Value.ToString() );
            else if ( key == SkillsUpgradesControl.Instance.shardResource ) SetShardText( entry.Value.ToString() );
            else if ( key == SkillsUpgradesControl.Instance.ceramicsResource ) SetCeramicsText( entry.Value.ToString() );
            else if ( key == SkillsUpgradesControl.Instance.furResource ) SetFurText( entry.Value.ToString() );
            else if ( key == SkillsUpgradesControl.Instance.bloodResource ) SetBloodText( entry.Value.ToString() );
        }
    }

    private void Pay() {
        foreach ( KeyValuePair<ResourceSO, int> entry in skill.price ) {
            SkillsUpgradesControl.Instance.PayAmount( entry.Key, entry.Value );
        }
    }

    private void OnBuySuccess() {
        Debug.Log( "Bought upgrade!" );

        Pay();
        upgradeData.wasBought = true;

        UpdateIfMaxLevel();
        SkillsUpgradesControl.Instance.UpdateCurrencyStates();
    }

    private void OnBuyFailed() {
        Debug.Log( "Not enough resources!" );
    }

    private void UpdateIfMaxLevel() {
        if ( !upgradeData.wasBought ) return;

        LockBuy();


        SetCurrentLevelText( "1" );
        SetPricesZero();
    }

    private void SetPricesZero() {
        SetSlimeText( "0" );
        SetShardText( "0" );
        SetCeramicsText( "0" );
        SetFurText( "0" );
        SetBloodText( "0" );
    }

    private void SetNameText( string text ) { upgradeNameText.text = text; }
    private void SetCurrentLevelText( string text ) { currentLevelText.text = text; }
    private void SetMaxLevelText( string text ) { maxLevelText.text = text; }

    private void SetSlimeText( string text ) { slimeText.text = text; }
    private void SetShardText( string text ) { shardText.text = text; }
    private void SetCeramicsText( string text ) { ceramicsText.text = text; }
    private void SetFurText( string text ) { furText.text = text; }
    private void SetBloodText( string text ) { bloodText.text = text; }

    private void LockBuy() { buyButton.interactable = false; }
    private void UnlockBuy() { buyButton.interactable = true; }
}

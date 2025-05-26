using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUpgradeBox : MonoBehaviour {
    [field: SerializeField] private TextMeshProUGUI upgradeNameText;
    [field: SerializeField] private TextMeshProUGUI currentLevelText;
    [field: SerializeField] private TextMeshProUGUI maxLevelText;
    [field: SerializeField] private TextMeshProUGUI currentPriceText;
    [field: SerializeField] private TextMeshProUGUI increasePerLevelText;

    [field: SerializeField] private Button buyButton;
    [field: SerializeField] private UpgradeSaveData upgradeData;

    private int currentPrice;


    public void Init( UpgradeSaveData data ) {
        upgradeData = data;
        UpdateState();
        UpdateIfMaxLevel();
    }

    public void OnBuyClicked() {
        int goldAmount = CurrencyUpgradesControl.Instance.GetGoldAmount();

        if ( goldAmount < currentPrice ) OnBuyFailed();
        else OnBuySuccess();


        UpdateState();
        CurrencyUpgradesControl.Instance.UpdateGoldState();
    }

    public void UpdateState() {
        currentPrice = upgradeData.basicPrice + (int)( upgradeData.currentLevel * (upgradeData.basicPrice * upgradeData.priceIncreasePercentage ) );

        SetNameText( upgradeData.id );
        SetCurrentLevelText( upgradeData.currentLevel.ToString() );
        SetMaxLevelText( upgradeData.maxLevels.ToString() );
        if( !HasReachedMaxLevel() ) SetCurrentPriceText( currentPrice.ToString() );
        SetIncreaseText( ( upgradeData.upgradeMod.basicValue * 100 ).ToString() + " %" );
    }

    private void OnBuySuccess() {
        Debug.Log( "Bought upgrade!" );

        CurrencyUpgradesControl.Instance.PayGoldAmount( currentPrice );
        upgradeData.currentLevel++;
        upgradeData.UpdateMod();

        UpdateIfMaxLevel();
    }

    private void OnBuyFailed() {
        Debug.Log( "Not enough gold!" );
    }

    private void UpdateIfMaxLevel() {
        if ( !HasReachedMaxLevel() ) return;

        LockBuy();
        SetCurrentPriceText( "None" );
    }

    private bool HasReachedMaxLevel() {
        return upgradeData.currentLevel == upgradeData.maxLevels;
    }

    private void SetNameText( string text ) { upgradeNameText.text = text; }
    private void SetCurrentLevelText( string text ) { currentLevelText.text = text; }
    private void SetMaxLevelText( string text ) { maxLevelText.text = text; }
    private void SetCurrentPriceText( string text ) { currentPriceText.text = text; }
    private void SetIncreaseText( string text ) { increasePerLevelText.text = text; }

    private void LockBuy() { buyButton.interactable = false; }
    private void UnlockBuy() { buyButton.interactable = true; }
}

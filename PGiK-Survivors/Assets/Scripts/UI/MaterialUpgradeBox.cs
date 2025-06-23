using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUpgradeBox : MonoBehaviour {
    [field: SerializeField] private TextMeshProUGUI upgradeNameText;
    [field: SerializeField] private TextMeshProUGUI currentLevelText;
    [field: SerializeField] private TextMeshProUGUI maxLevelText;
    [field: SerializeField] private Image foodImage;

    [field: SerializeField] private Button buyButton;
    [field: SerializeField] private MaterialSaveData upgradeData;
    [field: SerializeField] private FoodSO food;

    [Header( "Currency Text" )]
    [field: SerializeField] private TextMeshProUGUI sunflowerText;
    [field: SerializeField] private TextMeshProUGUI truffleText;
    [field: SerializeField] private TextMeshProUGUI carrionText;
    [field: SerializeField] private TextMeshProUGUI milkText;
    [field: SerializeField] private TextMeshProUGUI carrotText;
    [field: SerializeField] private TextMeshProUGUI lifeText;


    public void Init( MaterialSaveData data, FoodSO food ) {
        upgradeData = data;
        this.food = food;
        UpdateState();
        UpdateIfMaxLevel();
        foodImage.sprite = food.image;
    }

    public void OnBuyClicked() {
        if ( DoesntHaveEnough() ) OnBuyFailed();
        else OnBuySuccess();
    }

    public void UpdateState() {
        SetNameText( food.name );
        SetCurrentLevelText( upgradeData.currentLevel.ToString() );
        SetMaxLevelText( upgradeData.maxLevels.ToString() );
        SetPrices();
    }

    private bool DoesntHaveEnough() {
        foreach ( KeyValuePair<ResourceSO, int> entry in food.price ) {
            if ( MaterialsUpgradesControl.Instance.GetResourceAmount( entry.Key ) < entry.Value ) return true;
        }

        return false;
    }

    private void SetPrices() {
        foreach ( KeyValuePair<ResourceSO, int> entry in food.price ) {
            ResourceSO key = entry.Key;

            if ( key == MaterialsUpgradesControl.Instance.sunflowerResource ) SetSunflowerText( entry.Value.ToString() );
            else if ( key == MaterialsUpgradesControl.Instance.truffleResource ) SetTruffleText( entry.Value.ToString() );
            else if ( key == MaterialsUpgradesControl.Instance.carrionResoruce ) SetCarrionText( entry.Value.ToString() );
            else if ( key == MaterialsUpgradesControl.Instance.milkResource ) SetMilkText( entry.Value.ToString() );
            else if ( key == MaterialsUpgradesControl.Instance.carrotResource ) SetCarrotText( entry.Value.ToString() );
            else if ( key == MaterialsUpgradesControl.Instance.lifeResource ) SetLifeText( entry.Value.ToString() );
        }
    }

    private void Pay() {
        foreach ( KeyValuePair<ResourceSO, int> entry in food.price ) {
            MaterialsUpgradesControl.Instance.PayAmount( entry.Key, entry.Value );
        }
    }

    private void OnBuySuccess() {
        Debug.Log( "Bought upgrade!" );

        Pay();
        upgradeData.currentLevel++;

        UpdateState();
        UpdateIfMaxLevel();
        SkillsUpgradesControl.Instance.UpdateCurrencyStates();
    }

    private void OnBuyFailed() {
        Debug.Log( "Not enough resources!" );
    }

    private void UpdateIfMaxLevel() {
        if ( !HasReachedMaxLevel() ) return;
        LockBuy();
        SetPricesZero();
    }

    private void SetPricesZero() {
        SetSunflowerText( "0" );
        SetTruffleText( "0" );
        SetCarrionText( "0" );
        SetMilkText( "0" );
        SetCarrotText( "0" );
        SetLifeText( "0" );
    }

    private bool HasReachedMaxLevel() {
        return upgradeData.currentLevel == upgradeData.maxLevels;
    }

    private void SetNameText( string text ) { upgradeNameText.text = text; }
    private void SetCurrentLevelText( string text ) { currentLevelText.text = text; }
    private void SetMaxLevelText( string text ) { maxLevelText.text = text; }

    private void SetSunflowerText( string text ) { sunflowerText.text = text; }
    private void SetTruffleText( string text ) { truffleText.text = text; }
    private void SetCarrionText( string text ) { carrionText.text = text; }
    private void SetMilkText( string text ) { milkText.text = text; }
    private void SetCarrotText( string text ) { carrotText.text = text; }
    private void SetLifeText( string text ) { lifeText.text = text; }

    private void LockBuy() { buyButton.interactable = false; }
    private void UnlockBuy() { buyButton.interactable = true; }
}

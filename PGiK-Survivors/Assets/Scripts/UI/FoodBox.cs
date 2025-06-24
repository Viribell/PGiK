using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodBox : MonoBehaviour {
    [field: SerializeField] private TextMeshProUGUI currentLevelText;

    [field: SerializeField] private Button button;
    [field: SerializeField] public FoodSO food;
    [field: SerializeField] private Image foodImage;
    [field: SerializeField] private ToolTipShow tooltip;

    private int level;
    private Color defaultColor;
    private Color selectedColor;

    public void Init() {
        UpdateLevel();
        UpdateState();
        UpdateIfEmpty();
        foodImage.sprite = food.image;
        defaultColor = button.colors.normalColor;
        selectedColor = button.colors.selectedColor;
        tooltip.Deactivate();
        tooltip.SetTooltipText( food.description );
    }

    public void Init( FoodSO food ) {
        this.food = food;
        UpdateLevel();
        UpdateState();
        UpdateIfEmpty();
        foodImage.sprite = food.image;
        defaultColor = button.colors.normalColor;
        selectedColor = button.colors.selectedColor;
        tooltip.Deactivate();
        tooltip.SetTooltipText( food.description );
    }

    public void UpdateLevel() {
        level = CardControl.Instance.GetFoodLevel( food );
    }

    public void OnBuyClicked() {
        DoSelection();
        if ( food.isImmediate ) CardControl.Instance.HandleFoodEffects( food.foodType, null );
    }

    private void DoSelection() {
        if( CardControl.Instance.selectedFood != null && CardControl.Instance.selectedFood != this ) {
            CardControl.Instance.selectedFood.Unselect();
            CardControl.Instance.selectedFood = null;
        }

        Select();
        CardControl.Instance.selectedFood = this;
    }

    public void Select() {
        ColorBlock colors = button.colors;
        colors.normalColor = selectedColor;

        button.colors = colors;
    }

    public void Unselect() {
        ColorBlock colors = button.colors;
        colors.normalColor = defaultColor;

        button.colors = colors;
    }

    public void UseSuccess() {
        CardControl.Instance.UseFood( food );
        UpdateLevel();
        UpdateState();
        UpdateIfEmpty();
    }

    public void UpdateState() {
        SetCurrentLevelText( level.ToString() );
    }

    private void UpdateIfEmpty() {
        if ( level > 0 ) return;

        LockBuy();
        SetCurrentLevelText( "0" );
    }

    private void SetCurrentLevelText( string text ) { currentLevelText.text = text; }

    private void LockBuy() { button.interactable = false; }
    private void UnlockBuy() { button.interactable = true; }
}

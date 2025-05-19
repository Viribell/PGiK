using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StadiumBox : MonoBehaviour {
    [field: SerializeField] private int stadium;
    [field: SerializeField] private Button button;
    [field: SerializeField] private TextMeshProUGUI numberText;

    [Header( "Conditions" )]
    [field: SerializeField] private bool needsStadiumBeaten = false;
    [field: SerializeField] private int stadiumToBeat;

    private void Awake() {
        SetNumberText( stadium.ToString() );
    }

    public void ReloadStadiumBox() {
        if ( needsStadiumBeaten ) {
            if ( CheckIfUnlocked() ) UnlockStadium();
            else LockStadium();
        }
    }

    private bool CheckIfUnlocked() {
        SaveData save = SaveControl.Instance.GetSaveData();
        LevelType currentLevel = LevelChoiceControl.Instance.chosenLevel;
        LevelSaveData saveData = save.levels.Find( item => string.Equals( item.levelId, currentLevel.ToString() ) );

        if ( saveData == null ) {
            Debug.Log( "No such level found!" );
            return false;
        }

        StadiumData stadiumData = saveData.beatenStadiums.Find( item => item.stadium == stadiumToBeat );

        if ( stadiumData == null ) {
            Debug.Log( "No such stadium found!" );
            return false;
        }

        return stadiumData.isBeaten;
    }

    public void OnStadiumClicked() {
        LevelChoiceControl.Instance.OnStadiumChoice( stadium );
    }

    private void LockStadium() { button.interactable = false; }
    private void UnlockStadium() { button.interactable = true; }
    private void SetNumberText( string text ) { numberText.text = text; }
}

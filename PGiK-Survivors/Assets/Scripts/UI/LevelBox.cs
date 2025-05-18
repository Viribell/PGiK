using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelBox : MonoBehaviour {
    [field: SerializeField] private LevelType level;
    [field: SerializeField] private Button button;

    [Header( "Conditions" )]
    [field: SerializeField] private bool needsLevelUnlocked = false;
    [field: SerializeField] private LevelType levelToCheck;

    private void Start() {
        if ( needsLevelUnlocked ) {
            if ( CheckIfUnlocked() ) UnlockLevel();
            else LockLevel();
        }
    }

    private bool CheckIfUnlocked() {
        SaveData save = SaveControl.Instance.GetSaveData();
        LevelSaveData saveData = save.levels.Find( item => string.Equals( item.levelId, levelToCheck.ToString() ) );

        if( saveData == null ) {
            Debug.Log( "No such level found!" );
            return false;
        }

        return saveData.beatenStadiums[0].isBeaten;
    }

    public void OnLevelClicked() {
        LevelChoiceControl.Instance.OnLevelClicked( level );
    }

    private void LockLevel() { button.interactable = false; }
    private void UnlockLevel() { button.interactable = true; }
}

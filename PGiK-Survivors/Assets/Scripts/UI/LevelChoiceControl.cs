using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelChoiceControl : MonoBehaviour {
    public static LevelChoiceControl Instance { get; private set; }

    [Header("Base Info")]
    [field: SerializeField] private GameObject levelChoiceUI;
    [field: SerializeField] private GameObject levelWindow;
    [field: SerializeField] private GameObject stadiumWindow;

    [Header( "Elements" )]
    [field: SerializeField] private TextMeshProUGUI stadiumText;
    [field: SerializeField] private TextMeshProUGUI levelText;
    [field: SerializeField] private StadiumBox[] stadiumBoxes;

    [Header( "State" )]
    [field: SerializeField] public LevelType chosenLevel;
    [field: SerializeField] private int chosenStadium = 1;
    [field: SerializeField] private bool isLevelChoiceActive = false;

    private void Update() {
        if ( !isLevelChoiceActive ) return;

        if( GameInput.Instance.myInputActions.Dummy.Exit.WasPressedThisFrame() ) {
            Deactivate();
        }
    }

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of LevelChoiceControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }
    }

    private void Start() {
        if ( gameObject.activeSelf ) Deactivate();

        if ( stadiumWindow.gameObject.activeSelf ) DeactivateStadiumWindow();
    }

    public void OnLevelClicked( LevelType level ) {
        chosenLevel = level;
        chosenStadium = 1;
        DeactivateLevelWindow();
        ActivateStadiumWindow();
    }

    public void OnReturnToLevels() {
        DeactivateStadiumWindow();
        ActivateLevelWindow();
    }

    public void OnStadiumChoice(int stadium) {
        chosenStadium = stadium;
        GameState.Instance.chosenLevelStadium = chosenStadium;
        SetStadiumText( chosenStadium.ToString() );
    }

    public void OnStartLevel() {
        SceneLoader.Scene sceneToLoad = SceneLoader.Scene.ForestLevel;

        switch( chosenLevel ) {
            case LevelType.Cave: { sceneToLoad = SceneLoader.Scene.CaveLevel; } break;
            case LevelType.Tundra: { sceneToLoad = SceneLoader.Scene.TundraLevel; } break;
            case LevelType.Desert: { sceneToLoad = SceneLoader.Scene.DesertLevel; } break;
            case LevelType.Cemetery: { sceneToLoad = SceneLoader.Scene.CemeteryLevel; } break;
            case LevelType.Forest: { sceneToLoad = SceneLoader.Scene.ForestLevel; } break;
        }

        SaveControl.Instance.SaveGame();

        GameInput.Instance.DisableMaps();

        SceneLoader.Load( sceneToLoad );
    }

    #region StadiumWindow

    public void ActivateStadiumWindow() {
        ReloadStadiumBoxes();
        SetLevelText( chosenLevel.ToString() );
        SetStadiumText( chosenStadium.ToString() );
        stadiumWindow.SetActive( true );
    }

    public void DeactivateStadiumWindow() {
        stadiumWindow.SetActive( false );
    }

    public void SetStadiumText( string text ) { stadiumText.text = text; }
    public void SetLevelText( string text ) { levelText.text = text; }

    public void ReloadStadiumBoxes() {
        if ( stadiumBoxes == null || stadiumBoxes.Length <= 0 ) return;

        foreach(StadiumBox box in stadiumBoxes) {
            box.ReloadStadiumBox();
        }
    }

    #endregion

    #region LevelWindow
    public void ActivateLevelWindow() {
        levelWindow.SetActive( true );
    }

    public void DeactivateLevelWindow() {
        levelWindow.SetActive( false );
    }
    #endregion

    #region LevelChoice
    public void Activate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
        levelChoiceUI.SetActive( true );
        isLevelChoiceActive = true;
    }

    public void Deactivate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        levelChoiceUI.SetActive( false );
        isLevelChoiceActive = false;
    }
    #endregion
}

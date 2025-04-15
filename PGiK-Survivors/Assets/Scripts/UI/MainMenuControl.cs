using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour {

    [Header("First Selected Button")]
    [SerializeField] private Button firstSelected;

    [Header( "Next Scene Info" )]
    [SerializeField] private SceneLoader.Scene nextScene;


    [Header( "Menu Buttons" )]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    
    [Header( "Used Prefabs" )]
    [SerializeField] private UIConfirmationPopup popupPrefab;

    private void Awake() {
        SelectFirstButton();
    }

    public void OnPlayClicked() {
        LockPlayButton();

        if ( !SaveControl.Instance.HasSaveData() ) SaveControl.Instance.NewGame();
        else SaveControl.Instance.LoadGame();

        SaveControl.Instance.SaveGame();

        SceneLoader.Load( nextScene );
    }

    public void OnOptionsClicked() {
        // POPUP TEST CODE

        UIConfirmationPopup popup = Instantiate( popupPrefab, gameObject.transform );

        if ( popup == null ) Debug.Log( "This is not a confirmation popup prefab!" );

        popup.SetDestroyOnChoice( true );
        popup.Activate( "Text", () => { SelectFirstButton();  }, () => { SelectFirstButton(); } );
    }

    public void OnExitClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }


    private void LockPlayButton() {
        playButton.interactable = false;
    }

    private void SelectFirstButton() {
        if ( firstSelected != null ) firstSelected.Select();
    }
}

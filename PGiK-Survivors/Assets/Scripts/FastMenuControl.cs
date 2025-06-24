using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastMenuControl : MonoBehaviour {
    public static FastMenuControl Instance { get; private set; }

    [field: SerializeField] private GameObject uiArea;
    [field: SerializeField] private GameObject lobbyButton;
    [field: SerializeField] private bool hideLobbyButton = false;

    [Header( "State" )]
    [field: SerializeField] private bool isUiActive = false;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of FastMenuControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        if ( uiArea.activeSelf ) uiArea.SetActive( false );
        if ( hideLobbyButton ) lobbyButton.SetActive( false );
    }

    private void Update() {
        if ( !isUiActive ) return;

        if ( GameInput.Instance.myInputActions.Dummy.Exit.WasPressedThisFrame() ) {
            Deactivate();
        }
    }

    public void OnLobbyClicked() {
        GameInput.Instance.DisableMaps();

        SaveControl.Instance.SaveGame();

        PauseControl.SetPause( false );

        SceneLoader.Load( SceneLoader.Scene.LobbyScene );
    }

    public void OnMenuClicked() {
        GameInput.Instance.DisableMaps();

        SaveControl.Instance.SaveGame();

        PauseControl.SetPause( false );

        SceneLoader.Load( SceneLoader.Scene.MainMenuScene );
    }

    public void Activate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Dummy );
        PauseControl.SetPause( true );
        uiArea.SetActive( true );
        isUiActive = true;
    }

    public void Deactivate() {
        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        PauseControl.SetPause( false );
        uiArea.SetActive( false );
        isUiActive = false;
    }
}

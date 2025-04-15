using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMapType {
    Player,
    Dialogue,
    UI,
    MapFinishScreen
}

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }
    public MyInputActions myInputActions;

    private Dictionary<ActionMapType, InputActionMap> actionMaps;

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

        myInputActions = new MyInputActions();
        InitActionMaps();
        SwitchToMap( ActionMapType.Player );
    }

    private void OnEnable() {
        myInputActions.Player.Menu.performed += Player_Menu_performed;
        myInputActions.UI.Exit.performed += UI_Exit_performed;
        myInputActions.Dialogue.Exit.performed += Dialogue_Exit_performed; //mo¿liwe ¿e do zmiany póŸniej
    }

    private void OnDisable() {
        myInputActions.Player.Menu.performed -= Player_Menu_performed;
        myInputActions.UI.Exit.performed -= UI_Exit_performed;
        myInputActions.Dialogue.Exit.performed -= Dialogue_Exit_performed;
    }

    private void Dialogue_Exit_performed( InputAction.CallbackContext obj ) {
        DialogueControl.Instance.EndDialogue();
    }

    private void UI_Exit_performed( InputAction.CallbackContext obj ) {
        SwitchToMap( ActionMapType.Player );
        PauseControl.SetPause( false );
    }

    private void Player_Menu_performed( InputAction.CallbackContext obj ) {
        SwitchToMap( ActionMapType.UI );
        PauseControl.SetPause( true );
    }

    private void InitActionMaps() {
        actionMaps = new Dictionary<ActionMapType, InputActionMap>();

        actionMaps[ActionMapType.Player] = myInputActions.Player;
        actionMaps[ActionMapType.Dialogue] = myInputActions.Dialogue;
        actionMaps[ActionMapType.UI] = myInputActions.UI;
        actionMaps[ActionMapType.MapFinishScreen] = myInputActions.MapFinishScreen;
    }

    public void SwitchToMap( ActionMapType actionMap ) {
        foreach(KeyValuePair<ActionMapType, InputActionMap> entry in actionMaps) {
            if ( entry.Key == actionMap ) entry.Value.Enable();
            else entry.Value.Disable();
        }
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = myInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetPointerPosition() {
        Vector2 mouseVector = myInputActions.Player.MousePosition.ReadValue<Vector2>();

        return Camera.main.ScreenToWorldPoint( mouseVector );
    }
}

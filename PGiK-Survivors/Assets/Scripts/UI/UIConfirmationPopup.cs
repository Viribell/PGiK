using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIConfirmationPopup : MonoBehaviour{
    [Header( "Popup Config" )]
    [SerializeField] private bool destroyOnChoice = false;
    [SerializeField] private Button firstSelected;

    [Header( "UI Components" )]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake() {
        if ( firstSelected != null ) firstSelected.Select();
    }

    public void Activate( string displayText, UnityAction yesAction, UnityAction noAction ) {
        SetDisplayText( displayText );

        ClearListeners();

        AddYesAction( yesAction );
        AddNoAction( noAction );

        Show();
    }

    private void AddYesAction(UnityAction action) {
        yesButton.onClick.AddListener( () => {
            Hide();
            action();
            if ( destroyOnChoice ) Destroy( gameObject );
        } );
    }

    private void AddNoAction(UnityAction action) {
        noButton.onClick.AddListener( () => {
            Hide();
            action();
            if ( destroyOnChoice ) Destroy( gameObject );
        } );
    }

    private void ClearListeners() {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }

    public void Show() { gameObject.SetActive( true ); }
    public void Hide() { gameObject.SetActive( false ); }
    public void SetDisplayText( string text ) { displayText.text = text; }
    public void SetDestroyOnChoice(bool value) { destroyOnChoice = value; }

}

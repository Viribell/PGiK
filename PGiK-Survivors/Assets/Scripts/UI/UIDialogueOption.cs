using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueOption : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI numberText;

    private int number;
    private DialogueOption option;

    public void InitUIOption(DialogueOption option, int number) {
        SetOption( option );
        SetOptionText( option.optionText );
        SetNumberText( number );
    }

    private void Start() {
        GetComponent<Button>().onClick.AddListener( OnClick );
    }

    private void OnClick() {
        option.eventChannel?.Raise();
        DialogueControl.Instance.FeedNode( option.optionNode );
    }

    public void SetOption( DialogueOption option ) { this.option = option; }
    public void SetNumberText( int number ) { numberText.text = number.ToString(); }
    public void SetOptionText( string option ) { optionText.text = option; }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueControl : MonoBehaviour {
    public static DialogueControl Instance { get; private set; }

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject optionsArea;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private UIDialogueOption uiOptionPrefab;

    private DialogueSO dialogueData;
    private DialogueNodeSO currNode;
    private List<UIDialogueOption> uiOptions;

    private bool isTyping;
    private bool isDialogueActive;
    private bool hasOptions;

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

        if ( dialogueUI != null ) SwitchDialogueUI( false );
        if ( optionsArea != null ) SwitchOptionsArea( false );

        uiOptions = new List<UIDialogueOption>();
    }

    private void Start() {
        GameInput.Instance.myInputActions.Dialogue.Next.performed += DialogueLoop;
    }

    private void OnDisable() {
        GameInput.Instance.myInputActions.Dialogue.Next.performed -= DialogueLoop;
    }

    private void DialogueLoop( UnityEngine.InputSystem.InputAction.CallbackContext obj ) {
        if ( !isDialogueActive ) return;

        NextLine();
    }

    public void StartDialogue(NPCSO npcData) {
        if ( npcData == null || npcData.dialogueData == null) {
            Debug.Log("NPCData or npcDialogueData is null!");
            return;
        }

        if( npcData.dialogueData.firstNode == null ) {
            Debug.Log( "This NPC's dialogue is empty!" );
            return;
        }

        GameInput.Instance.SwitchToMap( ActionMapType.Dialogue );
        PauseControl.SetPause( true );

        SwitchDialogueUI( true );
        dialogueData = npcData.dialogueData;
        if( npcData.npcName != null ) SetNameText( npcData.npcName );

        isDialogueActive = true;
        currNode = dialogueData.firstNode;
        hasOptions = false;

        StartCoroutine( TypeLine() );
    }

    public void StartDialogue( DialogueSO dialogue, string name ) {
        if ( dialogue == null ) {
            Debug.Log( "Dialogue is null!" );
            return;
        }

        if ( dialogue.firstNode == null ) {
            Debug.Log( "This dialogue is empty!" );
            return;
        }

        GameInput.Instance.SwitchToMap( ActionMapType.Dialogue );
        PauseControl.SetPause( true );

        SwitchDialogueUI( true );
        dialogueData = dialogue;
        if ( name != null ) SetNameText( name );

        isDialogueActive = true;
        currNode = dialogueData.firstNode;
        hasOptions = false;

        StartCoroutine( TypeLine() );
    }

    public void EndDialogue() {
        StopAllCoroutines();
        ClearUIOptions();

        isDialogueActive = false;
        dialogueData = null;
        currNode = null;
        hasOptions = false;
        isTyping = false;
        SetDialogueText( "" );
        SetNameText( "" );
        SwitchOptionsArea( false );
        SwitchDialogueUI( false );

        GameInput.Instance.SwitchToMap( ActionMapType.Player );
        PauseControl.SetPause( false );
    }

    private void NextLine() {
        if ( isTyping ) {
            StopAllCoroutines();
            SetDialogueText( currNode.dialogueLine.line );
            isTyping = false;
            if ( hasOptions ) ShowOptions( currNode );

        } else if ( hasOptions ) {
            //nothing

        } else if ( currNode.nextNode != null ) {
            currNode = currNode.nextNode;
            StartCoroutine( TypeLine() );

        } else {
            EndDialogue();
        }
    }

    public void FeedNode(DialogueNodeSO node) {
        ClearUIOptions();

        SwitchOptionsArea( false );
        hasOptions = false;
        isTyping = false;
        currNode = node;

        if ( currNode == null ) {
            EndDialogue();
            return;
        }

        StartCoroutine( TypeLine() );
    }

    private void ClearUIOptions() {
        foreach ( UIDialogueOption uiOption in uiOptions ) {
            Destroy( uiOption.gameObject );
        }

        uiOptions.Clear();
    }

    private IEnumerator TypeLine() {
        isTyping = true;
        SetDialogueText( "" );
        hasOptions = CheckForOptions( currNode );

        string currLine = currNode.dialogueLine.line;

        foreach ( char letter in currLine ) {
            dialogueText.text += letter;
            yield return new WaitForSeconds( dialogueData.typingSpeed );
        }

        isTyping = false;

        if ( hasOptions ) ShowOptions( currNode );
    }

    private void ShowOptions(DialogueNodeSO node) {
        SwitchOptionsArea( true );

        int counter = 1;

        foreach(DialogueOption option in node.options) {
            UIDialogueOption newUIOption = Instantiate( uiOptionPrefab, optionsArea.transform );
            newUIOption.InitUIOption( option, counter );
            uiOptions.Add( newUIOption );
            counter++;
        }
    }

    private bool CheckForOptions( DialogueNodeSO node ) {
        return ( node.options != null ) && ( node.options.Length > 0 );
    }

    private void SetNameText(string text) { nameText.text = text; }
    private void SetDialogueText(string text) { dialogueText.text = text; }
    private void SwitchOptionsArea( bool active ) { optionsArea.SetActive( active ); }
    private void SwitchDialogueUI( bool active ) { dialogueUI.SetActive( active );  }
}

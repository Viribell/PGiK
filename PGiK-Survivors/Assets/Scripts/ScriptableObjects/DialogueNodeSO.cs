using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueOption {
    [SerializeField] public string optionText;
    [SerializeField] public DialogueNodeSO optionNode;
    [SerializeField] public EventChannelSO eventChannel;
}

[System.Serializable]
public class DialogueLine {

    [TextArea( 2, 10 )]
    public string line;

}

[CreateAssetMenu( menuName = "Scriptable Objects/Dialogue/DialogueNode" )]
public class DialogueNodeSO : ScriptableObject {
    [field: SerializeField] public DialogueLine dialogueLine;
    [field: SerializeField] public DialogueNodeSO nextNode;
    [field: SerializeField] public DialogueOption[] options;
}

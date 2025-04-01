using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu( menuName = "Scriptable Objects/Dialogue/Dialogue" )]
public class DialogueSO : ScriptableObject {
    [field: SerializeField] public DialogueNodeSO firstNode;
    [field: SerializeField] public float typingSpeed = 0.1f;

    [field: SerializeField] public AudioClip voiceSound;
    [field: SerializeField] public float voicePitch = 1.0f;
}

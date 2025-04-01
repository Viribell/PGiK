using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/NPC Rescue" )]
public class NPCRescueSO : ScriptableObject {
    [field: SerializeField] public NPCSO npc;
    [field: SerializeField] public Sprite defaultSprite;
    [field: SerializeField] public Sprite depletedSprite;
    [field: SerializeField] public DialogueSO startDialogue;
    [field: SerializeField] public Transform prefab;
}

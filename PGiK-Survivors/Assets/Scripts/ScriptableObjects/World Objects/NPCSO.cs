using UnityEngine;

[CreateAssetMenu( fileName = "NPCSO" )]
public class NPCSO : ScriptableObject {
    [field: SerializeField] public string npcName;
    [field: SerializeField] public Sprite sprite;

    [field: SerializeField] public DialogueSO dialogueData;
}

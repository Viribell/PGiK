using UnityEngine;

public abstract class QuestSO : ScriptableObject {
    [field: Header( "Quest Info" )]
    [field: SerializeField] public string questId;
    [field: SerializeField] public string description;

    [field: Header( "Quest State" )]
    [field: SerializeField] public bool isCompleted = false;
    [field: SerializeField] public float currAmount;
    [field: SerializeField] public float requiredAmount;

    public virtual void Init() {}

    public void Evaluate() {
        if ( currAmount >= requiredAmount ) Complete();
    }

    public void Progress( float value ) { currAmount += value; }
    public void Complete() { isCompleted = true; }
}

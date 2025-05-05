using UnityEngine;

public enum TrackedType {
    DamageTaken,
}

[CreateAssetMenu( menuName = "Scriptable Objects/Quests/Track Value Quest" )]
public class TrackValueQuestSO : QuestSO {
    [field: SerializeField] public TrackedType TrackedValue { get; set; }

    public void OnValueChanged( TrackedType type, float value ) {
        if ( isCompleted || type != TrackedValue ) return;

        currAmount += value;
        Evaluate();
    }
}

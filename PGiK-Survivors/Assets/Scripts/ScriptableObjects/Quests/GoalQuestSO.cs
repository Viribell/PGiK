using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Quests/Goal Quest" )]
public class GoalQuestSO : QuestSO {

    private void Awake() {
        requiredAmount = 1;
    }

    public void OnGoalReached() {
        if ( isCompleted ) return;

        currAmount = 1;
        Evaluate();
    }
}

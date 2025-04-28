using UnityEngine;

public class KillGoal : QuestGoal {
    [field: SerializeField] public EnemyType EnemyToKill { get; set; }

    public KillGoal(string description, bool isCompleted, float currAmount, float requiredAmount, EnemyType enemyType ) {
        this.description = description;
        this.isCompleted = isCompleted;
        this.currAmount = currAmount;
        this.requiredAmount = requiredAmount;
        EnemyToKill = enemyType;
    }

    public override void Init() {
        base.Init();
        //Listener dla �mierci przeciwnika jako� zrobi�, albo jakie� og�lne albo co�...
    }

    public void OnEnemyKilled(EnemyController enemy) {
        if ( isCompleted ) return;
        
        if( enemy.EnemyData.enemyType == EnemyToKill ) {
            currAmount++;
            Evaluate();
        }
    }
}

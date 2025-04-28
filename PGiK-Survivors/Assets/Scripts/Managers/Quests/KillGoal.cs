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
        //Listener dla œmierci przeciwnika jakoœ zrobiæ, albo jakieœ ogólne albo coœ...
    }

    public void OnEnemyKilled(EnemyController enemy) {
        if ( isCompleted ) return;
        
        if( enemy.EnemyData.enemyType == EnemyToKill ) {
            currAmount++;
            Evaluate();
        }
    }
}

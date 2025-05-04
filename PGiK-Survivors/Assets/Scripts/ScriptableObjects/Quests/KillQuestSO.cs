using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Quests/Kill Quest" )]
public class KillQuestSO : QuestSO {
    [field: SerializeField] public EnemyType EnemyToKill { get; set; }

    public override void Init() {
        base.Init();
    }

    public void OnEnemyKilled( EnemyController enemy ) {
        if ( isCompleted ) return;

        if ( enemy.EnemyData.enemyType == EnemyToKill ) {
            currAmount++;
            Evaluate();
        }
    }
}

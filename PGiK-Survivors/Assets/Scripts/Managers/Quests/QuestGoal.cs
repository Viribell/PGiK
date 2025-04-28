using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestGoal {
    [field: Header( "Goal Info" )]
    [field: SerializeField] public string description;
    [field: SerializeField] public Quest quest;

    [field: Header( "Goal State" )]
    [field: SerializeField] public bool isCompleted;
    [field: SerializeField] public float currAmount;
    [field: SerializeField] public float requiredAmount;

    public virtual void Init() {

    }

    public void Evaluate() {
        if ( currAmount >= requiredAmount ) Complete();
    }

    public void Complete() {
        isCompleted = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour {
    [field: SerializeField] public string questName;
    [field: SerializeField] public string description;

    [field: SerializeField] public bool isCompleted;

    [field: SerializeField] public List<QuestGoal> questGoals = new List<QuestGoal>();

    //[field: SerializeField] co� tam jaki� reward co daje wi�c hmm...

    //testL QuestBehaviour jako mono i nigdy si� nie pojawi dop�ki quest nie zaliczony
    //albo klasa albo interfejs dla klas ktore nie sa mono ale moga byc locked, jakos trzeba to ogarnac

    public void CheckGoals() {
        isCompleted = questGoals.All( g => g.isCompleted );
        
        if ( isCompleted ) GiveReward(); //do zmiany jakos
    }


    private void GiveReward() {

    }

}

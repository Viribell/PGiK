using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Undefined,

}

[CreateAssetMenu( menuName = "Scriptable Objects/Entity/Enemy" )]
public class EnemySO : EntitySO {
    [Header( "Enemy Info" )]
    [field: SerializeField] public EnemyType enemyType;
}

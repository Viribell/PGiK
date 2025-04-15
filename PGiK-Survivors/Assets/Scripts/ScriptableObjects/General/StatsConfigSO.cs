using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Stats Config" )]
public class StatsConfigSO : ScriptableObject {
    [field: SerializeField] public List<StatType> usedStats;
}

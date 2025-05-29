using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Card Set" )]
public class CardSetSO : ScriptableObject {
    [field: SerializeField] public List<CardSO> Cards { get; private set; }
}

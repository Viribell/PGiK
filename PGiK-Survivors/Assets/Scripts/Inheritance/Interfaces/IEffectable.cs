using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable {
    public void ApplyEffect( StatusEffectSO effectData );
    public void RemoveEffect();
    public void HandleEffect();
}

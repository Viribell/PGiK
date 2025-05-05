using UnityEngine;

public abstract class Destructible : MonoBehaviour {
    public void Destroy() {
        DestructAction();
        Destroy( gameObject );
    }

    protected virtual void DestructAction() { }
}

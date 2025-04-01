using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class EventChannelListener : MonoBehaviour {
    [Header("Listener config")]
    public EventChannelSO channel;
    public bool destroyObjectOnRaise = false;
    public bool destroyListenerOnRaise = false;

    [Header("Listener actions on Channel Raise")]
    public UnityEvent respone;

    private void OnEnable() {
        channel.Register( this );
    }

    private void OnDisable() {
        channel.Unregister( this );
    }

    public void OnEventRaised() {
        respone?.Invoke();
        if ( destroyObjectOnRaise ) { Perish(); return; }
        if ( destroyListenerOnRaise ) { PerishListener(); return; }
    }

    private void Perish() {
        channel.Unregister( this );
        Destroy( gameObject );
    }

    private void PerishListener() {
        channel.Unregister( this );
        Destroy( this );
    }
}

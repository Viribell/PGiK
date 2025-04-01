using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/Events/EventChannel" )]
public class EventChannelSO : ScriptableObject {
    private List<EventChannelListener> listeners = new List<EventChannelListener>();

    public void Register(EventChannelListener listener) { listeners.Add( listener ); }
    public void Unregister(EventChannelListener listener) { listeners.Remove( listener ); }

    public void Raise() {
        if ( listeners == null || listeners.Count == 0 ) return;

        for(int i = listeners.Count - 1; i >= 0; i-- ) {
            listeners[i].OnEventRaised();
        }
    }
}

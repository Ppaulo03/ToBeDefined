using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    [SerializeField] protected Signal signal = null;
    [SerializeField] protected UnityEvent signalEvent = null;

    public virtual void onSignalRaised() => signalEvent.Invoke();
    private void OnEnable() => signal.RegisterListener(this);
    private void OnDisable() => signal.DeRegisterListener(this);
    
}

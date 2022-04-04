﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/Signal", order = 0)]
public class Signal : ScriptableObject
{

    private List<SignalListener> listeners = new List<SignalListener>();
    
    public void Raise()
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].onSignalRaised();  
    }

    public void RegisterListener(SignalListener listener) => listeners.Add(listener);
    public void DeRegisterListener(SignalListener listener) => listeners.Remove(listener);
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{

    public static UnityEvent onPlayerEliminated = new UnityEvent();
    public static UnityEvent onPlayerDamaged = new UnityEvent();
}

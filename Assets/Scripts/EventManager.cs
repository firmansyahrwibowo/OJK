using System;
using System.Collections.Generic;

public class GameEvent { }

public class EventManager
{
    public delegate void DelegateEvent<T>(T evt);
    private static Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();

    /// <summary>
    /// Assign/add delegate function into the dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    public static void AddListener<T>(DelegateEvent<T> del) where T : GameEvent
    {
        Delegate tempDelegate;
        if (delegates.TryGetValue(typeof(T), out tempDelegate))
        {
            DelegateEvent<T> method = tempDelegate as DelegateEvent<T>;
            delegates[typeof(T)] = method += del;
        }
        else
            delegates[typeof(T)] = del;
    }

    /// <summary>
    /// Call delegate function accordingly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="evt"></param>
    public static void TriggerEvent<T>(T evt)
    {
        Delegate tempDelegate;
        if (delegates.TryGetValue(typeof(T), out tempDelegate))
        {
            DelegateEvent<T> del = tempDelegate as DelegateEvent<T>;
            del(evt);
        }
    }

    /// <summary>
    /// Remove delegate function from the dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    public static void RemoveListener<T>(DelegateEvent<T> del) where T : GameEvent
    {
        Delegate tempDelegate;
        if (delegates.TryGetValue(typeof(T), out tempDelegate))
        {
            DelegateEvent<T> method = tempDelegate as DelegateEvent<T>;
            method -= del;

            if (method != null)
                delegates[typeof(T)] = method;
            else
                delegates.Remove(typeof(T));
        }
    }

    public static void RemoveAllListeners()
    {
        delegates.Clear();
    }
}

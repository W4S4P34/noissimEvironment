using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PlayerCombatEvent
{
    HealEvent
}
public enum GameDungeonEvent
{
    StartDungeon,
    Pause,
    EndGame,
}

public class UnityParamEvent : UnityEvent<object[], Action> { }
public static class ActionEventHandler
{
    private static Dictionary<Enum, UnityEvent> listActionEvent = new Dictionary<Enum, UnityEvent>();
    private static Dictionary<Enum, UnityParamEvent> listParamActionEvent = new Dictionary<Enum, UnityParamEvent>();
    public static void AddNewActionEvent(Enum eventID,UnityAction callback)
    {
        UnityEvent actionEvent;
        if(listActionEvent.TryGetValue(eventID,out actionEvent))
        {
            actionEvent.AddListener(callback);
        }
        else
        {
            actionEvent = new UnityEvent();
            actionEvent.AddListener(callback);
            listActionEvent.Add(eventID, actionEvent);
        }
    }

    public static void AddNewActionEvent(Enum eventID, UnityAction<object[], Action> callback)
    {
        UnityParamEvent actionEvent;
        if (listParamActionEvent.TryGetValue(eventID, out actionEvent))
        {
            actionEvent.AddListener(callback);
        }
        else
        {
            actionEvent = new UnityParamEvent();
            actionEvent.AddListener(callback);
            listParamActionEvent.Add(eventID, actionEvent);
        }
    }

    public static void Invoke(Enum eventID)
    {
        try
        {
            listActionEvent[eventID].Invoke();
        }
        catch (Exception exc)
        {
            Debug.LogError(eventID);
            Debug.LogError("Error: " + exc.Message);
        }
    }

    public static void Invoke(Enum eventID, object[] param, Action onActionComplete)
    {
        try
        {
            listParamActionEvent[eventID].Invoke(param, onActionComplete);
        }
        catch (Exception exc)
        {
            Debug.LogError(eventID);
            Debug.LogError("Error: " + exc.Message);
        }
    }

    public static bool IsEventExist(Enum eventID)
    {
        return listActionEvent.ContainsKey(eventID);
    }
    public static void RemoveAction(Enum eventID)
    {
        try
        {
            listActionEvent[eventID].RemoveAllListeners();
        }
        catch (Exception exc)
        {
            Debug.LogError("Error: " + exc.Message);
        }
    }
    public static void RemoveAllAction()
    {
        listActionEvent.Clear();
        listParamActionEvent.Clear();
    }
}

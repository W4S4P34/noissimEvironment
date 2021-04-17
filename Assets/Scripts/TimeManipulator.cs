using System;
using System.Collections;
using UnityEngine;

public class TimeManipulator : MonoBehaviour
{
    [SerializeField]
    private static TimeManipulator instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    public static TimeManipulator GetInstance()
    {
        return instance;
    }

    #region Public Methods
    public void InvokeRepeatAction(float seconds, Action action, Action onCompleteAction)
    {
        StartCoroutine(InvokeRepeatAction_IEnumerator(seconds, action, onCompleteAction));
    }
    public void InvokeActionAfterSeconds(float seconds, Action action)
    {
        StartCoroutine(InvokeActionAfterSeconds_IEnumerator(seconds, action));
    }
    public void InvokeActionWithPromise(float seconds, Action action, Action onCompleteAction)
    {
        StartCoroutine(InvokeActionWithPromise_IEnumerator(seconds, action, onCompleteAction));
    }
    #endregion


    #region Coroutine Methods
    IEnumerator InvokeActionAfterSeconds_IEnumerator(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    IEnumerator InvokeRepeatAction_IEnumerator(float seconds, Action action, Action onCompleteAction)
    {
        yield return new WaitForSeconds(1f);
        seconds -= 1;
        if(seconds <= 0)
        {
            onCompleteAction?.Invoke();
        }
        else
        {
            action?.Invoke();
        }
    }
    IEnumerator InvokeActionWithPromise_IEnumerator(float seconds, Action action, Action onCompleteAction)
    {
        action?.Invoke();
        yield return new WaitForSeconds(seconds);
        onCompleteAction?.Invoke();
    }
    #endregion
}

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
    public void InvokeRepeatAction(float delayTime, int times, Action action, Action onCompleteAction)
    {
        StartCoroutine(InvokeRepeatAction_IEnumerator(delayTime, times, action, onCompleteAction));
    }
    public void InvokeRepeatActionUntil(Func<bool> action, Action onCompleteAction)
    {
        StartCoroutine(InvokeRepeatActionUntil_IEnumerator(action, onCompleteAction));
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
    IEnumerator InvokeRepeatAction_IEnumerator(float seconds, Action action, Action onCompleteAction)
    {
        yield return new WaitForSeconds(1f);
        if (seconds <= 0)
        {
            onCompleteAction?.Invoke();
        }
        else
        {
            action?.Invoke();
            StartCoroutine(InvokeRepeatAction_IEnumerator(seconds-1, action, onCompleteAction));
        }
    }
    IEnumerator InvokeRepeatAction_IEnumerator(float delayTime, int times, Action action, Action onCompleteAction)
    {
        action?.Invoke();
        times--;
        yield return new WaitForSeconds(delayTime);
        if (times > 0)
            StartCoroutine(InvokeRepeatAction_IEnumerator(delayTime, times, action, onCompleteAction));
        else
            onCompleteAction?.Invoke();

    }
    IEnumerator InvokeRepeatActionUntil_IEnumerator(Func<bool> action, Action onCompleteAction)
    {
        yield return new WaitUntil(action);
        onCompleteAction?.Invoke();
    }
    IEnumerator InvokeActionAfterSeconds_IEnumerator(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    IEnumerator InvokeActionWithPromise_IEnumerator(float seconds, Action action, Action onCompleteAction)
    {
        action?.Invoke();
        yield return new WaitForSeconds(seconds);
        onCompleteAction?.Invoke();
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private static Dictionary<string, Coroutine> _activeTimers = new Dictionary<string, Coroutine>();

    #region Timer Helper
    private class TimerHelper : MonoBehaviour { }
    private static TimerHelper _helper;
    private static void EnsureHelperExists()
    {
        if (_helper == null)
        {
            GameObject obj = new GameObject("GlobalTimerHelper");
            _helper = obj.AddComponent<TimerHelper>();
            DontDestroyOnLoad(obj);
        }
    }
    #endregion

    #region Timer with parameter

    #region Summary
    /// <summary>
    /// Starts a new timer with the specified duration and invokes the provided callback function with a parameter when the timer completes.
    /// </summary>
    /// <typeparam name="T">The type of the parameter passed to the callback function.</typeparam>
    /// <param name="duration">The duration of the timer in seconds. Must be a positive value.</param>
    /// <param name="callback">The callback function to be invoked after the timer completes. Cannot be null.</param>
    /// <param name="parameter">The parameter to pass to the callback function.</param>
    /// <returns>Returns a unique string identifier for the timer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the duration is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the callback function is null.</exception>
    #endregion
    public static string StartTimer<T>(float duration, Action<T> callback, T parameter) //call the timer with a parameter function
    {
        if (duration < 0) throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive.");
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        EnsureHelperExists();
        string timerId = Guid.NewGuid().ToString(); //Randomize a string ID
        Coroutine routine = _helper.StartCoroutine(TimerCoroutineWithParam(duration, callback, parameter, timerId));
        _activeTimers[timerId] = routine; //Add this timer to the activeTimer dictionary
        return timerId;
    }
    private static IEnumerator TimerCoroutineWithParam<T>(float duration, Action<T> callback, T parameter, string timerId)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke(parameter);
        _activeTimers.Remove(timerId);
    }
    #endregion

    #region Timer without parameter

    #region Summary
    /// <summary>
    /// Starts a new timer that executes a callback function after a specified duration.
    /// </summary>
    /// <param name="duration">The duration in seconds before the callback is triggered.</param>
    /// <param name="callback">The function to execute when the timer completes.</param>
    /// <returns>A unique string identifier for the timer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the duration is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the callback function is null.</exception>
    #endregion
    public static string StartTimer(float duration, Action callback) //call the timer without a parameter function
    {
        if (duration < 0) throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive.");
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        EnsureHelperExists();
        string timerId = Guid.NewGuid().ToString(); //Randomize a string ID
        Coroutine routine = _helper.StartCoroutine(TimerCoroutineWithoutParam(duration, callback, timerId));

        _activeTimers[timerId] = routine; //Add this timer to the activeTimer dictionary
        return timerId;
    }
    private static IEnumerator TimerCoroutineWithoutParam(float duration, Action callback, string timerId)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
        _activeTimers.Remove(timerId);
    }
    #endregion

    #region Cancel Timer
    public static void CancelTimer(string timerId) //Cancel Timer with ID 
    {
        if (_activeTimers.TryGetValue(timerId, out Coroutine coroutine))
        {
            _helper.StopCoroutine(coroutine);
            _activeTimers.Remove(timerId);
        }
        else
        {
            Debug.LogWarning($"Timer with ID {timerId} not found.");
        }
    }
    public static void CancelAllTimer() //Cancel all Active Timer 
    {
        foreach (var timer in _activeTimers.Values)
        {
            _helper.StopCoroutine(timer);
        }
        _activeTimers.Clear();
    }
    #endregion
}

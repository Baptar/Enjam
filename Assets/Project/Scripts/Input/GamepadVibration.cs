using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibration : MonoBehaviour
{
    private static GamepadVibration _instance;
    private Coroutine _vibrationCoroutine;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public static void Vibrate(float low, float high, float duration)
    {
        if (Gamepad.current == null || _instance == null)
            return;

        if (_instance._vibrationCoroutine != null)
            _instance.StopCoroutine(_instance._vibrationCoroutine);

        _instance._vibrationCoroutine = _instance.StartCoroutine(
            _instance.VibrationRoutine(low, high, duration)
        );
    }

    private IEnumerator VibrationRoutine(float low, float high, float duration)
    {
        Gamepad.current.SetMotorSpeeds(low, high);

        yield return new WaitForSeconds(duration);

        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
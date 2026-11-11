using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class KnockMarkerRelay : MonoBehaviour
{
    public static event Action<Transform, string> OnMarker;

    class Context { public Transform transform; }
    struct Pending { public Transform transform; public string markerName; }

    static readonly ConcurrentQueue<Pending> queue = new ConcurrentQueue<Pending>();
    static readonly EVENT_CALLBACK callback = new EVENT_CALLBACK(OnFmodEvent);
    static KnockMarkerRelay instance;

    public static void Play(string path, Transform soundTransform)
    {
        EnsureExists();

        var inst = RuntimeManager.CreateInstance(path);
        if (soundTransform != null)
            inst.set3DAttributes(RuntimeUtils.To3DAttributes(soundTransform));

        var handle = GCHandle.Alloc(new Context { transform = soundTransform });
        inst.setUserData(GCHandle.ToIntPtr(handle));
        inst.setCallback(callback,
            EVENT_CALLBACK_TYPE.TIMELINE_MARKER | EVENT_CALLBACK_TYPE.DESTROYED);

        inst.start();
        inst.release();
    }

    static void EnsureExists()
    {
        if (instance != null) return;
        var go = new GameObject("KnockMarkerRelay");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<KnockMarkerRelay>();
    }

    void Update()
    {
        // On revient sur le thread principal
        while (queue.TryDequeue(out Pending p))
            OnMarker?.Invoke(p.transform, p.markerName);
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT OnFmodEvent(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameters)
    {
        var inst = new EventInstance(instancePtr);
        inst.getUserData(out IntPtr userData);
        if (userData == IntPtr.Zero) return FMOD.RESULT.OK;

        var handle = GCHandle.FromIntPtr(userData);

        if (type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            var props = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(
                parameters, typeof(TIMELINE_MARKER_PROPERTIES));
            var ctx = (Context)handle.Target;
            queue.Enqueue(new Pending { transform = ctx.transform, markerName = props.name });
        }
        else if (type == EVENT_CALLBACK_TYPE.DESTROYED)
        {
            handle.Free();
        }
        return FMOD.RESULT.OK;
    }
}
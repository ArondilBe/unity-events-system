using System;
using System.Collections.Generic;

namespace Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> listeners = new();
        private static readonly object synchronizer = new();

        public static void Subscribe<T>(Action<T> gameEvent)
            where T : IGameEvent
        {
            var type = typeof(T);
            lock (synchronizer)
            {
                if (!listeners.TryGetValue(type, out var callbacks))
                {
                    callbacks = new List<Delegate>();
                    listeners[type] = callbacks;
                }
                callbacks.Add(gameEvent);
            }
        }

        public static void Unsubscribe<T>(Action<T> gameEvent)
            where T : IGameEvent
        {
            var type = typeof(T);
            lock (synchronizer)
            {
                if (listeners.TryGetValue(type, out var callbacks))
                {
                    callbacks.Remove(gameEvent);
                    if (callbacks.Count == 0)
                    {
                        listeners.Remove(type);
                    }
                }
            }
        }

        public static void Publish<T>(T gameEvent)
            where T : IGameEvent
        {
            var type = typeof(T);
            Delegate[] callbacksSnapshot;
            lock (synchronizer)
            {
                if (!listeners.TryGetValue(type, out var callbacks) || callbacks.Count == 0)
                {
                    return;
                }
                callbacksSnapshot = callbacks.ToArray();
            }

            for (var i = 0; i < callbacksSnapshot.Length; i++)
            {
                if (callbacksSnapshot[i] is Action<T> action)
                {
                    action.Invoke(gameEvent);
                }
            }
        }
    }
}

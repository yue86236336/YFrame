using System;
using System.Collections.Generic;

namespace YF
{
    public enum MessageType
    {
        //在这里添加消息类型
        sceneLoadProcess = 1
    }

    public class EventCenter : MonoSingleton<EventCenter>
    {
        private readonly Dictionary<int, List<Delegate>> dic = new Dictionary<int, List<Delegate>>();

        private void SubscribeInternal(int eventId, Delegate callback)
        {
            if (!dic.ContainsKey(eventId))
            {
                dic.Add(eventId, new List<Delegate>());
            }
            dic[eventId].Add(callback);
        }

        private bool UnsubscribeInternal(int eventId, Delegate callback)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                list.Remove(callback);
                if (list.Count == 0)
                {
                    dic.Remove(eventId);
                }
                return true;
            }
            return false;
        }

        public void Publish(int eventId)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action action)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        public void Publish<T>(int eventId, T arg)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T> action)
                    {
                        action.Invoke(arg);
                    }
                }
            }
        }

        public void Publish<T1, T2>(int eventId, T1 arg1, T2 arg2)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2> action)
                    {
                        action.Invoke(arg1, arg2);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3>(int eventId, T1 arg1, T2 arg2, T3 arg3)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3> action)
                    {
                        action.Invoke(arg1, arg2, arg3);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }

        public void Subscribe(int eventId, Action callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T>(int eventId, Action<T> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2>(int eventId, Action<T1, T2> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        //移除所有订阅
        public bool UnsubscribeAll(int eventId)
        {
            if (dic.ContainsKey(eventId))
            {
                dic.Remove(eventId);
                return true;
            }
            return false;
        }

        public bool Unsubscribe(int eventId, Action callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T>(int eventId, Action<T> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2>(int eventId, Action<T1, T2> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

    }

}

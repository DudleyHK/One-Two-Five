using System;
using System.Collections.Generic;
using System.Reflection;


public static class EventManager
{
    public struct Events
    {
        public static string Collision = "event_collision";
        public static string RegisterObject = "event_reg_object";
    }


    private static Dictionary<string, Delegate> EventList = new Dictionary<string, Delegate>
    {
        { Events.Collision, null },
        { Events.RegisterObject, null }
    };


    public static void AddListener(string _event, Delegate _value)
    {
        Delegate item;
        if (EventList.TryGetValue(_event, out item))
        {
            EventList[_event] = _value;
        }
    }


    public static void RemoveListener(string _event, Delegate _value)
    {
        Delegate item;
        if (EventList.TryGetValue(_event, out item))
        {
            EventList[_event] = null;
        }
    }


    public static bool Trigger(string _event, object _obj)
    {
        Delegate item;

        if (EventList.TryGetValue(_event, out item))
        {
            var methodInfo = item.Method;
            var target = item.Target;
            var parameters = methodInfo.GetParameters();

            var type = _obj.GetType();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var name = p.Name;

                var property = type.GetProperty(name);
                if (property == null) return false;

                var value = property.GetValue(_obj, null);
                args[i] = value;
            }

            if (methodInfo.Equals(null)) return false;
            methodInfo.Invoke(target, args);

            return true;
        }
        return false;
    }
}
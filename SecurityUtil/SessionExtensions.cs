using System.Collections.Generic;
using System.Web;

namespace SecurityUtil
{
    public static class SessionExtensions
    {
        public static void DestroySessions()
        {
            foreach (var session in _sessions)
            {
                session.Abandon();
                session.Clear();
            }
        }

        public static bool Exists(this HttpSessionStateBase session, string sessionName)
        {
            if (session == null || session.Keys.Count == 0)
            {
                return false;
            }
            return session[sessionName] != null;
        }

        public static T GetData<T>(this HttpSessionStateBase session, string key)
        {
            if (session.Exists(key))
            {
                return (T)session[key];
            }
            return default(T);
        }

        public static void SetData<T>(this HttpSessionStateBase session, string key, T value)
        {
            session[key] = value;
            _sessions.Add(session);
        }

        private static List<HttpSessionStateBase> _sessions = new List<HttpSessionStateBase>();

        private static bool IsNull(this HttpSessionStateBase session, string sessionName)
        {
            return session[sessionName] == null;
        }
    }
}
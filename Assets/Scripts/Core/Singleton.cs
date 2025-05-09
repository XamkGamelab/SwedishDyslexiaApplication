namespace SwedishApp.Core
{
    public abstract class Singleton<T> where T : class, new()
    {
#nullable enable
        private static T? _instance = null;
#nullable restore
        private static object _lock = new();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new T();
                    }
                }
                return _instance;
            }
        }
    }
}
namespace SwedishApp.Core
{
    /// <summary>
    /// This class can be used to create a singleton out of a class with no other inheritance
    /// </summary>
    /// <typeparam name="T">This is set to be whatever class the singleton is made out of.</typeparam>
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
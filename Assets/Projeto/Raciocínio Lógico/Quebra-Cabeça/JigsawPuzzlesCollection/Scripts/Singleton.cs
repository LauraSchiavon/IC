using UnityEngine;

namespace JigsawPuzzlesCollection.Scripts
{
    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        #region  Fields
        private static T s_instance;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object s_lock = new object();

        [SerializeField]
        protected bool m_persistent = true;
        #endregion

        #region  Properties
        public static T Instance
        {
            get
            {
                if (Quitting)
                {
                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return null;
                }
                lock (s_lock)
                {
                    if (s_instance != null)
                        return s_instance;
                    var instances = FindObjectsOfType<T>();
                    var count = instances.Length;
                    if (count > 0)
                    {
                        if (count == 1)
                            return s_instance = instances[0];
                        Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                        for (var i = 1; i < instances.Length; i++)
                            Destroy(instances[i]);
                        return s_instance = instances[0];
                    }

                    Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                    return s_instance = new GameObject($"{typeof(T).Name}").AddComponent<T>();
                }
            }
        }
        #endregion

        #region  Methods
        private void Awake()
        {
            if (m_persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
            OnAwake();
        }

        protected virtual void OnAwake() { }
        #endregion
    }

    public abstract class Singleton : MonoBehaviour
    {
        #region  Properties
        public static bool Quitting { get; private set; }
        #endregion

        #region  Methods
        private void OnApplicationQuit()
        {
            Quitting = true;
        }
        #endregion
    }
}
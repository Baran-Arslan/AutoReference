using iCare.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace iCare.AutoReference {
    public interface IListenAwake {
        void OnGameAwake();
    }

    public interface IListenStart {
        void OnGameStart();
    }

    public interface IListenUpdate {
        void OnGameUpdate();
    }

    public interface IListenFixedUpdate {
        void OnGameFixedUpdate();
    }

    public interface IListenLateUpdate {
        void OnGameLateUpdate();
    }

    public interface IListenOnDestroy {
        void OnGameDestroy();
    }

    public interface IListenSceneChange {
        void OnGameSceneChange(Scene scene, LoadSceneMode loadMode);
    }


    internal sealed class LoopManager : SerializedMonoBehaviour {
        [SerializeField] [AutoList] [CanStayNull]
        IListenAwake[] awakeListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenStart[] startListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenUpdate[] updateListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenFixedUpdate[] fixedUpdateListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenLateUpdate[] lateUpdateListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenOnDestroy[] destroyListeners;

        [SerializeField] [AutoList] [CanStayNull]
        IListenSceneChange[] sceneChangeListeners;

        void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this);

            if (awakeListeners.IsNullOrEmpty()) return;
            foreach (var listener in awakeListeners) listener.OnGameAwake();
        }

        void Start() {
            if (startListeners.IsNullOrEmpty()) return;
            foreach (var listener in startListeners) listener.OnGameStart();
        }

        void Update() {
            if (updateListeners.IsNullOrEmpty()) return;
            foreach (var listener in updateListeners) listener.OnGameUpdate();
        }

        void FixedUpdate() {
            if (fixedUpdateListeners.IsNullOrEmpty()) return;
            foreach (var listener in fixedUpdateListeners) listener.OnGameFixedUpdate();
        }

        void LateUpdate() {
            if (lateUpdateListeners.IsNullOrEmpty()) return;
            foreach (var listener in lateUpdateListeners) listener.OnGameLateUpdate();
        }

        void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (destroyListeners.IsNullOrEmpty()) return;
            foreach (var listener in destroyListeners) listener.OnGameDestroy();
        }

        void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            if (sceneChangeListeners.IsNullOrEmpty()) return;
            foreach (var listener in sceneChangeListeners) listener.OnGameSceneChange(arg0, arg1);
        }
    }
}
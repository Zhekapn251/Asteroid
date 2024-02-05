using System;
using System.Collections;
using UnityEngine;

namespace Misc
{
    public class CoroutineService : MonoBehaviour
    {
        public static CoroutineService Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Coroutine StartGameCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StopGameCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        public Coroutine ExecuteAfterDelay(float delay, Action action)
        {
            return StartCoroutine(ExecuteAfterDelayRoutine(delay, action));
        }

        private IEnumerator ExecuteAfterDelayRoutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
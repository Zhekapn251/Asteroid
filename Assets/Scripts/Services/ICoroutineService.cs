using System;
using System.Collections;
using UnityEngine;

namespace Services
{
    internal interface ICoroutineService
    {
        Coroutine ExecuteAfterDelay(float maxLifetime, Action returnToPool);
        Coroutine StartGameCoroutine(IEnumerator coroutine);
    }
}
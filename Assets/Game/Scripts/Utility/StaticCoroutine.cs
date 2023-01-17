using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    internal class StaticCoroutineRunner : MonoBehaviour
    {
    }

    public class StaticCoroutine
    {
        private static StaticCoroutineRunner runner;

        public static Coroutine Start(IEnumerator coroutine)
        {
            EnsureRunner();
            return runner.StartCoroutine(coroutine);
        }

        private static void EnsureRunner()
        {
            if (runner == null)
            {
                runner = new GameObject("[Static Coroutine Runner]").AddComponent<StaticCoroutineRunner>();
                Object.DontDestroyOnLoad(runner.gameObject);
            }
        }

    }
}

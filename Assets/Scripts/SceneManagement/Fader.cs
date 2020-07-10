using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currenActiveFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currenActiveFade != null)
            {
                StopCoroutine(currenActiveFade);
            }
            currenActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currenActiveFade;
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
    }
}
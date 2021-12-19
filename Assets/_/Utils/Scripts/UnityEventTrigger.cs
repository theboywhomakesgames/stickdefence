using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PT.Utils
{
    public class UnityEventTrigger : MonoBehaviour
    {
        public UnityEvent e;

        public void Trigger()
        {
            e?.Invoke();
        }

        public void TriggerWithDelay(float delay)
        {
            TimeManager.Instance.DoWithDelay(delay, () =>
            {
                e?.Invoke();
            });
        }
    }
}
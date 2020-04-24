using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        private void LateUpdate()
        {
            if (target)
            {
                transform.position = target.position;
            }
            else
            {
                Debug.LogWarning("FOLLOWCAMERA UPDATE: no target!");
            }
        }
    }
}
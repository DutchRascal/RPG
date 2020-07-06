namespace RPG.Core
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CameraFacing : MonoBehaviour
    {
        Text text;

        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}

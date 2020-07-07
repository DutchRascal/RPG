namespace RPG.UI.DamageText
{
    using UnityEngine;

    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject targettoDestroy = null;

        public void DestroyTarget()
        {
            Destroy(targettoDestroy);
        }
    }
}
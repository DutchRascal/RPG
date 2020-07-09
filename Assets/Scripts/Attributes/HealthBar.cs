namespace RPG.Attributes
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground=null;

         private void Update()
        {
            float healthFraction = healthComponent.GetFraction();
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
            if (healthFraction <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
namespace RPG.Attributes
{
    using UnityEngine;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        private void Update()
        {
            rootCanvas.gameObject.SetActive(true);
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
            if (Mathf.Approximately(healthComponent.GetFraction() ,1) || 
                Mathf.Approximately(healthComponent.GetFraction(), 0))
            {
                rootCanvas.gameObject.SetActive(false);
            }
        }
    }
}
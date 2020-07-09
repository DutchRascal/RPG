using RPG.Attributes;
using RPG.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            string textValue = "N/A";
            Health health = fighter.GetTarget();
            if (health)
            {
                textValue = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
            GetComponent<Text>().text = textValue;
        }
    }
}
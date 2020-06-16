using RPG.Resources;
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
            Health target = fighter.GetTarget();
            if (target)
            {
                textValue = string.Format("{0:0}%", target.GetPercentage());
            }
            GetComponent<Text>().text = textValue;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            Vector3 spawnPositionCorrection;
            spawnPositionCorrection = new Vector3(0, -1, 0);
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            instance.transform.position += spawnPositionCorrection;
            instance.SetValue(damageAmount);
        }
    }
}
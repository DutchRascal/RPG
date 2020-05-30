using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] Transform target = null;

        private void Update()
        {
            if (!target) { return; }
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (!targetCapsule)
            {
                return target.position;
            }
            return target.position + Vector3.up * (targetCapsule.height + 0.5f) / 2;
        }
    }
}
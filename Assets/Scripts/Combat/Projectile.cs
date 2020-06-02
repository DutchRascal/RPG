using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;

        Health target = null;
        float damage = 0;

        private void Update()
        {
            if (!target) { return; }
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (!targetCapsule)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (targetCapsule.height + 0.5f) / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            print($"OnTriggerEnter - {other.name} - {target}");
            if (other.GetComponent<Health>() != target) { return; }
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
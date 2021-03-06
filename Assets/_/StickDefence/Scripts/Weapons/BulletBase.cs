using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Weapons
{
    public class BulletBase : MonoBehaviour
    {
        public UnityEvent OnContact, OnGetShot;
        public int damage = 50;

        public void GetShot(Vector3 dir, Transform tar)
        {
            target = tar;
            hasTarget = true;
            GetShot(dir);
        }

        public void GetShot(Vector3 dir)
        {
            if(hasLR)
                lr.Clear();
            rb.velocity = dir.normalized * speed;
            transform.up = dir.normalized;
            gotShot = true;
            OnGetShot?.Invoke();
        }

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed, impactRange = 0;
        [SerializeField] private LayerMask impactLayerMask;
        [SerializeField] private bool drops = false;
        [SerializeField] private bool hasLR = false;
        [SerializeField] private TrailRenderer lr; 

        private Transform target;
        private bool hasTarget = false;
        private bool gotShot = false;

        private void Update()
        {
            if (gotShot && !drops)
            {
                Vector3 dir = rb.velocity.normalized;
                if (hasTarget)
                {
                    if(target == null)
                    {
                        hasTarget = false;
                    }

                    dir = target.position - transform.position;
                    dir = dir.normalized;
                }

                rb.velocity = dir * speed;
            }

            transform.up = rb.velocity.normalized;
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnContact?.Invoke();

            if(impactRange > 0)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, impactRange, impactLayerMask);
                foreach(Collider collider in colliders)
                {
                    if (collider.isTrigger)
                        continue;

                    Damager d = collider.GetComponent<Damager>();
                    if(d != null)
                    {
                        d.Damage(damage);
                    }
                }
            }

            gameObject.SetActive(false);
        }
    }
}
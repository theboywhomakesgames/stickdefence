using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DB.War.Stickman;
using DB.Utils;

namespace DB.War.Weapons
{
    public class AutoShooter : MonoBehaviour
    {
        public void Enter(Collider other)
        {
            if (other.isTrigger)
                return;

            targets.Add(other.transform);

            Stickman.Stickman stick = other.gameObject.GetComponent<Stickman.Stickman>();
            if(stick != null)
            {
                stick.OnKilledByGun += OneGotKilled;
            }
        }

        public void OneGotKilled(Stickman.Stickman stick)
        {
            print("killed");
            targets.Remove(stick.transform);
            stick.OnKilledByGun -= OneGotKilled;
        }

        public void Exit(Collider other)
        {
            if (other.isTrigger)
                return;

            Stickman.Stickman stick = other.gameObject.GetComponent<Stickman.Stickman>();
            if (stick != null)
            {
                targets.Remove(other.transform);
                stick.OnKilledByGun -= OneGotKilled;
            }
        }

        [SerializeField] private List<Transform> targets;
        [SerializeField] private GunBase gun;
        [SerializeField] private Transform turret, aimT;
        [SerializeField] private bool turretRotation, resetTurret;
        [SerializeField] private BoolCondition hasTargetCondition;
        [SerializeField] private int forwMultiplier = 1;

        private Quaternion defTurretRot;

        private void Awake()
        {
            defTurretRot = turret.localRotation;
        }

        private void Update()
        {
            if (targets.Count > 0)
            {
                hasTargetCondition.value = true;
                if (turretRotation)
                {
                    Vector3 forw = forwMultiplier * (targets[0].position - turret.position);
                    forw.y = 0;
                    turret.forward = forw;
                }

                aimT.position = targets[0].position;
                gun.Shoot(targets[0]);
            }
            else
            {
                hasTargetCondition.value = false;
                if (turretRotation && resetTurret)
                {
                    turret.localRotation = Quaternion.Slerp(
                        turret.localRotation, defTurretRot, Time.deltaTime
                    );
                }
            }
        }
    }
}
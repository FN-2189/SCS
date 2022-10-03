using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [CreateAssetMenu(fileName = "New Cannon", menuName = "Cannon")]
    public class Cannon : ScriptableObject
    {
        [Tooltip("In °/s")] public Vector2 traverseSpeed;
        public bool CanRotateAround;
        [Tooltip("In °")] public float MaxTraverseLeft, MaxTraverseRight, MaxTraverseUp, MaxTraverseDown;

        public GameObject Turret, BarrelMount, Barrel;
        public Vector3 BarrelMountOffset, BarrelOffset;
        public float BulletSpawnOffset;

        [Tooltip("In Rounds/min")]public float FireRate;
        public GameObject BulletType;
        [Tooltip("In m/s")] public float MuzzleVelocity;
    }
}
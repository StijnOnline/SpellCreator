using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator
{
    [CreateAssetMenu(fileName = "ExplosionForce_Action", menuName = "Actions/ExplosionForce_Action", order = 1)]
    public class ExplosionForce_Action : Action
    {
        public Vector3 explosionOffset;
        public float explosionRange;
        public float explosionForce;
        public float upwardModifier;
        public LayerMask layermask;

        public override void Act(GameObject g)
        {
            Collider[] colliders = Physics.OverlapSphere(g.transform.position + explosionOffset,explosionRange, layermask);

            

            foreach (Collider c in colliders)
            {
                if (c.gameObject != g)
                {
                    Rigidbody r = c.attachedRigidbody;
                    r?.AddExplosionForce(explosionForce, g.transform.position + explosionOffset, explosionRange, upwardModifier);
                }
            }
        }
    }
}
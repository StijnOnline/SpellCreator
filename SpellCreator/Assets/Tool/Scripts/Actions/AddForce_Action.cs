using SpellCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator
{

    [CreateAssetMenu(fileName = "AddForce_Action", menuName = "Actions/AddForce_Action", order = 1)]
    public class AddForce_Action : Action
    {
        public Vector3 direction;
        public bool relative = false;

        public override void Act(GameObject g)
        {
            Rigidbody r =  g.GetComponent<Rigidbody>();
            if (r == null)
            {
                Debug.LogWarning("AddForce Action: Gameobject does not have a rigidbody");
                return;
            }
            Vector3 d = direction;
            if (relative)
                d = g.transform.TransformDirection(direction);
            r.AddForce(d);
        }
    }
}
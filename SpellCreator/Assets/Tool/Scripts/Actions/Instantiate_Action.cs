using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator
{

    [CreateAssetMenu(fileName = "Instantiate_Action", menuName = "Actions/Instantiate_Action", order = 1)]
    public class Instantiate_Action : Action
    {

        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotation;
        public bool relative = false;
        public bool delete = false;
        public float deleteDelay = 1;

        public override void Act(GameObject g)
        {
            Vector3 pos = position;
            if (relative)
                pos = g.transform.position + position;
            GameObject go = GameObject.Instantiate(prefab,pos, Quaternion.Euler(rotation));
            if(delete)
                Destroy(go, deleteDelay);
        }
    }
}
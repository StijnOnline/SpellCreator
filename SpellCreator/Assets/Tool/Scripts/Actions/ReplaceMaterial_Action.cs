using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator
{

    //This Action is hard coded in the event to wait in the coroutine
    [CreateAssetMenu(fileName = "ReplaceMaterial_Action", menuName = "Actions/ReplaceMaterial_Action", order = 1)]
    public class ReplaceMaterial_Action : Action
    {
        public Material material;

        public override void Act(GameObject g)
        {
            Renderer r = g.GetComponent<Renderer>();
            if (r == null) { Debug.LogWarning("Object doesn't have a Renderer");return; }
            r.material = material;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    //This Action is hard coded in the event to wait in the coroutine
    [CreateAssetMenu(fileName = "Wait_Action", menuName = "Actions/Wait_Action", order = 1)]
    public class Wait_Action : Action {
        
        public float waitTime = 1f;

        public override void Act(GameObject g) {
            
        }
    }
}
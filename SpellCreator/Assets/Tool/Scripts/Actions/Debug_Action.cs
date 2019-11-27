using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    [CreateAssetMenu(fileName = "Debug_Action", menuName = "Actions/Debug_Action", order = 1)]
    public class Debug_Action : Action {

        public bool warn = false;
        public string debugText = "Some Debug Text";
        public float somenumber = 69.420f;

        
        public override void Act(GameObject g) {
            string text = "Debug Action: " + debugText + somenumber;
            if(warn)
                Debug.LogWarning(text);
            else
                Debug.Log(text);
            

        }

        
    }


}
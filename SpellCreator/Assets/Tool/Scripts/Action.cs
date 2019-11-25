using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    [System.Serializable]
    public class Action : ScriptableObject{
        //TODO Modifier List

        public virtual void Act(GameObject g) {
            Debug.Log("Base Action triggered from " + g);
        }
    }
}
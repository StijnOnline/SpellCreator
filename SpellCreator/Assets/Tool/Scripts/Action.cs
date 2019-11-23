using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    [System.Serializable]
    public class Action : ScriptableObject{
        //TODO Modifier List
        public virtual void Act() {
            Debug.Log("Base Action triggered");
        }
    }
}
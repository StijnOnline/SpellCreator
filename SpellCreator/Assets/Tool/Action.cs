using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    [System.Serializable]
    public abstract class Action {
        public abstract void Act();
    }
}
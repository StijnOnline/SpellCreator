using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    public abstract class Action : ScriptableObject{
        public abstract void Act();
    }
}
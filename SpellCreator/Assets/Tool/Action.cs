using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {

    public abstract class Action {

        //I dont know how i should make modifiers
        //public List<Modifier> modifiers = new List<Modifier>();

        //public void AddModifier(Modifier modifier) {
        //    modifiers.Add(modifier);
        //}
        //

        public abstract void Act();


    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Event : ScriptableObject{
        public string eventName;
        public List<Action> actions = new List<Action>();
        
        public void AddAction(Action action) {
            actions.Add(action);
        }


        //FIX Removing an Action does not delete them from the folder
        public void RemoveAction(Action action) {
            actions.Remove(action);
        }

        public void Execute() {
            foreach(Action action in actions) {
                action.Act();
            }
        }
    }
    
}
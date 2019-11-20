using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Event : ScriptableObject{
        public string eventName;
        public List<Action> actions = new List<Action>();
        
        public void AddAction(Action action) {
            //action = ScriptableObject.Instantiate(action);//make sure to create a new instance
            actions.Add(action);
        }

        public void Execute() {
            foreach(Action action in actions) {
                action.Act();
            }
        }
    }
    
}
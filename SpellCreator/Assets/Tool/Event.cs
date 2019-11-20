using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Event : ScriptableObject{
        public string eventName;
        public List<Action> actions = new List<Action>();

        public Event(string _name) {
            eventName = _name;
        }

        public void AddAction(Action action) {
            actions.Add(action);
        }

        public void Execute() {
            foreach(Action action in actions) {
                action.Act();
            }
        }
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Event {
        public string name;
        public List<Action> actions = new List<Action>();

        public Event(string _name) {
            name = _name;
            actions = new List<Action>();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    [System.Serializable]
    public class Event : ScriptableObject{
        public string eventName;
        public List<Action> actions = new List<Action>();

        private bool finishedExecuting = true;
        public bool FinishedExecuting { get {return finishedExecuting; } }

        public void AddAction(Action action) {
            actions.Add(action);
        }

        public void RemoveAction(Action action) {
            actions.Remove(action);
        }

        public IEnumerator ExecuteCoRoutine(GameObject g) {
            finishedExecuting = false;
            foreach(Action action in actions) {
                if(action.GetType() == typeof(Wait_Action)) {
                    Wait_Action wait = (Wait_Action)action;
                    yield return new WaitForSeconds(wait.waitTime);

                }else
                action.Act(g);
            }
            finishedExecuting = true;
        }
    }
    
}
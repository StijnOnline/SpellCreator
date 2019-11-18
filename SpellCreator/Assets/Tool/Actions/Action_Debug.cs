using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Debug_Action : Action {
        #region Modifiers

        public class Debug_Type_Modifier : Modifier {
            public enum Debug_Type { Log, Warn, Error };
            public Debug_Type DebugType = Debug_Type.Warn; //TODO: remove default value from testing
        }

        //etc.
        
        public Debug_Type_Modifier DebugTypeModifier = new Debug_Type_Modifier(); //TODO list

        #endregion

        public string debugText = "Some Debug Text";
        public string moredebugText = "Some Debug Text";

        public override void Act() {
            string text = "Debug Action: " + debugText;
            
            if(DebugTypeModifier.enabled) {
                switch(DebugTypeModifier.DebugType) {
                    case Debug_Type_Modifier.Debug_Type.Warn:
                        Debug.LogWarning(text);
                        break;
                    case Debug_Type_Modifier.Debug_Type.Error:
                        Debug.LogError(text);
                        break;
                    default:
                        Debug.Log(text);
                        break;
                }
            } else {
                Debug.Log(text);
            }

        }

        
    }


}
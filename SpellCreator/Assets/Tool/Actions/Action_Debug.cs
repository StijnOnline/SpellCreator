﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Debug_Action : Action {
        #region Modifiers
        public class ExtraText_Modifier : Modifier {
            public string extraText = "SomeExtraText";
        }

        public class Debug_Type_Modifier : Modifier {
            public enum Debug_Type { Log, Warn, Error };
            public Debug_Type DebugType = Debug_Type.Warn; //TODO: remove default value from testing
        }

        //etc.

        public ExtraText_Modifier ExtraTextModifier = new ExtraText_Modifier();
        public Debug_Type_Modifier DebugTypeModifier = new Debug_Type_Modifier();

        #endregion

        public override void Act() {
            string text = "Debug Action";
            if(ExtraTextModifier.enabled) {
                text += ": " + ExtraTextModifier.extraText;
            }
            
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
            }

        }

        
    }


}
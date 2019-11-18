using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpellCreator
{
    public static class ActionTracker
    {

        public static List<System.Type> FindActions() //FIX 
        {
            List<System.Type> actions = new List<System.Type>();
            //Reflection: find all actions        
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(Action)))
                    actions.Add(type);
            }

            return actions;

        }
    }

}
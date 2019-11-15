using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellCreator;
using System.Reflection;

public class Test : MonoBehaviour {
    public void Start() {
        SpellCreator.Event testevent = new SpellCreator.Event();
        Debug_Action testAction = new Debug_Action();

        //Editor and save/load system can access any variable via reflection
        PropertyInfo prop = testAction.DebugTypeModifier.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
        if(null != prop && prop.CanWrite) {
            prop.SetValue(testAction, true, null);
        }

        testevent.AddAction(testAction);

        testevent.Execute();
    }
}

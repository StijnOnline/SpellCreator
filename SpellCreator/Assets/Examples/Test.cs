using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellCreator;
using System.Reflection;

public class Test : MonoBehaviour {
    public void Start() {
        //SpellCreator.Event testevent = new SpellCreator.Event("TestEvent");
        //Debug_Action testAction = new Debug_Action();
        /*
        //Editor and save/load system can access any variable via reflection
        PropertyInfo prop = testAction.DebugTypeModifier.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
        if(null != prop && prop.CanWrite) {
            prop.SetValue(testAction, true, null);
        }

        PropertyInfo prop2 = testAction.GetType().GetProperty("debugText", BindingFlags.Public | BindingFlags.Instance);
        if(null != prop2 && prop2.CanWrite) {
            prop2.SetValue(testAction, "test debug text", null);
        }
        NOT WORKING
        */

        //testevent.AddAction(testAction);

        //EventSaver.SaveEvent(testevent);

        //testevent.Execute();



        //SpellCreator.Event e = EventSaver.LoadEventAsXML("TestEvent.xml");
        //e.Execute();
    }
}

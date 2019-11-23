using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellCreator;
using System.Reflection;

public class Test : MonoBehaviour {

    public string testeventname;
    public void Start() {
        //SpellCreator.Event e = EventSaver.LoadEventAsXML(testeventname);
        //e.Execute();

        SpellCreator.Event ev = EventSaver.LoadEventAsObject(testeventname);
        
        StartCoroutine(ev.ExecuteCoRoutine());
    }
}

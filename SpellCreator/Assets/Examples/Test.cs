using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpellCreator;
using System.Reflection;

public class Test : MonoBehaviour {

    public SpellCreator.Event testevent;
    public void Start() {
        
        StartCoroutine(testevent.ExecuteCoRoutine(gameObject));
    }
}

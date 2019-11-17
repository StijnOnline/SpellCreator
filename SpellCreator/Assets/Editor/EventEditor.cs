using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SpellCreator;

public class EventEditor : EditorWindow {

    private bool createAction = false;
    private static List<System.Type> subclasses = new List<System.Type>();

    [MenuItem("Window/Event Editor")]
    static void Init() {
        EventEditor window = (EventEditor)EditorWindow.GetWindow(typeof(EventEditor));

        
        //window.Show();
    }

    void OnFocus() {
        //Reflection: find all actions        
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach(var assembly in assemblies) {
            var types = assembly.GetTypes();
            foreach(var type in types) {
                if(type.IsSubclassOf(typeof(Action)))
                    subclasses.Add(type);
            }
        }
    }



    void OnGUI() {

        //TODO select/load
        //TODO Create

        int x = 20;

        BeginWindows();

        //tests
        GUILayout.Window(1, new Rect(5, x, position.width - 10, 100), ActionWindow, "Action Name");
        x += 105;
        GUILayout.Window(2, new Rect(5, x, position.width - 10, 100), ActionWindow, "Action Name");
        x += 105;



        if(!createAction) {
            if(GUI.Button(new Rect(5, x, position.width - 10, 20), "Add Action")) {
                createAction = true;
            }
        } else {
            GUILayout.Window(3, new Rect(5, x, position.width - 10, 60), NewActionWindow, "Add Action");
            

        }

        
       
        EndWindows();


        
    }

    void ActionWindow(int unusedWindowID) {
        GUILayout.Label("Action Vars");
        GUI.DragWindow();
    }

    void NewActionWindow(int unusedWindowID) {        
        var labelStyleButton = new GUIStyle();
        labelStyleButton.border = new RectOffset();

        int selected = 0;
        List<string> options = new List<string>();    
        foreach(System.Type type in subclasses) {
            options.Add(type.Name);
        }
        selected = EditorGUILayout.Popup("Action to Add", selected, options.ToArray());
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Cancel")) {
            createAction = false;
        }
        if(GUILayout.Button("Add Action")) {
            createAction = false;
        }
        GUILayout.EndHorizontal();

        GUI.DragWindow();
    }
}

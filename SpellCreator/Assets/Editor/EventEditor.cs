using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SpellCreator;
using System.IO;

public class EventEditor : EditorWindow {

    private SpellCreator.Event editingEvent;
    private int selectedEvent = 0;
    private bool addActionClicked = false;
    string createEventText = "";

    [MenuItem("Window/Event Editor")]
    static void Init() {
        EventEditor window = (EventEditor)EditorWindow.GetWindow(typeof(EventEditor));
    }

    void OnGUI() {
        GUILayout.Label("Event", "boldLabel");

        //Select Event
        var directory = new DirectoryInfo(EventSaver.DIR);
        var files = directory.GetFiles();

        if(files.Length > 0) {
            if(editingEvent == null) { editingEvent = EventSaver.LoadEvent(files[0].Name); }

            List<string> options = new List<string>();

            foreach(FileInfo file in files) {
                if(file.Extension == ".xml")
                    options.Add(file.Name);
            }

            selectedEvent = EditorGUILayout.Popup("Event:", selectedEvent, options.ToArray());

            if(editingEvent.name != options[selectedEvent]) {
                editingEvent = EventSaver.LoadEvent(options[selectedEvent]);
            }
        }

        //Create Event
        GUILayout.BeginHorizontal();
        GUILayout.Label("New Event:");
        createEventText = GUILayout.TextField(createEventText);
        if(GUILayout.Button("Create") && createEventText != "") {
            if(editingEvent != null) EventSaver.SaveEvent(editingEvent); // make sure to save old
            editingEvent = new SpellCreator.Event(createEventText);
            EventSaver.SaveEvent(editingEvent);
            createEventText = "";
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Path: " + EventSaver.DIR + editingEvent + ".xml");
        
        
        //Actions
        GUILayout.Label("Actions", "boldLabel");        
        int x = 110;
        int windowID = 0;
        BeginWindows();
        if(editingEvent != null) {
            foreach(Action action in editingEvent.actions) {
                GUILayout.Window(windowID++, new Rect(5, x, position.width - 10, 100), ActionWindow, action.GetType().Name);                
                x += 105;
            }
        }

        //Add Action
        if(!addActionClicked) {
            if(GUI.Button(new Rect(5, x, position.width - 10, 20), "Add Action")) {
                addActionClicked = true;
            }
        } else {
            GUILayout.Window(3, new Rect(5, x, position.width - 10, 60), AddActionWindow, "Add Action");
        }



        EndWindows();



    }

    void ActionWindow(int unusedWindowID) {
        GUILayout.Label(editingEvent.actions[unusedWindowID].GetType().Name);
        //Reflection to get all variables
    }

    void AddActionWindow(int unusedWindowID) {
        var labelStyleButton = new GUIStyle();
        labelStyleButton.border = new RectOffset();

        int selected = 0;
        List<string> options = new List<string>();
        List<System.Type> types = ActionTracker.FindActions();

        foreach(System.Type type in types) {
            options.Add(type.Name);
        }
        selected = EditorGUILayout.Popup("Action to Add", selected, options.ToArray());
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Cancel")) {
            addActionClicked = false;
        }
        if(GUILayout.Button("Add Action")) {
            addActionClicked = false;
            Action newAction = (Action)System.Activator.CreateInstance(types[selected]);
            editingEvent.AddAction(newAction);
            EventSaver.SaveEvent(editingEvent);
        }
        GUILayout.EndHorizontal();

        GUI.DragWindow();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SpellCreator;
using System.IO;
using System.Reflection;

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
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Used as divider
        GUILayout.Label("Actions", "boldLabel");

        HorizontalLine(5,2);
        if(editingEvent != null) {
            foreach(Action action in editingEvent.actions) {
                ActionWindow(action);           
            }
        }

        //Add Action
        if(!addActionClicked) {
            if(GUILayout.Button("Add Action")) {
                addActionClicked = true;
            }
        } else {
            AddActionWindow();
        }
    }

    void ActionWindow(Action _action) {
        GUILayout.Label(_action.GetType().Name);
        HorizontalLine(0,1);

        //Editor.CreateEditor(_action);


        //Reflection
        PropertyInfo[] properties = _action.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Variable: " + property.Name);
            GUILayout.EndHorizontal();
        }




        HorizontalLine(5,2);
    }

        void AddActionWindow() {
        GUILayout.Label("Add Action", "boldLabel");

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
    }

    public static void HorizontalLine(float padding, float height) {
        GUILayout.Space(padding);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), Color.grey);
        GUILayout.Space(padding);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SpellCreator;
using System.IO;
using System.Reflection;

public class EventEditor : EditorWindow {

    private static SpellCreator.Event editingEvent;
    private static int selectedEvent = 0;
    private static bool addActionClicked = false;
    private static string createEventText = "";
    private static Vector2 scrollPos = Vector2.zero;


    [MenuItem("Window/Event Editor")]
    static void Init() {
        EventEditor window = (EventEditor)EditorWindow.GetWindow(typeof(EventEditor));
    }

    void OnGUI() {

        scrollPos = GUILayout.BeginScrollView(scrollPos);


        GUILayout.Label("Event", "boldLabel");

        //Select Event
        var directory = new DirectoryInfo(EventSaver.SAVED_DATA_DIR);
        var files = directory.GetFiles();

        if(files.Length > 0) {
            //TODO Move this outside OnGui()

            List<string> options = new List<string>();

            foreach(FileInfo file in files) {
                if(file.Extension == ".xml")
                    options.Add(file.Name.Substring(0, file.Name.Length - 4));
            }

            selectedEvent = EditorGUILayout.Popup("Event:", selectedEvent, options.ToArray());

            if(editingEvent == null) {
                editingEvent = EventSaver.LoadEventAsXML(options[selectedEvent]);
            } else if(editingEvent.eventName != options[selectedEvent]) {
                editingEvent = EventSaver.LoadEventAsXML(options[selectedEvent]);
            }
        }


        if(editingEvent != null) GUILayout.Label("Path: " + EventSaver.SAVED_DATA_DIR + editingEvent.eventName + ".xml");

        //Create Event
        GUILayout.BeginHorizontal();
        GUILayout.Label("New Event:");
        createEventText = GUILayout.TextField(createEventText);
        if(GUILayout.Button("Create") && createEventText != "") {
            CreateEvent(createEventText);
        }
        GUILayout.EndHorizontal();

        


        //Actions
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Used as divider
        GUILayout.Label("Actions", "boldLabel");


        Separator(5, 2);
        if(editingEvent != null) {
            if(editingEvent.actions != null) {
                foreach(Action action in editingEvent.actions) {
                    ActionWindow(action);
                }
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


        GUILayout.EndScrollView();

    }

    static void ActionWindow(Action _action) {
        GUILayout.Label(_action.GetType().Name);
        Separator(0, 1);

        Editor e = Editor.CreateEditor(_action);
        e.OnInspectorGUI();

        //TODO: Create Modifier Editor

        Separator(5, 2);
    }

    static void AddActionWindow() {
        if(editingEvent == null) {
            addActionClicked = false;
            Debug.LogWarning("The event you are trying to add to is null");
        } else if(editingEvent.eventName == null) {
            addActionClicked = false;
            Debug.LogWarning("The event you are trying to add to is null");
        }

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
            Action newAction = null;
            
            newAction = (Action)AssetDatabase.LoadAssetAtPath(EventSaver.TOOL_DATA_DIR + options[selected] + ".asset", typeof(ScriptableObject));
            if(newAction == null) { Debug.LogError("Could not load asset at: " + EventSaver.TOOL_DATA_DIR + options[selected]); }
            
            editingEvent.AddAction(newAction);//FIX: create instance for editing, now two actions are the same
            Debug.Log("Created Action: " + newAction.ToString());

            EventSaver.SaveEventAsXML(editingEvent);
        }
        GUILayout.EndHorizontal();
    }

    public static void Separator(float padding, float height) {
        GUILayout.Space(padding);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), Color.grey);
        GUILayout.Space(padding);


    }
    public static void CreateEvent(string _name) {
        if(editingEvent != null) {
            if(editingEvent.eventName != null) EventSaver.SaveEventAsXML(editingEvent);// make sure to save old
        }        

        editingEvent = ScriptableObject.CreateInstance<SpellCreator.Event>();
        editingEvent.eventName = _name;

        EventSaver.SaveEventAsXML(editingEvent);
        createEventText = "";

        //TODO Make the new event the selected event
    }
}
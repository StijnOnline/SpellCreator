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
    private static int selectedNewAction = 0;
    private static bool addActionClicked = false;
    private static string createEventText = "";
    private static Vector2 scrollPos = Vector2.zero;

    private const string IconPath = "Assets/Tool/Icons/";
    private static Texture upIcon;
    private static Texture downIcon;


    [MenuItem("Window/Event Editor")]
    static void Init() {
        upIcon = (Texture) AssetDatabase.LoadAssetAtPath(IconPath + "upIcon.png", typeof(Texture));
        downIcon = (Texture) AssetDatabase.LoadAssetAtPath(IconPath + "downIcon.png", typeof(Texture));
        EventEditor window = (EventEditor)EditorWindow.GetWindow(typeof(EventEditor));
    }

    void OnGUI() {
        if (upIcon == null)
            upIcon = (Texture)AssetDatabase.LoadAssetAtPath(IconPath + "upIcon.png", typeof(Texture));
        if(downIcon == null)
            downIcon = (Texture)AssetDatabase.LoadAssetAtPath(IconPath + "downIcon.png", typeof(Texture));


        scrollPos = GUILayout.BeginScrollView(scrollPos);


        GUILayout.Label("Event", "boldLabel");

        //Select Event
        var directory = new DirectoryInfo(EventSaver.SAVED_DATA_DIR);
        var files = directory.GetFiles();

        if(files.Length > 0) {
            //FIX Move this outside OnGui()

            List<string> options = new List<string>();            

            string[] guids = AssetDatabase.FindAssets("t:Event", new string[] { EventSaver.SAVED_DATA_DIR.TrimEnd('/') }); ;
            foreach(string guid in guids) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                options.Add(assetPath.Replace(EventSaver.SAVED_DATA_DIR, "").Replace(".asset", ""));
            }



            selectedEvent = EditorGUILayout.Popup("Event:", selectedEvent, options.ToArray());

            if(editingEvent == null) {
                editingEvent = EventSaver.LoadEventAsObject(options[selectedEvent]);
            } else if(editingEvent.eventName != options[selectedEvent]) {
                editingEvent = EventSaver.LoadEventAsObject(options[selectedEvent]);
            }
            //FIX Save selected to file for later
        }


        if(editingEvent != null) GUILayout.Label("Path: " + EventSaver.SAVED_DATA_DIR + editingEvent.eventName + ".asset");

        //Create Event
        Separator(5, 2);
        createEventText = EditorGUILayout.TextField("New Event:", createEventText);
        if(GUILayout.Button("Create") && createEventText != "") {
            CreateEvent(createEventText);
        }




        //Actions
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //Used as divider
        GUILayout.Label("Actions", "boldLabel");


        Separator(5, 2);
        if(editingEvent != null) {
            if(editingEvent.actions != null) {
                foreach(Action action in editingEvent.actions) {
                    if(action != null)
                        if(ActionWindow(action))
                            break;
                }
            }
        }

        //Add Action
        if(!addActionClicked) {
            if(GUILayout.Button("Add Action")) {
                addActionClicked = true;
            }
        } else {
            if(editingEvent.actions != null) {
                AddActionWindow();
            }
        }


        GUILayout.EndScrollView();
    }

    //return true if collection is modified
    static bool ActionWindow(Action _action) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(_action.GetType().Name);

        GUIStyle orderButtonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
        orderButtonStyle.fixedWidth = 20f;
        orderButtonStyle.fixedHeight = 20f;

        if (GUILayout.Button(new GUIContent(upIcon), orderButtonStyle))
        {
            MoveActionUp(editingEvent.actions.IndexOf(_action));
            return true;
        }
        if (GUILayout.Button(downIcon, orderButtonStyle))
        {
            MoveActionUp(editingEvent.actions.IndexOf(_action) + 1);
            return true;
        }
        GUILayout.FlexibleSpace();

        GUIStyle removeButtonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
        removeButtonStyle.fixedWidth = 80f;

        if(GUILayout.Button("Remove", removeButtonStyle)) {
            editingEvent.RemoveAction(_action);

            string pathToDelete = AssetDatabase.GetAssetPath(_action);
            AssetDatabase.DeleteAsset(pathToDelete);
            return true;
        }
        GUILayout.EndHorizontal();

        //EditorGUILayout.HelpBox(_action.ActionToolTip, MessageType.Info);

        Separator(0, 1);

        Editor e = Editor.CreateEditor(_action);
        e.OnInspectorGUI();



        //TODO: Create Modifier Editor

        Separator(5, 2);
        return false;
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


        List<string> options = new List<string>();
        List<System.Type> types = ActionTracker.FindActions();

        foreach(System.Type type in types) {
            options.Add(type.Name);
        }
        selectedNewAction = EditorGUILayout.Popup("Action to Add", selectedNewAction, options.ToArray());
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Cancel")) {
            addActionClicked = false;
        }
        if(GUILayout.Button("Add Action")) {
            addActionClicked = false;
            Action newAction = null;

            //TODO: automatically create the object?
            newAction = (Action)AssetDatabase.LoadAssetAtPath(EventSaver.TOOL_DATA_DIR + options[selectedNewAction] + ".asset", typeof(ScriptableObject));
            if(newAction == null) { Debug.LogError("Could not load asset at: " + EventSaver.TOOL_DATA_DIR + options[selectedNewAction]); }

            newAction = ScriptableObject.Instantiate(newAction);//DISCUSS: memory leak or auto-collected?
            editingEvent.AddAction(newAction);
            Debug.Log("Created Action: " + newAction.ToString());

            EventSaver.SaveEventAsObject(editingEvent);
        }
        GUILayout.EndHorizontal();
    }

    public static void Separator(float padding, float height) {
        GUILayout.Space(padding);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), Color.grey);
        GUILayout.Space(padding);


    }
    public static void CreateEvent(string _name) {
        // make sure to save old, shouldn't be neccessary anymore
        if(editingEvent != null) {
            if(editingEvent.eventName != null) {
                EventSaver.SaveEventAsObject(editingEvent);
            }
        }

        editingEvent = ScriptableObject.CreateInstance<SpellCreator.Event>();
        editingEvent.eventName = _name;

        EventSaver.SaveEventAsObject(editingEvent);
        createEventText = "";

        //FIX Make the new event the selected event
    }

    //to move an action down, pass the index + 1
    public static void MoveActionUp(int index) {
        if(index > 0 && index < editingEvent.actions.Count) {
            Action tempAction = editingEvent.actions[index-1];
            editingEvent.actions[index - 1] = editingEvent.actions[index];
            editingEvent.actions[index] = tempAction;
        }
    }
}
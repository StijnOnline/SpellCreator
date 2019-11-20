using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace SpellCreator {
    //DO: Replace somethings:
    //use getfields instead of getproperties to make saving easier? 
    //less usefull for scriptableObject saving

    public static class EventSaver {

        public static string SAVED_DATA_DIR = "Assets/Tool/Data/Saved/";
        public static string TOOL_DATA_DIR = "Assets/Tool/Data/Tool/";
        public static string TEMP_DATA_DIR = "Assets/Tool/Data/Temp/";

        /* Description:
        // Events should be saved as XML, preferably like this 
        Events would later be saved as part of a spell
        (I think in this situation i can understand XML better than scriptable objects)

        <Event>
            <EventName> </EventName>
            <Action>
                <ActionName> </ActionName>
                -any amount of required variables of the Action
                <Modifier>
                    <ModifierName>  </ModifierName> 
                    -any amount of variables of the Modifier
                </Modifier>
                -More Modifiers
            </Action>
            -More Actions
        </Event>
        */
        public static void SaveEventAsXML(Event _event) {
            Debug.Log("Saving: " + _event.eventName);

            if(_event == null) { Debug.LogError("Event to Save is null");return; }
            if(_event.actions == null) {
                Debug.LogWarning("Event to Save has no actions");
                _event.actions = new List<Action>();
            }

            XmlDocument xmlDocument = new XmlDocument();
            XmlNode rootNode = xmlDocument.CreateElement("Event");
            xmlDocument.AppendChild(rootNode);

            foreach(Action action in _event.actions) {
                XmlNode actionNode = xmlDocument.CreateElement("Action");
                rootNode.AppendChild(actionNode);

                XmlNode actionNameNode = xmlDocument.CreateElement("ActionName");
                actionNameNode.InnerText = action.name;
                actionNode.AppendChild(actionNameNode);

                //Using Reflection
                FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                XmlNode firstModifier = null; //Used for the order of the nodes
                foreach(FieldInfo field in fields) {
                    if(field.FieldType.BaseType != typeof(Modifier)) {//Is not a Modifier
                        XmlNode actionVarNode = xmlDocument.CreateElement(field.Name);
                        actionVarNode.InnerText = field.GetValue(action).ToString();
                        if(firstModifier == null) {
                            actionNode.AppendChild(actionVarNode);
                        } else {
                            actionNode.InsertBefore(actionVarNode, firstModifier);
                        }

                    } else {//Is a Modifier
                        //TODO: Add if statement

                        XmlNode modifierNode = xmlDocument.CreateElement("Modifier");
                        actionNode.AppendChild(modifierNode);

                        if(firstModifier == null) { firstModifier = modifierNode; }

                        XmlNode modiferNameNode = xmlDocument.CreateElement("ModifierName");
                        modiferNameNode.InnerText = field.ToString();
                        modifierNode.AppendChild(modiferNameNode);

                        FieldInfo[] fields2 = field.FieldType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                        foreach(FieldInfo field2 in fields2) {
                            XmlNode modiferVarNode = xmlDocument.CreateElement(field2.Name);
                            modiferVarNode.InnerText = field2.GetValue(field.GetValue(action)).ToString();
                            modifierNode.AppendChild(modiferVarNode);

                        }
                    }

                }


            }

            if(!System.IO.Directory.Exists(SAVED_DATA_DIR)) { System.IO.Directory.CreateDirectory(SAVED_DATA_DIR); }
            xmlDocument.Save(SAVED_DATA_DIR + _event.eventName + ".xml");
        }

        public static Event LoadEventAsXML(string fileName) {
            Debug.Log("Attempting to Load: " + fileName);

            Event _loadedEvent = (SpellCreator.Event) ScriptableObject.CreateInstance(typeof(SpellCreator.Event));
            _loadedEvent.eventName = fileName;
            _loadedEvent.name = _loadedEvent.eventName;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SAVED_DATA_DIR + fileName + ".xml");

            foreach(XmlNode action in xmlDocument.FirstChild.ChildNodes) {

                Action newAction = null;
                System.Type actionType = null;

                newAction = (Action)AssetDatabase.LoadAssetAtPath(TOOL_DATA_DIR + action.FirstChild.InnerText + ".asset", typeof(Action));
                if(newAction == null) {
                    Debug.LogError("Could not load action: " + TOOL_DATA_DIR + action.FirstChild.InnerText);
                }

                actionType = newAction.GetType();
                if(actionType == null) { Debug.LogError("Action Type Not Recognized: " + action.FirstChild.InnerText); break; }              

                _loadedEvent.AddAction(newAction);

                foreach(XmlNode actionInfo in action.ChildNodes) {
                    if(actionInfo != actionInfo.FirstChild) {
                        if(actionInfo.Name != "Modifier") {
                            //Reflection
                            actionType.GetField(actionInfo.Name)?.SetValue(newAction, actionInfo.InnerText);
                        } else {
                            foreach(XmlNode modifierInfo in actionInfo.ChildNodes) {
                                //TODO: Modifier Loading Logic
                            }
                        }
                    }
                }
            }

            return _loadedEvent;
        }

        public static T CreateAsset<T>() where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T>();

            //string[] paths = Directory.GetFiles(Application.dataPath, "ScriptableObjectUtility.cs", SearchOption.AllDirectories);
            //string path = paths[0].Replace(Application.dataPath, "");

            //string path = 
            //string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, SAVED_DATA_DIR);

            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();

            return asset;
        }
    }
}
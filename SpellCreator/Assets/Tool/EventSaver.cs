using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace SpellCreator {
    //todo change getfields to getproperties
    public static class EventSaver {

        public static string DIR = "Assets/Saved/";

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
        public static void SaveEvent(Event _event) {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode rootNode = xmlDocument.CreateElement("Event");
            xmlDocument.AppendChild(rootNode);

            foreach(Action action in _event.actions) {
                XmlNode actionNode = xmlDocument.CreateElement("Action");
                rootNode.AppendChild(actionNode);

                XmlNode actionNameNode = xmlDocument.CreateElement("ActionName");
                actionNameNode.InnerText = action.ToString();
                actionNode.AppendChild(actionNameNode);

                //Using Reflection
                FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                XmlNode firstModifier = null; //Used for ordering the nodes
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
                            //TODO: if enabled

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

            if(!System.IO.Directory.Exists(DIR)) { System.IO.Directory.CreateDirectory(DIR); }
            xmlDocument.Save(DIR + _event.name + ".xml");
        }

        public static Event LoadEvent(string fileName) {
            Event _loadedEvent = (SpellCreator.Event) ScriptableObject.CreateInstance(typeof(SpellCreator.Event));
            _loadedEvent.name = fileName.Substring(0, fileName.Length - 4);
            //Event _loadedEvent = new Event(fileName.Substring(0, fileName.Length - 4));

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(DIR + fileName);

            foreach(XmlNode action in xmlDocument.FirstChild.ChildNodes) {

                Action newAction = null;
                System.Type actionType = null;
                actionType = System.Type.GetType(action.FirstChild.InnerText);
                if(actionType == null) { break; }
                //newAction = (Action)System.Activator.CreateInstance(actionType);
                //newAction = (Action)ScriptableObject.CreateInstance(actionType);
                newAction = (Action)ScriptableObjectUtility.CreateAsset < actionType> ();

                _loadedEvent.AddAction(newAction);

                foreach(XmlNode actionInfo in action.ChildNodes) {
                    if(actionInfo != actionInfo.FirstChild) {
                        if(actionInfo.Name != "Modifier") {
                            //Reflection
                            actionType.GetField(actionInfo.Name)?.SetValue(newAction, actionInfo.InnerText);
                        } else {
                            foreach(XmlNode modifierInfo in actionInfo.ChildNodes) {
                                //TODO: Modifier Logic
                            }
                        }
                    }
                }
            }

            return _loadedEvent;
        }
    }
}
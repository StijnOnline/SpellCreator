using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace SpellCreator {

    public static class EventSaver {

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

            XmlNode eventNameNode = xmlDocument.CreateElement("EventName");
            eventNameNode.InnerText = _event.name;
            rootNode.AppendChild(eventNameNode);

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


            xmlDocument.Save(Application.persistentDataPath + "/" + _event.name + ".xml");
        }
    }
}
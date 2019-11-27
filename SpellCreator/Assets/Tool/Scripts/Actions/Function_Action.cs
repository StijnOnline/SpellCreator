using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace SpellCreator {
    [CreateAssetMenu(fileName = "Function_Action", menuName = "Actions/Function_Action", order = 1)]
    public class Function_Action : Action {

        public string ObjectPath = "ExampleObject/ExampleChild";
        public string FullComponentName = "ExampleComponent";
        public string functionName = "ExampleFunction";

        public override void Act(GameObject g) {

            GameObject go = GameObject.Find(ObjectPath);
            if(go == null) {
                Debug.LogWarning("Function Action: Gameobject could not be Found");
                return;
            }

            System.Type componentType = System.Type.GetType(FullComponentName);
            object comp = go.GetComponent(componentType);

            if(comp == null) {
                Debug.LogWarning("Function Action: Component could not be Found");
                return;
            }

            MethodInfo magicMethod = componentType.GetMethod(functionName);

            if(magicMethod == null) {
                Debug.LogWarning("Function Action: Method could not be Found");
                return;
            }

            object magicValue = magicMethod.Invoke(comp, null);


        }
    }
}
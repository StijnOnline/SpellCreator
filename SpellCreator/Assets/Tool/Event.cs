﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCreator {
    public class Event {
        public List<Action> actions = new List<Action>();

        public void AddAction(Action action) {
            actions.Add(action);
        }

        public void Execute() {
            foreach(Action action in actions) {
                action.Act();
            }
        }
    }

    /*
    Events should be saved as XML, preferably like this 
    (I think in this situation i can understand XML better than scriptable objects)

    <Event>
        <EventName> <Event/Name>
        <Action>
            <ActionName> </ActionName>
            -any amount of variables depending on the Action
            <Modifier>
                <ModifierName>  </ModifierName> 
                -any amount of variables depending on the Modifier
            </Modifier>
        </Action>
    </Event>
    */
}
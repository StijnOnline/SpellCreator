using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpellCreator {
    [CreateAssetMenu(fileName = "PlayAudio_Action", menuName = "Actions/PlayAudio_Action", order = 1)]
    public class PlayAudio_Action : Action {
        public AudioClip audioClip;
        public float destroyDelay = 10;

        public override void Act(GameObject g) {

            GameObject go = new GameObject();
            go.AddComponent<AudioSource>().PlayOneShot(audioClip);
            Destroy(go, destroyDelay);
        }
    }
}
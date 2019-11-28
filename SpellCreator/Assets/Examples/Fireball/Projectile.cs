using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpellCreator.Event OnCreated;
    public SpellCreator.Event OnCollision;

    public void Start() {
        if(OnCreated != null)
        StartCoroutine( OnCreated.ExecuteCoRoutine(gameObject));
    }

    public void OnCollisionEnter(Collision collision) {
        if(OnCollision != null)
            StartCoroutine(OnCollision.ExecuteCoRoutine(gameObject));        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpellCreator.Event OnCreated;
    public SpellCreator.Event OnCollision;

    public void Start() {
        StartCoroutine( OnCreated.ExecuteCoRoutine(gameObject));
    }

    public void OnCollisionEnter(Collision collision) {
        StartCoroutine(OnCollision.ExecuteCoRoutine(gameObject));        
    }
    
}

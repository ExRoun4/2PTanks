using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameManagerScript GMS;
    Rigidbody2D rb;

    private int rebounds;

    Vector2 velocity;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 8, ForceMode2D.Impulse);
        GMS = GameObject.Find("{*GameManager*}").GetComponent<GameManagerScript>();
    }

    void Update() {
        if(!GMS.restart){
            velocity = rb.velocity;
            rb.simulated = true;
        } else {
            rb.simulated = false;
        }
        if(GMS.gameEnds){
            rb.simulated = false;
        }

        if(rebounds >= 7){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "wall" || col.gameObject.tag == "bound"){
            rebounds += 1;

            //change velocity
            var speed = velocity.magnitude;
            var dir = Vector3.Reflect(velocity.normalized, col.contacts[0].normal);
            rb.velocity = dir * Mathf.Max(speed, 0f);

            //play sound
            GMS.soundSource.PlayOneShot(GMS.bounceSound, 0.5f);
        }
    }
}

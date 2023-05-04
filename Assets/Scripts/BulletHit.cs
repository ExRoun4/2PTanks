using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    GameManagerScript GMS;

    float reset;

    void Start(){
        reset = 0.1f;
        GMS = GameObject.Find("{*GameManager*}").GetComponent<GameManagerScript>();
    }

    void Update(){
        reset -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(reset <= 0){
            if(col.gameObject.tag == "player"){
                if(!GMS.restart){
                    GMS.restartRound = 2;
                    GMS.round += 1;
                    GMS.restart = true;

                    if(col.gameObject.GetComponent<TankScript>().TANK == "RED"){
                        GMS.score[0] += 1;
                    } else {
                        GMS.score[1] += 1;
                    }

                    col.gameObject.GetComponent<TankScript>().model.SetActive(false);
                    
                    Instantiate(GMS.explosionParticle, col.gameObject.transform.position, Quaternion.identity);
                    //play sound
                    GMS.soundSource.PlayOneShot(GMS.playerExplosion, 0.5f);
                }
            }
        }
    }
}

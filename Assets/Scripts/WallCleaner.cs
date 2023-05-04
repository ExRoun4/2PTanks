using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCleaner : MonoBehaviour
{
    GameManagerScript GMS;

    float clearTimer = 0.2f;
    
    void Start(){
        GMS = GameObject.Find("{*GameManager*}").GetComponent<GameManagerScript>();
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.tag == "wall"){
            Destroy(col.gameObject.transform.parent.gameObject);
        }
    }

    void Update(){
        if(clearTimer > 0){
            clearTimer -= Time.deltaTime;
        }

        //clear way
        if(clearTimer <= 0){
            if(transform.position.x <= -11 && transform.position.y <= -4){
                Destroy(gameObject);
                GMS.gameStarted = true;
            }

            if(transform.position.x == -11){
                //face left
                transform.position = new Vector2(transform.position.x, transform.position.y-2);
                transform.rotation = Quaternion.Euler(0, 0, 0);

                clearTimer = 0.2f;
                return;
            }
            if(transform.position.y == -4){
                //face down
                transform.position = new Vector2(transform.position.x-2, transform.position.y);
                transform.rotation = Quaternion.Euler(0, 0, -90);

                clearTimer = 0.2f;
                return;
            }

            int random = Random.Range(1, 4);
            if(random == 1){
                //face left
                transform.position = new Vector2(transform.position.x, transform.position.y-2);
                transform.rotation = Quaternion.Euler(0, 0, 0);

                clearTimer = 0.2f;
            }
            if(random == 3 || random == 2){
                //face down
                transform.position = new Vector2(transform.position.x-2, transform.position.y);
                transform.rotation = Quaternion.Euler(0, 0, -90);

                clearTimer = 0.2f;
            }
        }
    }
}

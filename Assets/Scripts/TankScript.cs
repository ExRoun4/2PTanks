using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankScript : MonoBehaviour
{
    GameManagerScript GMS;
    Rigidbody2D rb;

    public float speed;
    public Joystick moveJoystick;
    public Joystick shootJoystick;
    public GameObject tower;

    public GameObject model;

    public GameObject bulletPrefab;
    public Transform shootPoint;

    private float shootRecoil;
    public string TANK;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        GMS = GameObject.Find("{*GameManager*}").GetComponent<GameManagerScript>();
    }

    void Update(){
        //recoil
        if(shootRecoil > 0){
            shootRecoil -= Time.deltaTime;
        }
        
        //rotation
        if(GMS.gameStarted){
            if(!GMS.gameEnds){
                if(!GMS.restart){
                    //move
                    if(moveJoystick.Horizontal != 0 || moveJoystick.Vertical != 0){
                        float angle = Mathf.Atan2(moveJoystick.Horizontal, moveJoystick.Vertical) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, -angle);
                    }

                    //aim and shoot
                    if(shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0){
                        float angle = Mathf.Atan2(shootJoystick.Horizontal, shootJoystick.Vertical) * Mathf.Rad2Deg;
                        tower.transform.rotation = Quaternion.Euler(0, 0, -angle);
                        shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y, -angle);

                        Shoot();
                    } else if(shootJoystick.Horizontal == 0 && shootJoystick.Vertical == 0){
                        tower.transform.rotation = Quaternion.Lerp(tower.transform.rotation, transform.rotation, 10 * Time.deltaTime);
                        shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y, -90);
                    }

                    model.SetActive(true);
                } else {
                    float angle = Mathf.Atan2(shootJoystick.Horizontal, shootJoystick.Vertical) * Mathf.Rad2Deg;
                    tower.transform.rotation = Quaternion.Euler(0, 0, -angle);
                    shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, shootJoystick.gameObject.transform.GetChild(0).gameObject.transform.eulerAngles.y, -90);
                }
            }
        }
    }

    void FixedUpdate(){
        //movement
        if(!GMS.gameEnds && !GMS.restart){
            if(moveJoystick.Horizontal != 0 || moveJoystick.Vertical != 0){
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }
    }

    public void Shoot(){
        if(!GMS.gameEnds){
            if(!GMS.restart){
                if(shootRecoil <= 0){
                    shootRecoil = 0.5f;

                    //shoot
                    GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, tower.transform.rotation);
                    bullet.transform.parent = GMS.projectileParent.transform;

                    GMS.bulletsFired += 1;

                    //play sound
                    GMS.soundSource.PlayOneShot(GMS.shootSound, 0.5f);
                }
            }
        }
    }
}

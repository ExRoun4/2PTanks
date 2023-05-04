using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject generationParent;
    public GameObject projectileParent;

    public GameObject wallPrefab;
    private int rotation;

    public GameObject wayCleaner;

    public GameObject explosionParticle;

    public AudioSource soundSource;
    public AudioClip shootSound;
    public AudioClip bounceSound;
    public AudioClip playerExplosion;

    [HideInInspector] public bool gameStarted;
    [HideInInspector] public float restartRound;
    [HideInInspector] public bool restart = false;
    [HideInInspector] public bool gameEnds;

    [HideInInspector] public int[] score = {0, 0};
    [HideInInspector] public int round = 1;

    public TextMeshProUGUI[] roundCounterText;
    public TextMeshProUGUI[] scoreText;

    public GameObject RoundRecoil;
    public GameObject WinPanel;

    public TextMeshProUGUI WinnerText;
    public TextMeshProUGUI StatsText;

    public Transform[] Tanks;

    [HideInInspector] public int bulletsFired;

    void Start() {
        //generation
        generate();
    }

    void Update(){
        roundCounterText[0].text = "Round <color=green>"+round+"</color>";
        roundCounterText[1].text = "Round <color=green>"+round+"</color>";

        scoreText[0].text = "<color=blue>Blue</color> > "+score[0]+" / 10\n<color=red>Red</color> > "+score[1]+" / 10";
        scoreText[1].text = "<color=blue>Blue</color> > "+score[0]+" / 10\n<color=red>Red</color> > "+score[1]+" / 10";

        if(gameStarted){
            RoundRecoil.SetActive(false);
        } else {
            RoundRecoil.SetActive(true);
        }

        if(restart){
            restartRound -= Time.deltaTime;
            
            if(restartRound <= 0){
                if(score[0] < 10 && score[1] < 10){
                    for(int i = 0; i < generationParent.transform.childCount; i++){
                        Destroy(generationParent.transform.GetChild(i).gameObject);
                    }

                    for(int i = 0; i < projectileParent.transform.childCount; i++){
                        Destroy(projectileParent.transform.GetChild(i).gameObject);
                    }

                    generate();
                    restart = false;
                    gameStarted = false;
                } else {
                    gameEnds = true;
                    WinPanel.SetActive(true);
                    for(int i = 0; i < projectileParent.transform.childCount; i++){
                        Destroy(projectileParent.transform.GetChild(i).gameObject);
                    }

                    if(score[0] == 10){
                        WinnerText.text = "<color=blue>BLUE WINS</color>";
                    }
                    if(score[1] == 10){
                        WinnerText.text = "<color=red>RED WINS</color>";
                    }

                    StatsText.text = "<color=green>Game Stats:</color>\n<color=blue>Blue</color> > "+score[0]+" / 10\n<color=red>Red</color> > "+score[1]+" / 10\n\n<color=green>Round:</color> <color=white>"+round+"</color>\n<color=green>Bullets fired:</color> "+bulletsFired+"";
                }
            }
        }
    }

    public void generate(){
        GameObject generator = Instantiate(new GameObject(), new Vector2(-10, 4), Quaternion.identity);

        int width = 0;
        bool rotate = false;

        //generate walls
        for(int i = 0; i < 108; i++){
            width += 1;
            if(Random.Range(1, 4) == 2){
                int z = 0;
                if(rotate){
                    z = 90;
                } else {
                    z = 0;
                }

                GameObject wall = Instantiate(wallPrefab, generator.transform.position, Quaternion.Euler(0, 0, z));
                wall.transform.parent = generationParent.transform;
            }
            generator.transform.position = new Vector2(generator.transform.position.x + 2, generator.transform.position.y);

            if(width >= 11){
                width = 0;

                if(rotate){
                    rotate = false;
                    generator.transform.position = new Vector2(-10, generator.transform.position.y-1);
                } else {
                    rotate = true;
                    generator.transform.position = new Vector2(-11, generator.transform.position.y-1);
                }
            }
        }

        Destroy(generator);

        //instantiate way cleaner
        GameObject cleaner = Instantiate(wayCleaner, new Vector2(11, 4), Quaternion.identity);

        //teleport players
        Tanks[0].position = new Vector2(-11, -4);
        Tanks[0].transform.rotation = Quaternion.Euler(0, 0, -90);

        Tanks[1].position = new Vector2(11, 4);
        Tanks[1].transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    public void BackToMenu(){
        SceneManager.LoadScene(0);
    }
}

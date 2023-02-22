using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;


public class NPC : MonoBehaviour
{

    public bool final_npc;
    public int strength;
    private bool ableToHitPlayer=false;
    public Text damageText;
    private Player player;
    public int health;
    public float moveSpeed;
    public int baseAttack;
    public float chaseRadius;
    public float attackRadius;
    Transform playerTarget;
    private float changeX;
    private float changeY;
    private Animator animator;
    public AudioSource hitAudioSource;
    public bool finalBoss;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        changeX = transform.position.x;
        changeY = transform.position.y;
        
        playerTarget = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player){
            if (this.ableToHitPlayer){
                //damageText.text = $"NPC hit you by {this.strength.ToString()}";
                this.player.hit(this.strength);
            }
            this.ableToHitPlayer = false;
        }
        checkDistance();
    }

    void checkDistance(){
        
        if (Vector3.Distance(playerTarget.position, transform.position) <= chaseRadius &&
            Vector3.Distance(playerTarget.position, transform.position) > attackRadius){
            
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
        }

        Vector3 newPos = transform.position;

        double xDirection = Math.Round(changeX - newPos.x, 2);
        double yDirection = Math.Round(changeY - newPos.y, 2);
        
        changeX = newPos.x;
        changeY = newPos.y;

        if (xDirection==0.00 && yDirection==0.00){
            animator.SetBool("moving", false);
        }else{
            int finalXValue, finalYValue;

            if (xDirection==0.00){
                finalXValue = 0;
            }else{
                bool leftMovement = xDirection>0;
                if (leftMovement){
                    finalXValue = -1;
                }else{
                    finalXValue = 1;
                }
            }
            
            if (yDirection==0.00){
                finalYValue = 0;
            }else{
                bool downMovement = yDirection>0;
                if (downMovement){
                    finalYValue = -1;
                }else{
                    finalYValue = 1;
                }
            }

            animator.SetFloat("moveY", finalYValue);
            animator.SetFloat("moveX", finalXValue);

            animator.SetBool("moving", true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        player = null;
    }

    void OnTriggerEnter2D(Collider2D other){
        player = other.GetComponent<Player>();
        if (player){
            this.ableToHitPlayer=true;

            Vector2 direction = other.gameObject.transform.position - transform.position;
            bool leftMovement = direction.x>0;
            bool downMovement = direction.y>0;
            
            player.allowLeftMovement= !leftMovement;
            player.allowRightMovement= leftMovement;
            player.allowDownMovement= !downMovement;
            player.allowUpMovement = downMovement;
        }
    }

    public void hit(int value){
        health-=value;

        this.player.increaseExp(this.strength);
        
        if (health<=0){
            
            this.player.increaseKilledOgres();

            if (this.finalBoss){
                SceneManager.LoadScene("Won");
            }

            Destroy(gameObject);
        }else{
            hitAudioSource.Play();
        }
    }

}

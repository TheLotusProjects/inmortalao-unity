using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public static string name;

    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public Text playerName; 

    public Image healthImage;

    private int level=1;
    private int exp=1;
    private int totalExp=10;
    private int health=40;
    private int totalHealth=40;
    private int strength=10;

    public Text txtLevel;
    public Text txtExp;
    public Text txtHealth;
    public Text txtKilledOgres;

    public bool allowLeftMovement=true;
    public bool allowRightMovement=true;
    public bool allowUpMovement=true;
    public bool allowDownMovement=true;

    private NPC npcToHit=null;
    public PlayerHead playerHead;

    public AudioSource walkingAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource drinkPotionAudioSource;
    public AudioSource levelUpAudio;
    public AudioSource clickAudio;

    private int killedOgres=0;
    private int totalOgresToKill=30;

    private bool moving=false;

    private float initialHeathWidthSize;

    public NpcSpawner npcSpawner;

    public void increaseExp(int amount){
        this.exp+= amount;
        if (this.exp>=this.totalExp){
            this.level+=1;
            this.exp = this.exp % this.totalExp;
            this.totalExp += this.level * 20;
            this.strength = this.strength * 2;
            this.updateLevel();
            levelUpAudio.Play();
        }
        this.updateExp();
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerName.text = name;
        
        this.updateLevel();
        this.updateExp();
        this.updateHealth();
        this.updateKilledOgres();

        StartCoroutine(ReproduceWalkingSound());

        RectTransform rectTrs = this.healthImage.GetComponent<RectTransform>();
        initialHeathWidthSize = rectTrs.sizeDelta.x;
    }


    IEnumerator ReproduceWalkingSound() {
		
		while (true) {
            if (this.moving){
                walkingAudioSource.Play();
                yield return new WaitForSeconds(0.4F);
            }
            yield return new WaitForSeconds(0F);
        }
    }

    public bool ableToSpawnBossNpc(){
        return this.killedOgres>=this.totalOgresToKill;
    }

    public void increaseKilledOgres(){
        this.killedOgres+=1;
        this.updateKilledOgres();
        this.npcSpawner.reduceOgres();
    }

    public void updateKilledOgres(){
        txtKilledOgres.text = this.killedOgres.ToString() + " / " + this.totalOgresToKill.ToString();
    }

    public void updateLevel(){
        txtLevel.text = this.level.ToString();   
    }

    private void updateExp(){
        txtExp.text = this.exp.ToString() + " / " + this.totalExp.ToString();
    }

    private void updateHealth(){
        txtHealth.text = this.health.ToString() + " / " + this.totalHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        move();

        if (this.health<=0){
            SceneManager.LoadScene("End");
        }

        bool pressCtrl = Input.GetKeyDown(KeyCode.LeftControl);

        if (pressCtrl){
            if (npcToHit){
                npcToHit.hit(this.strength);
            }
        }
        
    }

    protected void move(){
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero){
            MoveCharacter();
            playerHead.updateAnimator(change);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
            this.moving=true;
        }else{
            animator.SetBool("moving", false);
            this.moving=false;
        }
    }

    private void allowAllMovements(){
        this.allowLeftMovement=true;
        this.allowRightMovement=true;
        this.allowUpMovement=true;
        this.allowDownMovement=true;
    }

    void MoveCharacter(){
        Vector3 plus = new Vector3(0, 0, 0);
        
        if (change.x<0 && this.allowLeftMovement ||
            change.x>0 && this.allowRightMovement ||
            change.y<0 && this.allowDownMovement || 
            change.y>0 && this.allowUpMovement   ){
            plus = change * speed * Time.deltaTime;
            this.allowAllMovements();
        }

        Vector3 vect = transform.position + plus;

        myRigidBody.MovePosition(
            vect
        );

    }

    void OnTriggerExit2D(Collider2D other){
        npcToHit = null;
    }

    void OnTriggerEnter2D(Collider2D other){
        npcToHit = other.GetComponent<NPC>();
    }

    public void hit(int value){
        this.health -= value;
        float percentage = (float)value/this.totalHealth;
        
        RectTransform rectTrs = this.healthImage.GetComponent<RectTransform>();
        float valToSubs = -1*(initialHeathWidthSize*percentage)/2;
        changeHealthStatusBar(-1*percentage, valToSubs);

        this.updateHealth();
        hitAudioSource.Play();
    }

    public void drinkPotion(){
        clickAudio.Play();

        if (this.health>= this.totalHealth)
            return;

        RectTransform rectTrs = this.healthImage.GetComponent<RectTransform>();
        int value = 10;
        float percentage = (float)value/this.totalHealth;
        float valToSubs = (initialHeathWidthSize*percentage)/2;
        changeHealthStatusBar(percentage, valToSubs);
        this.health+=value;

        this.updateHealth();
        drinkPotionAudioSource.Play();
    }

    private void changeHealthStatusBar(float percentage, float valToSubs){
        RectTransform rectTrs = this.healthImage.GetComponent<RectTransform>();
        float valRect = rectTrs.sizeDelta.x+initialHeathWidthSize*percentage;
        rectTrs.position = new Vector2(rectTrs.position.x + valToSubs, rectTrs.position.y);
        rectTrs.sizeDelta = new Vector2(valRect, rectTrs.sizeDelta.y);
    }

}

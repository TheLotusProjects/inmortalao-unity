using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{

    public GameObject basicNpc;
    public GameObject finalNpc;
    public Player player;

    public bool hasFinalBoss=false;
    private int amountOfOgres=0;
    private int totalOgres=15;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNpc());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.ableToSpawnBossNpc() && this.hasFinalBoss == false){
            Instantiate(finalNpc, new Vector3(0, 0, 0),
				Quaternion.Euler(0f, 0f, 0f));
            this.hasFinalBoss = true;
        }
    }

    public void reduceOgres(){
        amountOfOgres-=1;
    }

    IEnumerator SpawnNpc() {
		while (true) {

            if (amountOfOgres==totalOgres) continue;

			// instantiate a random airplane past the right egde of the screen, facing left
			Instantiate(basicNpc, new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0),
				Quaternion.Euler(0f, 0f, 0f));

            amountOfOgres+=1;

			// pause this coroutine for 1 second
			yield return new WaitForSeconds(3);

		}
	}
}

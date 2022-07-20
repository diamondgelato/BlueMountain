using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject death;
    [SerializeField] GameObject hitParticle;
    [SerializeField] int scoreIncrement = 15;
    [SerializeField] int hitPoints = 3;
    
    ScoreBoard scoreboard;
    Rigidbody rb;
    GameObject parent;
    GameObject deathFX;
    int currentHitPoints;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        scoreboard = FindObjectOfType<ScoreBoard>();
        parent = GameObject.FindWithTag("SpawnRuntime");

        currentHitPoints = hitPoints;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    private void OnParticleCollision(GameObject other) {
        ProcessHit();
        KillEnemy();
    }

    void ProcessHit () {
        scoreboard.UpdateScore(scoreIncrement);
    }

    void KillEnemy() {
        currentHitPoints -= 1;

        if (currentHitPoints == 0) {
            // instantiate explosion 
            Debug.Log(gameObject. name + " killed");
            deathFX = Instantiate(death, transform.position, Quaternion.identity, parent.transform);
            Destroy(gameObject);
        } else {
            // change color for 0.1 sec
            Debug.Log(gameObject.name + " hits: " + (hitPoints - currentHitPoints));
            deathFX = Instantiate(hitParticle, transform.position, Quaternion.identity, parent.transform);
        }
    }
}

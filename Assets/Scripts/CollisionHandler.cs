using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    [SerializeField] ParticleSystem collisionParticles;
    MeshRenderer mesh;

    float delayTime = 1f;
    int currSceneInd;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log(gameObject.name + " triggered by: " + other.gameObject.name);

        CrashHandler(); 
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log(gameObject.name + " collision with: " + other.gameObject.name);
    }

    void CrashHandler() {
        // disable player control
        GetComponent<PlayerController>().enabled = false;

        // Play particles
        collisionParticles.Play();

        DisablePlayer();

        // wait one second and get next level
        Invoke("ReloadLevel", delayTime);
    }

    void ReloadLevel () {
        currSceneInd = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneInd);
    }

    void DisablePlayer () {
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren) {
            if(child.gameObject.GetComponent<MeshRenderer>()){
                child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }    
        }

        Collider coll = gameObject.GetComponent<Collider>();
        coll.enabled = false;
    }
}

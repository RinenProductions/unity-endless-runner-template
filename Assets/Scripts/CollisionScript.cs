using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour {
    GameManager gameManager;
    PlayerController controller;
    CameraController camera;
    public GameObject Sparks;
    public GameObject Explosion;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        camera = GameObject.Find("Normal Camera").GetComponent<CameraController>();
        controller = transform.root.GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Obstacle")
        {
            if (gameManager.active)
            {
                gameManager.FailState(collision.relativeVelocity);
                GetComponent<Rigidbody>().AddForceAtPosition(-collision.relativeVelocity, collision.contacts[0].point, ForceMode.Impulse);
            }
            StartCoroutine(camera.screenShake(1.0f, 0.5f));
            GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion, 1.5f);
        }
        else
        {
            StartCoroutine(camera.screenShake(0.5f, 0.2f));
        }
        GameObject go = Instantiate(Sparks, collision.contacts[0].point, Quaternion.identity);
        Destroy(go, 0.5f);
    }

}

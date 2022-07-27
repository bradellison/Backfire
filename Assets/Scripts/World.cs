using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    //public Gradient[] colorings;
    public GameObject worldDestoryParticles;

    void Start()
    {
        //this.gameObject.GetComponent<MapGenerator>().coloring = colorings[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Bullet") {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }

    void OnDestroy() {
        GameObject particles = Instantiate(worldDestoryParticles);
        particles.transform.position = this.transform.position;
    }
}

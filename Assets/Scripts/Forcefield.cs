using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcefield : MonoBehaviour
{

    PlayerController player;
    public float forcefieldBulletHitCounterDecrease;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.transform.position;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "World") {
            player.HitWorld(other.gameObject);
        } else if (other.gameObject.tag == "Bullet") {
            HitBullet(other.gameObject);
        }
    }

    void HitBullet(GameObject bullet) {
        player.DecreaseForceFieldCounter(forcefieldBulletHitCounterDecrease);
        Destroy(bullet);
    }

}

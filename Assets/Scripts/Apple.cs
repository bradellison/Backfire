using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}

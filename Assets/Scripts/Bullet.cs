using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float moveSpeed;

    [Header("Script Managed Variables")]
    public Vector3 movementVector;
    public Vector2 camWorldSize;

    Vector2 spriteSize = new Vector2(); 

    TrailRenderer trailRenderer;

    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        spriteSize.x = this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSize.y = this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;

        StartCoroutine(EnableCollisions());

        trailRenderer = this.gameObject.transform.GetChild(0).GetComponent<TrailRenderer>();
    }

    IEnumerator EnableCollisions()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>(), false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CheckScreenEdgesWithSingle();
    }

    void MoveBullet() {
        transform.Translate(movementVector * moveSpeed * Time.deltaTime);
    }

    void CheckScreenEdgesWithSingle() {
        float halfX = spriteSize.x / 2;
        float halfY = spriteSize.y / 2;

        //checks if the edge of the bullet has reached the border, and moves it to the other side of the screen
        if(transform.position.x - halfX > camWorldSize.x && movementVector == Vector3.right) {
            Vector3 newSpawnVector = new Vector3(-camWorldSize.x - halfX, transform.position.y, transform.position.z);
            StartCoroutine(HitScreenEdge(newSpawnVector));
        } else if(transform.position.x + halfX < -camWorldSize.x && movementVector == Vector3.left) {
            Vector3 newSpawnVector = new Vector3(camWorldSize.x + halfX, transform.position.y, transform.position.z);
            StartCoroutine(HitScreenEdge(newSpawnVector));
        } else if (transform.position.y - halfY > camWorldSize.y && movementVector == Vector3.up) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, -camWorldSize.y - halfY, transform.position.z);
            StartCoroutine(HitScreenEdge(newSpawnVector));
        } else if (transform.position.y + halfY < -camWorldSize.y && movementVector == Vector3.down) {
            Vector3 newSpawnVector = new Vector3(transform.position.x, camWorldSize.y + halfY, transform.position.z);
            StartCoroutine(HitScreenEdge(newSpawnVector));
        }
    } 

    IEnumerator HitScreenEdge(Vector3 newSpawnVector) {
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = newSpawnVector;
        yield return new WaitForSeconds(0.1f);
        trailRenderer.emitting = true;
    }

}

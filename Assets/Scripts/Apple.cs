using UnityEngine;

public class Apple : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Bullet")) {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }
}

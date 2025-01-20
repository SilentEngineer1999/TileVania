using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    bool wasCollected = false;
    [SerializeField] int coinValue = 100;
    [SerializeField] AudioClip coinPickupSFX;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindFirstObjectByType<GameSession>().AddToScore(coinValue);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Attack : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fragile fragile = collision.gameObject.GetComponent<Fragile>();
        fragile.Damage(1);
    }

}

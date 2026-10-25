using UnityEngine;

public class Fragile : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] FragileParticle particle;
    [SerializeField] Color color;

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        FragileParticle fp = Instantiate(particle, transform.position, Quaternion.identity);
        UIManager.instance.Scores += 100;
        Destroy(fp, 2);
        Destroy(gameObject);
    }

}

using UnityEngine;

public class FragileParticle : MonoBehaviour
{

    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void ChangeColor(Color color)
    {
        var pMain = particle.main;
        pMain.startColor = color;
    }

}

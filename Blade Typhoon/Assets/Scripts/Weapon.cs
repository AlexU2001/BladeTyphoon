using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _edge;
    [SerializeField] private float _damage;

    [SerializeField] private ParticleSystem[] _particles;

    private float _initialDamage;

    private float _particleRate = 15f;
    private float _initialParticleRate;

    private void Awake()
    {
        _initialDamage = _damage;
        _initialParticleRate = _particleRate;
    }

    private void OnEnable()
    {
        Timer.Instance.TimerStarted += DoublerEffect;
        Timer.Instance.TimerOver += DoublerEffectEnd;
    }
    private void OnDisable()
    {
        Timer.Instance.TimerStarted -= DoublerEffect;
        Timer.Instance.TimerOver -= DoublerEffectEnd;
    }
    private void DoublerEffect(string tag)
    {
        if (!tag.Equals("Doubler"))
            return;

        _damage *= 2;
        _particleRate *= 2;
        var em = _particles[0].emission;
        em.rateOverTime = _particleRate;
        _particles[0].Play();
    }

    private void DoublerEffectEnd(string tag)
    {
        if (!tag.Equals("Doubler"))
            return;

        _damage = _initialDamage;
        _particleRate = _initialParticleRate;
        var em = _particles[0].emission;
        em.rateOverTime = _initialParticleRate;
        _particles[0].Stop();
    }
}

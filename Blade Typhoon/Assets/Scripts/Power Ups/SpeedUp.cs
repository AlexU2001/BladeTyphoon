using UnityEngine;
public class SpeedUp : PowerUp
{
    private Player _player;

    [SerializeField] private float _duration;

    private void Start()
    {
        _player = Player.Instance;
    }
    public override void OnSpawn()
    {
    }
    public override void OnPickUP()
    {
        Timer.Instance.StartTimer("SpeedUp", _duration);
        Destroy(gameObject);
    }
}

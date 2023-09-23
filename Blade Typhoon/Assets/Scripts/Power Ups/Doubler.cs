using UnityEngine;
namespace Assets.Scripts.Power_Ups
{
    public class Doubler : PowerUp
    {

        [SerializeField] private float _duration;

        private Player _player;
        private void Awake()
        {
            _player = Player.Instance;
        }
        public override void OnSpawn()
        {

        }
        public override void OnPickUP()
        {
            Timer.Instance.StartTimer("Doubler", _duration);
            Destroy(gameObject);
        }
    }
}
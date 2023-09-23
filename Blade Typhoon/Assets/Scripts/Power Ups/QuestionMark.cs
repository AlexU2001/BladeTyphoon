using UnityEngine;

namespace Assets.Scripts.Power_Ups
{
    public class QuestionMark : PowerUp
    {
        [SerializeField] private Weapon[] _weapons;

        private Player _player;

        private void Start()
        {
            _player = Player.Instance;
        }

        public override void OnSpawn()
        {
            //Debug.Log("Spawned Power Up");
        }
        public override void OnPickUP()
        {
            int randomWeapon = Random.Range(0, _weapons.Length);
            int randomSlot = Random.Range(0, 3);
            _player.AddWeapon(_weapons[randomWeapon], randomSlot);
            Destroy(gameObject);
        }
    }
}
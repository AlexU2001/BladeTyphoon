using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class Larva : Enemy
    {
        private SpriteRenderer _renderer;
        public override void Attack()
        {
            //throw new System.NotImplementedException();
        }

        public override void Move()
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();

            float xPos = Player.Instance.transform.position.x;
            if (xPos >= transform.position.x)
                _renderer.flipX = true;
            else
                _renderer.flipX = false;

            transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, speed * Time.deltaTime);
        }

        public override void OnStateChange()
        {
            //throw new System.NotImplementedException();
        }

        public override void WhileOnState()
        {
            //throw new System.NotImplementedException();
        }
    }
}
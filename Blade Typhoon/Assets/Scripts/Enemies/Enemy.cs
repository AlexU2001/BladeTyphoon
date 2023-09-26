using System.Collections;
using UnityEngine;

public enum EnemyState { Undecided, Rage, Reluctance, Fear, Paralysed }
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyState state = EnemyState.Undecided;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float health;

    [SerializeField] protected GameObject[] _drops;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _knocbackHeight;
    [SerializeField] private AnimationCurve _knocbackDistance;
    private bool _knocbacked;
    protected Rigidbody2D _rb;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Attack();

    public abstract void Move();

    public virtual void ChangeStates(EnemyState state)
    {
        this.state = state;
        OnStateChange();
    }
    public abstract void OnStateChange();
    public abstract void WhileOnState();

    public virtual void TakeDamage(float dmg, Vector2 direction, float knockpower)
    {
        if (health <= 0)
            return;

        health -= dmg;
        StartCoroutine(Knockback(direction, knockpower));

        if (health <= 0)
            Die();
    }

    private IEnumerator Knockback(Vector2 direction, float power)
    {
        if (_knocbacked)
            yield return null;

        _rb.AddForce(direction * power, ForceMode2D.Impulse);
        float time = 0;
        time += Time.deltaTime;
        _rb.velocity = new Vector2(_rb.velocity.x * _knocbackDistance.Evaluate(time), _rb.velocity.y * _knocbackHeight.Evaluate(time));
        yield return new WaitForSeconds(1);
        _rb.velocity = Vector2.zero;
    }
    public void Die()
    {
        int doDrop = Random.Range(0, 3);
        if (doDrop == 0)
        {
            int dropIndex = Random.Range(0, _drops.Length);
            Instantiate(_drops[dropIndex], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Move();
        WhileOnState();
    }
}

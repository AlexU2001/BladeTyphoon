using System.Collections;
using UnityEngine;
public class KnockbackEnemy : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private AnimationCurve _knocbackHeight;
    [SerializeField] private AnimationCurve _knocbackDistance;
    private bool _knockbacked = false;
    protected Rigidbody2D _rb;

    [Range(1f, 100f)]
    [SerializeField] private float power;
    [Range(1f, 100f)]
    [SerializeField] private float xCurvePower;
    [Range(1f, 100f)]
    [SerializeField] private float yCurvePower;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 200, 50), "Knock away"))
        {
            StartCoroutine(Knockback());
        }

        if (GUI.Button(new Rect(250, 20, 200, 50), "Reset Object"))
        {
            _rb.velocity = Vector2.zero;
            transform.position = Vector2.zero;
        }
    }

    private IEnumerator Knockback()
    {
        if (_knockbacked)
            yield return null;

        _knockbacked = true;
        _rb.AddForce(Vector2.right * power, ForceMode2D.Impulse);

        float xPower = xCurvePower;
        float yPower = yCurvePower;
        float time = 0f;
        float value = 0f;
        float duration = 2f;
        Vector2 originalVelocity = _rb.velocity;
        bool opposite = false;
        while (time <= duration)
        {
            xPower = xCurvePower * _knocbackHeight.Evaluate(value);
            yPower = yCurvePower * _knocbackHeight.Evaluate(value);
            time += Time.deltaTime;
            if (time < duration / 2)
            {
                value += Time.deltaTime;
            }
            else
            {
                value -= Time.deltaTime;
                xPower *= -1;
                yPower *= -1;
            }
            //Debug.Log("Velocity, X: " + _rb.velocity.x + ", Y: " + _rb.velocity.y);
            Debug.Log("Value: " + value + "X Power: " + xPower);
            //Debug.Log("Combined: " + _rb.velocity.x * xPower);
            _rb.velocity = new Vector2(originalVelocity.x + (xPower), originalVelocity.y + (yPower));
            yield return null;
        }
        _rb.velocity = Vector2.zero;
        _knockbacked = false;
    }
}

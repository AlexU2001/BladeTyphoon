using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _movement = Vector2.zero;
    [SerializeField] private float _speed = 5f;

    [Header("Combat")]
    [SerializeField] private WeaponSlot[] _slots;
    [SerializeField] private Weapon _startingWeapon;
    [SerializeField] float _speedMultiplier = 1.5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        Timer.Instance.TimerStarted += MultiplySpeed;
        Timer.Instance.TimerOver += ResetSpeed;
    }

    private void OnDisable()
    {
        Timer.Instance.TimerStarted -= MultiplySpeed;
        Timer.Instance.TimerOver -= ResetSpeed;
    }

    private void Start()
    {
        foreach (WeaponSlot slot in _slots)
            slot.ResetIndexes();

        AddWeapon(_startingWeapon, 0);
    }

    private void FixedUpdate()
    {
        foreach (var slot in _slots)
            slot.getSlot().RotateAround(transform.position, Vector3.forward, slot.getSpeed());
        _rb.velocity = _movement * _speed;
    }

    private void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    // Creates a weapon and places it at a spot, determined by a random index, at the edge of a circle
    public void AddWeapon(Weapon weapon, int slotIndex)
    {
        if (slotIndex < 0)
            return;

        WeaponSlot slot = _slots[slotIndex];

        if (slot.RemainingIndexes() < 0)
            return;

        int randomIndex = slot.AssignRandomIndex();
        int angle = randomIndex * (360 / slot.getMax());
        Vector3 pos = CircleEdge(slotIndex + 1, angle);
        Vector3 Euler = new Vector3(0, 0, -angle);

        Weapon weaponObj = Instantiate(weapon, slot.getSlot());

        Quaternion current = slot.getSlot().rotation;
        slot.getSlot().rotation = Quaternion.identity;

        weaponObj.transform.eulerAngles = Euler;
        weaponObj.transform.position = pos;

        slot.getSlot().rotation = current;

        //Debug.Log("Created weapon at " + slot.getSlot().name + " with index of " + randomIndex);
    }

    // Returns the position of the edge in a circle of x radius at angle y
    private Vector3 CircleEdge(float radius, int a)
    {
        Vector3 center = transform.position;
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
    private void MultiplySpeed(string tag)
    {
        if (!tag.Equals("SpeedUp"))
            return;
        StopAllCoroutines();
        //Debug.Log("Multiplying... * " + multiplier);
        foreach (var slot in _slots)
        {
            //Debug.Log("Expected results: " + slot.getSpeed() * multiplier);
            if (slot.getSpeed() <= (slot.getInitialSpeed() * _speedMultiplier * 10))
                slot.setSpeed(slot.getSpeed() * _speedMultiplier);
            //Debug.Log("Outcome: " + slot.getSpeed());
        }
    }

    private void ResetSpeed(string tag)
    {
        if (!tag.Equals("SpeedUp"))
            return;

        StartCoroutine(GradualSpeedLoss());

    }

    private IEnumerator GradualSpeedLoss()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (_slots[0].getSpeed() > _slots[0].getInitialSpeed())
        {
            foreach (var slot in _slots)
            {
                slot.setSpeed(slot.getSpeed() / _speedMultiplier);
            }
            yield return wait;
        }
        foreach (WeaponSlot slot in _slots)
        {
            slot.setSpeed(slot.getInitialSpeed());
        }
    }
}

[System.Serializable]
public class WeaponSlot
{
    [SerializeField] private Transform _slot;
    [SerializeField] private int _maxSpots;
    [SerializeField] private float _rotateSpeed;

    private float _initialSpeed;

    private Queue<int> _openIndexes = new Queue<int>();

    public WeaponSlot()
    {
        _maxSpots = 15;
        _rotateSpeed = 5f;
        _initialSpeed = _rotateSpeed;
    }

    public void ResetIndexes()
    {
        _initialSpeed = _rotateSpeed;
        List<int> indexList = new List<int>();
        for (int i = 0; i < _maxSpots; i++)
            indexList.Add(i);

        indexList.Shuffle();

        foreach (var item in indexList)
        {
            _openIndexes.Enqueue(item);
        }

    }

    public Transform getSlot() { return _slot; }

    public float getSpeed() { return _rotateSpeed; }
    public float getInitialSpeed() { return _initialSpeed; }
    public void setSpeed(float speed) { _rotateSpeed = speed; }

    public int RemainingIndexes() { return _openIndexes.Count; }

    public int AssignRandomIndex()
    {
        if (_openIndexes.Count <= 0)
            return -1;
        return _openIndexes.Dequeue();
    }

    public int getMax() { return _maxSpots; }
}

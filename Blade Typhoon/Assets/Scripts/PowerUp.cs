using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public abstract void OnSpawn();
    public abstract void OnPickUP();

    private void Start()
    {
        OnSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            OnPickUP();
    }
}

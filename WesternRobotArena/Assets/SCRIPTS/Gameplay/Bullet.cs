using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float lifeTime = 3f;

    private GameObject owner;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            transform.forward * speed * Time.deltaTime;
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void OnTriggerEnter(Collider other)
    {
        //stops the bullet hitting the player who fired it
        if (other.transform.root.gameObject == owner)
            return;

        Destroy(gameObject);
    }
}
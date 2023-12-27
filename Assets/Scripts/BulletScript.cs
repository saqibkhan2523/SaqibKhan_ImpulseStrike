using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float maxBulletDistance = 1000f;
    public float bulletSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);

        if(transform.position.x > maxBulletDistance || transform.position.y > maxBulletDistance || transform.position.z > maxBulletDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);
    }
}

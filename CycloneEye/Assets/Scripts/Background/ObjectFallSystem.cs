using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallSystem : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    [SerializeField] private bool isFalling = false;
    private Vector3 centerPos = Vector3.zero;

    private float counter = 0.0f;
    private float radius = 0.0f;
    private float velocity = 0.0f;
    private float multiplier = 1.0f;

    private Transform target;

    private Quaternion rot;

    public void Fall()
    {
        if (centerPos == Vector3.zero)
            centerPos = transform.position;
        Vector3 direction = target.position - transform.position;
        centerPos += direction.normalized * Time.deltaTime * velocity;

        counter += Time.deltaTime * multiplier;
        transform.position = centerPos + new Vector3(Mathf.Cos(counter) * radius, 0, Mathf.Sin(counter) * radius);

        if (velocity <= 10.0f)
            velocity += Time.deltaTime * 2;

        if (radius <= 6.0f)
        {
            radius += Time.deltaTime * 3;

        }
        else
        {
            multiplier += Time.deltaTime * 0.5f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            Fall();
            if ((target.position - transform.position).magnitude < 200)
            {
                Destroy(this.gameObject);
               
            }

            transform.rotation = Quaternion.Euler(90.0f, counter*100, 0.0f);
        }
    }

    void Start()
    {
        
        rot = transform.rotation;
        target = GameObject.Find("CoreCyclone").transform;
        int rdm = Random.Range(0, 75);
        if (rdm <= 1)
        {
            GetComponent<SpriteRenderer>().sprite = images[2];
        }
        if (rdm > 5 && rdm <= 25)
        {
            GetComponent<SpriteRenderer>().sprite = images[3];
        }
        if (rdm > 25 && rdm <= 50)
        {
            GetComponent<SpriteRenderer>().sprite = images[0];
        }
        if (rdm > 50 &&  rdm <=75)
        {
            GetComponent<SpriteRenderer>().sprite = images[1];
        }
       
        isFalling = true;
    }
}

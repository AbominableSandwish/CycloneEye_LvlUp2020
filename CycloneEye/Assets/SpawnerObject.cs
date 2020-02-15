using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObject : MonoBehaviour
{
    private float lastTimeToSpawn = 0.0f;
    private float timeToWait = 0.5f;

    [SerializeField] private GameObject obj;

    // Update is called once per frame
    void Update()
    {
        if (lastTimeToSpawn + timeToWait < Time.time)
        {
            if (Random.Range(0, 100) < 20)
            {
                lastTimeToSpawn = Time.time;
                float x = Random.Range(-4.0f , 2.0f);

                if (x < 0)
                {
                    x -= 2;
                }
                if (x > 0)
                {
                    x += 4;
                }

                float y = Random.Range(-4.0f, 4.0f);

                if (y < 0)
                {
                    y -= 5;
                }
                if (y > 0)
                {
                    y += 3;
                }

                Instantiate(obj, transform.position+ new Vector3(x, 0.0f, y), Quaternion.identity);
            }
            else
            {
                lastTimeToSpawn = Time.time;
            }
        }
    }
}

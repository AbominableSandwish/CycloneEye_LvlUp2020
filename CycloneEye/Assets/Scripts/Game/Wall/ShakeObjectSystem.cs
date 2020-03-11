using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObjectSystem : MonoBehaviour
{
    private Vector3 startingPos;
    private float startTime;

    private float speed = 40.0f;

    private float amount = 0.2f;
    // Start is called before the first frame update

    private bool isShake = false;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShake)
        {
            startTime+= Time.deltaTime;
            gameObject.transform.position = startingPos + Vector3.right * Mathf.Sin(startTime * speed) * amount;
            gameObject.transform.position = startingPos + Vector3.up * (Mathf.Sin(startTime * speed) * amount);
        }
        else
        {
            transform.position = startingPos;
        }
        
    }

    public void SetActiveShake()
    {
        startTime = 0;
        this.isShake = !this.isShake;
    }
}

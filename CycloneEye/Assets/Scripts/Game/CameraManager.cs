using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float shakeStrength = 0.1f;
    private Vector3 positionOffSet;

    void Start()
    {
        positionOffSet = transform.position;
        EventManager.onPlayerDamaged.AddListener(Chake);

        this.players = new List<Transform>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            print(player.name);
            this.players.Add(player.GetComponent<Transform>());
        }

    }

    private List<Transform> players;


    void FixedUpdate()
    {
        Debug.Log(PositionAveraging().magnitude);
        transform.position = positionOffSet + Vector3.down * PositionAveraging().magnitude;
    }

    Vector3 PositionAveraging()
    {
        Vector3 average = Vector3.zero;
        foreach (var player in this.players)
        {
            Debug.Log(player.localPosition);
            average += player.localPosition;
        }

        average /= this.players.Count;
        return average;
    }

    Vector3 LerpSmoothing()
    {
        return new Vector3();
    }

    void Chake()
    {
        StartCoroutine(ChakeAnim());
    }

    IEnumerator ChakeAnim()
    {
        Vector3 originPos = transform.position;
        for(int i = 0; i < 10; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 target = new Vector3(Random.Range(-1f, 1f) * shakeStrength, originPos.y, Random.Range(-1f, 1f) * shakeStrength);
            for(float t = 0; t < 0.01f; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, target, t * 100);
                yield return null;
            }
        }
        Vector3 startPos2 = transform.position;
        for (float t = 0; t < 0.01f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos2, originPos, t * 100);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float shakeStrength = 0.1f;
    private Vector3 centerStage;

    [SerializeField] private int nbrPlayer = 0;

    void Start()
    {
        centerStage = transform.position - Vector3.up*10;
        EventManager.onPlayerDamaged.AddListener(Chake);

        this.players = new List<Transform>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            this.players.Add(player.GetComponent<Transform>());
            nbrPlayer++;
        }

    }

    private List<Transform> players;


    void FixedUpdate()
    {
        transform.position = centerStage + Vector3.down * PositionAveraging().magnitude + Vector3.right * PositionAveraging().x + Vector3.forward * PositionAveraging().y;
        transform.position += Vector3.up * 10;
    }

    Vector3 PositionAveraging()
    {
        Vector3 average = Vector3.zero;
        foreach (var player in this.players)
        {
            average += player.localPosition;
        }

        average /= this.players.Count;
         
        return average;
    }

    public void RemovePlayer(PlayerController player)
    {
        this.players.Remove(player.transform);
        nbrPlayer--;
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(centerStage, 0.15f);
        if (players != null)
        {
            int i = 0;
            Vector3[] lerpPosition;
            if (players.Count == 2)
            {
                lerpPosition = new Vector3[2];


                foreach (var player in this.players)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(player.position, 0.15f);
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(centerStage, player.position);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(Vector3.Lerp(centerStage, player.position, 0.5f), 0.15f);
                    lerpPosition[i] = Vector3.Lerp(centerStage, player.position, 0.5f);
                    i++;
                }

                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[1], lerpPosition[0]);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Vector3.Lerp(lerpPosition[1], lerpPosition[0], 0.5f), 0.15f);
            }
        }
    }
}

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
        centerStage = transform.position - Vector3.up*4;
        EventManager.onPlayerDamaged.AddListener(Chake);
        EventManager.onWallDestroyed.AddListener(Chake);

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
        Vector3 average = PositionStageAverage();
        Debug.Log(average.magnitude);
        transform.position = centerStage + Vector3.up * PlayerMagnitude(average) + Vector3.right * average.x + Vector3.forward * average.z; //+Vector3.down * PlayerMagnitude()
    }

    float PlayerMagnitude(Vector3 center)
    {
        float average = 0.0f;  
        foreach (var player in this.players)
        {
                float result = (center - player.position).magnitude;
            if (result < 0)
            {
                result *= -1;
            }

            average += result;
        }

        average /= players.Count;
        

        return average;
    }

    Vector3 PlayerAverage()
    {
        Vector3 average = Vector3.zero;
        foreach (var player in this.players)
        {
            average += player.position;
        }

        average /= this.players.Count;

        return average;
    }

    public Vector3 PositionStageAverage()
    {
    int i = 0;
    Vector3[] lerpPosition;
    lerpPosition = new Vector3[players.Count];


    foreach (var player in this.players)
    {
        lerpPosition[i] = Vector3.Lerp(centerStage, player.position, 0.5f);
        i++;
    }

        Vector3 positionAverage= Vector3.zero;
        if (lerpPosition.Length == 2)
        {
            positionAverage = Vector3.Lerp(lerpPosition[0], lerpPosition[1], 0.5f);
        }
        if (lerpPosition.Length == 3)
        {
            Vector3[] lerpPosition2 = new Vector3[2];
            lerpPosition2[0] = Vector3.Lerp(lerpPosition[0], lerpPosition[1], 0.5f);
            lerpPosition2[1] = Vector3.Lerp(lerpPosition[1], lerpPosition[2], 0.5f);
            positionAverage = Vector3.Lerp(lerpPosition2[0], lerpPosition2[1], 0.5f);
        }
        if (lerpPosition.Length == 4)
        {
            Vector3[] lerpPosition2 = new Vector3[2];

            lerpPosition2[0] = Vector3.Lerp(lerpPosition[0], lerpPosition[2], 0.5f);
            lerpPosition2[1] = Vector3.Lerp(lerpPosition[1], lerpPosition[3], 0.5f);

            positionAverage = Vector3.Lerp(lerpPosition2[0], lerpPosition2[1], 0.5f);
        }

        return positionAverage;
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
                lerpPosition = new Vector3[players.Count];


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

            if (lerpPosition.Length == 2)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[0], lerpPosition[1]);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Vector3.Lerp(lerpPosition[0], lerpPosition[1], 0.5f),0.15f);
            }
            if (lerpPosition.Length == 3)
            {
                Vector3[] lerpPosition2 = new Vector3[2];

                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[0], lerpPosition[1]);
                Gizmos.color = Color.red;
                lerpPosition2[0] = Vector3.Lerp(lerpPosition[0], lerpPosition[1], 0.5f);
                Gizmos.DrawSphere(lerpPosition2[0], 0.15f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[1], lerpPosition[2]);
                Gizmos.color = Color.red;
                lerpPosition2[1] = Vector3.Lerp(lerpPosition[1], lerpPosition[2], 0.5f);
                Gizmos.DrawSphere(lerpPosition2[1], 0.15f);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition2[0], lerpPosition2[1]);
                Gizmos.color = Color.magenta;
                Vector3 center= Vector3.Lerp(lerpPosition2[0], lerpPosition2[1], 0.5f);
                Gizmos.DrawSphere(center, 0.15f);
            }
            if (lerpPosition.Length == 4)
            {
                Vector3[] lerpPosition2 = new Vector3[2];

                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[0], lerpPosition[2]);
                Gizmos.color = Color.red;
                lerpPosition2[0] = Vector3.Lerp(lerpPosition[0], lerpPosition[2], 0.5f);
                Gizmos.DrawSphere(lerpPosition2[0], 0.15f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition[1], lerpPosition[3]);
                Gizmos.color = Color.red;
                lerpPosition2[1] = Vector3.Lerp(lerpPosition[1], lerpPosition[3], 0.5f);
                Gizmos.DrawSphere(lerpPosition2[1], 0.15f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lerpPosition2[0], lerpPosition2[1]);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Vector3.Lerp(lerpPosition2[0], lerpPosition2[1], 0.5f), 0.15f);
            }
        }
              
        }
    }

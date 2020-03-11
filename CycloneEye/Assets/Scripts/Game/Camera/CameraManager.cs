using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    float shakeStrength = 2.0f;
    private Vector3 centerStage;


    private float multiplier = 1.8f;
    [SerializeField] private const float MIN_DISTANCE = 3.0f;

    [SerializeField] private float strength = 5.0f;
    private int idPlayerDistanceMax;
    private List<Transform> players;
    bool isShaking = false;
    private Vector3 distanceCamera;


    void Start()
    {
        centerStage = transform.position - Vector3.up*10;
        EventManager.onPlayerDamaged.AddListener(Chake);

        this.players = new List<Transform>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            this.players.Add(player.GetComponent<Transform>());
        }

    }
    void FixedUpdate()
    {
        Vector3 average = PositionStageAverage();
        distanceCamera = Vector3.up * (PlayerMagnitude(centerStage) * multiplier);

           
        Vector3 target = centerStage + distanceCamera + Vector3.right * average.x + Vector3.forward * average.z; //+Vector3.down * PlayerMagnitude()
        transform.position = LerpSmoothing(target);
    }

    float PlayerMagnitude(Vector3 center)
    {
        float distance = 0.0f;
        foreach (var player in this.players)
        {
            float magnitude = (player.position - center).magnitude;
            if (magnitude > distance)
            {
                idPlayerDistanceMax = player.GetComponent<PlayerController>().Index;
                distance = magnitude;
            }
        }


        if (distance < MIN_DISTANCE)
        {
            distance = MIN_DISTANCE;
        }

        return distance;
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
    }

    Vector3 LerpSmoothing(Vector3 targetPosition)
    {
        return Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);
    }

    void Chake()
    {
        if (!isShaking)
        {
            isShaking = true;
            StartCoroutine(ChakeAnim());
        }
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
                transform.position = Vector3.Lerp(startPos, target, t * strength);
                yield return null;
            }
        }
        Vector3 startPos2 = transform.position;
        for (float t = 0; t < 0.01f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos2, originPos, t * strength);
            yield return null;
        }
        isShaking = false;
    }

    void OnDrawGizmos()
    {
        if (players != null)
        {
            int i = 0;
            Vector3[] lerpPosition;
            lerpPosition = new Vector3[players.Count];


            foreach (var player in this.players)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(player.position, 0.15f);
                if (player.GetComponent<PlayerController>().Index != idPlayerDistanceMax)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

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
                Gizmos.DrawSphere(Vector3.Lerp(lerpPosition[0], lerpPosition[1], 0.5f), 0.15f);
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
                Vector3 center = Vector3.Lerp(lerpPosition2[0], lerpPosition2[1], 0.5f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackManager : MonoBehaviour {


    class GizmoEjection
    {
        private Vector3 playerPositon;
        private Vector3 baseForce;
        private float power;
        private Vector3 endPosition;

        private float timeStart;

        public GizmoEjection(Color color ,Vector3 playerPositon, Vector3 baseForce, float power, Vector3 endPosition)
        {
            this.playerPositon = playerPositon;
            this.baseForce = baseForce;
            this.power = power;
            this.endPosition = endPosition;
            timeStart = Time.time;
        }

        public void Draw()
        {
            Gizmos.DrawSphere(this.playerPositon, 0.2f);
            Gizmos.DrawSphere(this.endPosition, 0.2f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPlayerEjectionWay(Vector3 playerPositon, RaycastHit hit, Vector3 direction, float force, float stopDist)
    {

        //this.playerPositon = playerPositon;
        //this.endPosition = hit.point + direction * force / 20.0f;
    }

    void OnDrawGizmos()
    {
        //if (this.playerPositon != null)
        //{
        //    Gizmos.DrawSphere(this.playerPositon, 0.2f);
        //    Gizmos.DrawSphere(this.endPosition, 0.2f);
        //}
    }
}

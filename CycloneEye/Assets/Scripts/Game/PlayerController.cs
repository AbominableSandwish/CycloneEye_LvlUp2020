using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    NORMAL, ATTACKING, PUSHED, KO
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject attackAnim;
    [SerializeField] private GameObject Trace;

    private Text scoreText;
    private Animator scoreAnimator;

    Animator anim;

    float damages;
    Rigidbody rBody;
    [SerializeField] PlayerState state;
    float chargingAttack;
    bool charging = false;

    public PlayerState State { get { return state; } set { state = value; } }
    public float Damages { get { return damages; } }

    public void StopPush()
    {
        rBody.velocity = Vector3.zero;
        state = PlayerState.NORMAL;
    }

    // Start is called before the first frame update
    void Start()
    {
        charging = false;
        state = PlayerState.NORMAL;
        anim = GetComponentInChildren<Animator>();
        rBody = GetComponent<Rigidbody>();
        scoreText = GameObject.Find("TextScore" + index).GetComponent<Text>();
        scoreAnimator = scoreText.gameObject.GetComponent<Animator>();
    }

    float NextimeToSpawnTrace = 0.0f;

    void FixedUpdate()
    {
        if (state == PlayerState.KO)
        {
            Fall();
            if (NextimeToSpawnTrace <= Time.time)
            {
                NextimeToSpawnTrace = Time.deltaTime + 0.0f;

                Instantiate(Trace, transform.position, transform.localRotation, GameObject.Find("Stage").transform);
            }
            return;
        }

        if (state == PlayerState.PUSHED)
        {
            if (NextimeToSpawnTrace <= Time.time)
            {
                NextimeToSpawnTrace = Time.deltaTime + 0.01f;

                Instantiate(Trace, transform.position, transform.localRotation, GameObject.Find("Stage").transform);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetButtonDown("Start " + index))
            GameManager.Instance.Pause(index);

        if (GameManager.State != GameState.PLAYING || state != PlayerState.NORMAL) return;

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal " + index), 0, Input.GetAxis("Vertical " + index)) * moveSpeed;
        if (charging) movement /= 3;
        rBody.AddForce(movement);
        if (rBody.velocity.magnitude > moveSpeed)
            rBody.velocity = rBody.velocity.normalized * moveSpeed;

        if (!charging)
            anim.speed = movement.magnitude/moveSpeed;
        else
            anim.speed = 1;

        if (movement.magnitude > 0)
        {
            anim.SetBool("walking", true);
            transform.LookAt(transform.position+movement);
        }
        else { 
            anim.SetBool("walking", false);
        }

        if (Input.GetButtonDown("Attack " + index))
        {
            charging = true;
            chargingAttack = 0;
            anim.SetBool("charging", true);
        }
        if (Input.GetButton("Attack " + index) && charging)
        {
            chargingAttack = Mathf.Min(1f, chargingAttack+=Time.deltaTime);
        }
        if (Input.GetButtonUp("Attack " + index) && charging)
        {
            charging = false;
            anim.SetBool("charging", false);
            StartCoroutine(AttackAnim());
        }
    }


    IEnumerator AttackAnim()
    {
        Instantiate(attackAnim, transform.position + transform.forward*0.2f + transform.right*0.1f, transform.rotation);
        state = PlayerState.ATTACKING;
        rBody.velocity = Vector3.zero;
        if(chargingAttack < 0.333f)
        yield return new WaitForSeconds(.333f- chargingAttack);
        TestAttackPropultion();
        yield return new WaitForSeconds(.5f);
        state = PlayerState.NORMAL;
    }

    void TestAttackPropultion()
    {
        Collider[] colls = Physics.OverlapBox(transform.position + transform.forward * 0.2f + transform.right * 0.1f, new Vector3(.5f, .5f, .5f));
        foreach(Collider coll in colls)
        {
            if (coll.tag == "Player" && coll.gameObject != this.gameObject)
            {
                Vector3 direction = (coll.transform.position - transform.position).normalized;
                coll.GetComponent<PlayerController>().Push(direction, 1+chargingAttack*20);
            }
        }
    }

    public void Push(Vector3 baseForce, float power)
    {
        transform.LookAt(transform.position - baseForce);
        anim.SetTrigger("pushed");
        anim.SetBool("walking", false);
        anim.SetBool("charging", false);
        charging = false;
        EventManager.onPlayerDamaged.Invoke();
        damages += power;
        rBody.AddForce(baseForce * Mathf.Pow(damages *10, 1.1f));
        StartCoroutine(PushAnim());
        scoreText.text = ((int) damages).ToString();
        scoreAnimator.SetTrigger("TakeDamage");
    }

    IEnumerator PushAnim()
    {
        state = PlayerState.PUSHED;
        yield return new WaitForSeconds(0.4f);
        if(state != PlayerState.KO)
            state = PlayerState.NORMAL;
    }

    public void Eliminate()
    {
        EventManager.onPlayerEliminated.Invoke();
    }

    private Vector3 centerPos = Vector3.zero;

    private float counter = 0.0f;
    private float radius = 0.0f;
    private float velocity = 0.0f;
    private float multiplier = 1.0f;

    public void Fall()
    {
        if (centerPos == Vector3.zero)
            centerPos = transform.position;
        Vector3 direction = GameObject.Find("CoreCyclone").transform.position - transform.position;
        centerPos += direction.normalized * Time.deltaTime * velocity;

        counter += Time.deltaTime * multiplier;
        transform.position = centerPos + new Vector3(Mathf.Cos(counter)*radius, 0, Mathf.Sin(counter)*radius);

        if (velocity <= 10.0f)
            velocity += Time.deltaTime * 2;

        if (radius <= 6.0f)
        {
            radius += Time.deltaTime * 3;
           
        }
        else
        {
            multiplier += Time.deltaTime*0.5f;
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    NORMAL, ATTACKING, PUSHED, KO, GUARDING, LOCK
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject attackAnim;
    [SerializeField] private GameObject Trace;
    float damageEjection = 0;
    private float powerEjection = 1f;
    [SerializeField] float dashForce = 1000;

    private Text scoreText;
    private Text damageText;
    private Animator scoreAnimator;
    private Animator damageAnimator;

    Animator anim;

    float damages;
    Rigidbody rBody;
    [SerializeField] PlayerState state;
    float chargingAttack;
    bool charging = false;
    bool guarding = false;

    public bool eliminated = false;

    public PlayerState State { get { return state; } set { state = value; } }
    public float Damages { get { return damages; } }
    public int Index { get { return index; } }

    private MotherFuckingAudioManager audioManager;
    private FeedBackManager gizmosManager;
    private SpriteRenderer renderer;
    [SerializeField] private Material normal;
    [SerializeField] private Material hurt;

    public void StopPush()
    {
        rBody.velocity = Vector3.zero;
        state = PlayerState.NORMAL;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
        charging = false;
        state = PlayerState.NORMAL;
        anim = GetComponentInChildren<Animator>();
        rBody = GetComponent<Rigidbody>();
        damageText = GameObject.Find("TextDamages" + index).GetComponent<Text>();
        damageAnimator = damageText.gameObject.GetComponent<Animator>();
        scoreText = GameObject.Find("TextScore" + index).GetComponent<Text>();
        scoreAnimator = scoreText.gameObject.GetComponent<Animator>();
        scoreText.text = ScoreManager.FinalScore(index - 1).ToString("00");
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    float NextimeToSpawnTrace = 0.0f;

    void FixedUpdate()
    {
        if (state == PlayerState.KO)
        {
            Fall();
            if (NextimeToSpawnTrace <= Time.time)
            {
                NextimeToSpawnTrace = Time.deltaTime + 0.05f;
                GameObject newTrace = Instantiate(Trace, transform.position, transform.GetComponentInChildren<SpriteRenderer>().transform.rotation, GameObject.Find("Stage").transform);
                newTrace.GetComponent<SystemTrace>().source = transform.GetComponentInChildren<SpriteRenderer>();
            }
            return;
        }

        if (state == PlayerState.PUSHED || state == PlayerState.ATTACKING)
        {
            if (NextimeToSpawnTrace <= Time.time)
            {
                NextimeToSpawnTrace = Time.deltaTime + 0.005f;

                GameObject newTrace = Instantiate(Trace, transform.position, transform.GetComponentInChildren<SpriteRenderer>().transform.rotation, GameObject.Find("Stage").transform);
                newTrace.GetComponent<SystemTrace>().source = transform.GetComponentInChildren<SpriteRenderer>();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        int controllerId = GameManager.playerOrder[index - 1];

        if (Input.GetButtonDown("Start " + controllerId))
            GameManager.Instance.Pause(index);

        if (GameManager.State != GameState.PLAYING || (state != PlayerState.NORMAL && state != PlayerState.GUARDING)) return;

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal " + controllerId), 0, Input.GetAxis("Vertical " + controllerId)) * moveSpeed;
        if (charging) movement /= 2;
        if (guarding) movement *= 0;
        rBody.AddForce(movement);
        if (rBody.velocity.magnitude > moveSpeed)
            rBody.velocity = rBody.velocity.normalized * moveSpeed;

        if (!charging && !guarding)
            anim.speed = movement.magnitude / moveSpeed;
        else
            anim.speed = 1;

        if (movement.magnitude > 0)
        {
            anim.SetBool("walking", true);
            if (!charging && !guarding)
                transform.LookAt(transform.position + movement);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if (!charging)
        {
            if (Input.GetButtonDown("Guard " + controllerId))
            {
                state = PlayerState.GUARDING;
                guarding = true;
                anim.SetBool("guarding", true);
            }
            if (Input.GetButtonUp("Guard " + controllerId) && guarding)
            {
                state = PlayerState.NORMAL;
                guarding = false;
                anim.SetBool("guarding", false);
            }
        }

        if (!guarding)
        {
            if (Input.GetButtonDown("Attack " + controllerId))
            {
                charging = true;
                chargingAttack = 0;
                anim.SetBool("charging", true);
            }
            if (Input.GetButton("Attack " + controllerId) && charging)
            {
                chargingAttack = Mathf.Min(1f, chargingAttack + Time.deltaTime);
            }
            if (Input.GetButtonUp("Attack " + controllerId) && charging)
            {
                StartCoroutine(AttackAnim());
            }
        }
    }

    IEnumerator ChargeAnim(Vector3 direction, float force, float stopDist)
    {
        Vector3 StartPosition = transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        RaycastHit hit;
        Vector3 EndPoint;

        if (Physics.Raycast(transform.position, direction, out hit, force))
        {
            EndPoint = hit.point - direction * stopDist;
            if (GameObject.Find("Game Manager").GetComponent<FeedBackManager>() != null)
                GameObject.Find("Game Manager").GetComponent<FeedBackManager>().ShowPlayerEjectionWay(StartPosition, hit, direction, force, stopDist);
        }
        else
        {
            EndPoint = transform.position + direction * force;
            if (GameObject.Find("Game Manager").GetComponent<FeedBackManager>() != null)
                GameObject.Find("Game Manager").GetComponent<FeedBackManager>().ShowPlayerEjectionWay(StartPosition, hit, direction, force, stopDist);
        }

    

        Vector3 startPos = transform.position;
        for(float t = 0; t <= 0.1f; t += Time.deltaTime)
        {
            if (Physics.Raycast(transform.position, direction, out hit, force))
            {
                EndPoint = hit.point - direction * stopDist;
            }

            transform.position = Vector3.Lerp(startPos, EndPoint, t * 10);
           
            yield return null;
        }
        transform.position = EndPoint;

        /*
        float remainingDist = Mathf.Abs(force - Vector3.Distance(transform.position, startPos));
        if (remainingDist > 1f)
        {
            Vector3 bounced = -direction;
            yield return StartCoroutine(ChargeAnim(bounced, remainingDist, stopDist));
        }
        */
    }

    IEnumerator AttackAnim()
    {

        state = PlayerState.ATTACKING;
        if (chargingAttack < 0.12f)
        {
            yield return new WaitForSeconds(.12f - chargingAttack);
        }
        StartCoroutine(ChargeAnim(transform.forward,dashForce * chargingAttack, 0.3f));
        Instantiate(attackAnim, transform.position + transform.forward * 0.2f + transform.right * 0.1f, transform.rotation);
        yield return new WaitForSeconds(.1f);
        rBody.velocity = Vector3.zero;
        TestAttackPropultion();
        yield return new WaitForSeconds(.7f);
        state = PlayerState.NORMAL;
    }

    float modifier = 1f;
    void TestAttackPropultion()
    {
        Vector3 direction = Vector3.zero;
        Collider[] colls = Physics.OverlapBox(transform.position + transform.forward * 0.2f + transform.right * 0.1f, new Vector3(.5f, .5f, .5f), transform.rotation);
        bool blocked = false;
        bool hitplayer = false;
        foreach (Collider coll in colls)
        {
            if (coll.tag == "Player" && coll.gameObject != this.gameObject)
            {
                hitplayer = true;
                direction = (coll.transform.position - transform.position).normalized;
                blocked = blocked || coll.GetComponent<PlayerController>().Push(direction, this.powerEjection, index);

            }
        }
        charging = false;
        anim.SetBool("charging", false);
        anim.SetBool("attack_blocked", blocked);

        if (blocked)
        {
            audioManager.PlaySound(MotherFuckingAudioManager.SoundList.PARADE);
            StartCoroutine(CounterMalusEmplification());
            StartCoroutine(ChargeAnim(-direction, 1f, 0f));
        }
        else
        {
            if (!hitplayer)
            {
                audioManager.PlaySound(MotherFuckingAudioManager.SoundList.WOOSH);
            }
            else
            {
                audioManager.PlaySound(MotherFuckingAudioManager.SoundList.HIT);
            }
        }
    }

    IEnumerator CounterMalusEmplification()
    {
        modifier = 5f;
        yield return new WaitForSeconds(0.2f);
        modifier = 1f;

    }

    public int pusher = -1;
    public bool Push(Vector3 baseForce, float power, int pusherIndex)
    {
        pusher = pusherIndex;
        bool guarded = false;
        if (state == PlayerState.GUARDING)
        {
            if (Vector3.Angle(-transform.forward, baseForce) < 50f)
            {
                // IF GUARDED
                guarded = true;
                baseForce = baseForce / 5;
                power *= .2f;
                anim.SetTrigger("guard_hit");
                StartCoroutine(GuardAnim());
            }
            else
            {
                // IF GUARD FAILD
                guarded = false;
                guarding = false;
                anim.SetBool("guarding", false);
                transform.LookAt(transform.position - baseForce);
                anim.SetTrigger("pushed");
                StartCoroutine(PushAnim());
            }
        }
        else if (state == PlayerState.ATTACKING)
        {
            if (Vector3.Angle(-transform.forward, baseForce) < 30f)
            {
                audioManager.PlaySound(MotherFuckingAudioManager.SoundList.PARADE);
                guarded = true;
                baseForce = baseForce / 2;
                power *= .2f;
                anim.SetTrigger("attack_blocked");
                StartCoroutine(ClashAnim());
            }
            else
            {
                //float factor = (power / (1 + chargingAttack * 20)) / 2;
               // baseForce = factor * baseForce / 3;
                power *= 0.3f;

                // StartCoroutine(ClashAnim());
                anim.SetTrigger("pushed");
                StartCoroutine(PushAnim());
                this.damageEjection += power * 20.0f;
                StartCoroutine(ChargeAnim(baseForce, power + (1 * (this.damageEjection / 100.0f)), 0.0f));
            }
        }
        else
        {
            transform.LookAt(transform.position - baseForce);
            anim.SetTrigger("pushed");
            StartCoroutine(PushAnim());
        }
        anim.SetBool("walking", false);
        anim.SetBool("charging", false);
        charging = false;

        EventManager.onPlayerDamaged.Invoke();
        if (!guarded)
        {
            this.damageEjection += power * 20.0f;
            StartCoroutine(ChargeAnim(baseForce, power + (1 * (this.damageEjection/100.0f)), 0.0f));
        }

        damageText.text = ((int) this.damageEjection/2).ToString("0");
        damageAnimator.SetTrigger("TakeDamage");
        return guarded;
    }

    IEnumerator GuardAnim()
    {
        state = PlayerState.LOCK;
        yield return new WaitForSeconds(0.3f);
        if (state != PlayerState.KO)
        {
            pusher = -1;

            if (Input.GetButton("Guard " + GameManager.playerOrder[index - 1]))
            {
                anim.SetBool("guarding", true);
                guarding = true;
                state = PlayerState.GUARDING;
            }
            else
            {
                anim.SetBool("guarding", false);
                guarding = false;
                state = PlayerState.NORMAL;
            }
        }
    }

    IEnumerator ClashAnim()
    {
        yield return new WaitForSeconds(0.8f);
        if (state != PlayerState.KO)
        {
            pusher = -1;
            state = PlayerState.NORMAL;
        }
    }

    IEnumerator PushAnim()
    {
        state = PlayerState.PUSHED;
        StartCoroutine(Blink());
        yield return new WaitForSeconds(0.4f);
        if (state != PlayerState.KO)
        {
            pusher = -1;
            state = PlayerState.NORMAL;
        }
    }

    IEnumerator Blink()
    {
        renderer.material = hurt;
        yield return new WaitForSeconds(0.15f);
        renderer.material = normal;
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

    public void ChangePointsAnim(int value)
    {
        scoreText.text = ScoreManager.FinalScore(index - 1).ToString("00");
        if (value > 0)
        scoreAnimator.SetTrigger("score_up");
        else
        scoreAnimator.SetTrigger("score_down");
    }



}

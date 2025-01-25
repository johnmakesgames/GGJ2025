using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Collider2D bubbleDetector;
    Rigidbody2D rigidbody;
    [SerializeField] private GameObject playerCamera;

    [Header("Player")]
    public bool CanRotate;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxRotationAngle;
    [Space(10)]
    [Header("Bubble")]
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private GameObject bubbleSpriteObject;
    [SerializeField] 
    private float bubbleSize { get; set; } = 0;
    [SerializeField] 
    private float inflatingSpeedScale;
    [SerializeField] 
    private int bubbleForceMultiplier;

    private float inputDelay = 0.1f;
    private float timeSinceInput = 0;

    public float BubbleRechargeTimeSeconds;
    private float BubbleRechargeTimer = 0.0f;

    public float MaxBubbleSize { get; set; } = 2.5f;
    public int MaxBubbleCount { get; set; } = 5;
    private int bubbleCount { get; set; }

    private bool waitingForBubbleKeyLift;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbleDetector = GetComponentInChildren<Collider2D> ();
        rigidbody = GetComponent<Rigidbody2D>();
        bubbleSize = 0;
        bubbleCount = 3;
        waitingForBubbleKeyLift = false;
        BubbleRechargeTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.SetPositionAndRotation(transform.position + new Vector3(0.0f, 0.0f, -10.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));

        float normalisedSize = bubbleSize / MaxBubbleSize;

        timeSinceInput += Time.deltaTime;
        BubbleRechargeTimer += Time.deltaTime;

        if(BubbleRechargeTimer > BubbleRechargeTimeSeconds)
        {
            AddBubble(1);
            BubbleRechargeTimer = 0.0f;
        }

        //Inflating bubble
        if (waitingForBubbleKeyLift == false && bubbleCount > 0)
        {
            if (Input.GetKey(KeyCode.Space) && (timeSinceInput > inputDelay))
            {
                //Inflating Bubble
                bubbleSize += (animationCurve.Evaluate(normalisedSize) * inflatingSpeedScale * Time.deltaTime);

                if (normalisedSize >= 1)
                {
                    // POP
                    rigidbody.AddForceY(MaxBubbleSize * -10000 * Time.deltaTime);
                    bubbleSize = 0;
                    timeSinceInput = Mathf.Min(timeSinceInput, 0.0f);
                    waitingForBubbleKeyLift = true;
                }
            }
        }

        //Stop inflation (cc: Bank of England)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            bubbleSize = 0;
            timeSinceInput = Mathf.Min(timeSinceInput, 0);
            waitingForBubbleKeyLift = false;
            RemoveBubble(1);
        }

        bubbleSpriteObject.GetComponent<Transform>().localScale = new Vector3(normalisedSize * 2, normalisedSize * 2, normalisedSize * 2);

        Debug.Log("Bubbles left: " + bubbleCount + "/" + MaxBubbleCount);
        Debug.Log("Bubble size: " + bubbleSize);

        if (Application.isEditor)
        {
            if(Input.GetKeyDown("o"))
            {
                AddBubble(1);
            }

            if (Input.GetKeyDown("p"))
            {
                RemoveBubble(1);
            }
        }
    }

    private void AddBubble(int count)
    {
        bubbleCount += count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, MaxBubbleCount);
    }

    public void RemoveBubble(int count)
    {
        bubbleCount -= count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, MaxBubbleCount);
    }

    private void FixedUpdate()
    {
        if (CanRotate)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rigidbody.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * +1.0f);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rigidbody.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * -1.0f);
            }

            Vector3 angles = transform.eulerAngles;
            if (angles.z > 180.0f)
            {
                angles.z -= 360.0f;
            }
            angles.z = Mathf.Clamp(angles.z, -maxRotationAngle, maxRotationAngle);
            transform.eulerAngles = angles;
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if (bubbleSize > 0)
        {
            Vector2 direction = this.transform.up.normalized;
            rigidbody.AddForce(direction * bubbleForceMultiplier * bubbleSize * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
          if (collider.gameObject.tag == "ResetBubble")
        {
            AddBubble(1);
            Destroy(collider.gameObject);
        }
    }
}


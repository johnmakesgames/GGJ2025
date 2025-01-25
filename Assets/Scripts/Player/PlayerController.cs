using System;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

    public PlayerStats PlayerInfo;
    private UiManager BubbleUIContainer;

    private bool grounded;

    [Space(10)]
    [Header("Shooting")]
    private bool canExtendTongue;
    private bool tongueGoingRight = false;
    private bool isTongueExtended = false;
    [SerializeField] 
    private AnimationCurve TongueLengthCurve;
    private float tongueLength;
    [SerializeField] 
    private float tongueExtensionSpeed;
    [SerializeField]
    private float maxTongueLength;
    [SerializeField]
    private float tongueScale;
    [SerializeField]
    private Transform mouthTransform;
    [SerializeField]
    private Transform tongueEndPosition;
    [SerializeField]
    private GameObject tongueSprite;

    [Header("Debugs")]
    [SerializeField] private bool bubbleDetailLog;
    [SerializeField] private bool tongueDetailLog;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbleDetector = GetComponentInChildren<Collider2D> ();
        rigidbody = GetComponent<Rigidbody2D>();
        bubbleSize = 0;
        bubbleCount = 3;
        waitingForBubbleKeyLift = false;
        BubbleRechargeTimer = 0.0f;
        PlayerInfo = GetComponent<PlayerStats>();
        BubbleUIContainer = GameObject.FindGameObjectWithTag("BubbleUIContainer").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.SetPositionAndRotation(transform.position + new Vector3(0.0f, 0.0f, -10.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f));

        float normalisedSize = bubbleSize / (MaxBubbleSize + PlayerInfo.MaxBubbleSizeMod);

        timeSinceInput += Time.deltaTime;
        BubbleRechargeTimer += Time.deltaTime;

        if(BubbleRechargeTimer > (BubbleRechargeTimeSeconds + PlayerInfo.BubbleRechargeTimeMod))
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
                bubbleSize += (animationCurve.Evaluate(normalisedSize) * (inflatingSpeedScale + PlayerInfo.InflatingSpeedMod) * Time.deltaTime);

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

        if (bubbleDetailLog)
        {
            Debug.Log("Bubbles left: " + bubbleCount + "/" + (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));
            Debug.Log("Bubble size: " + bubbleSize);
        }


        //Tongue Controls
        if (Input.GetKeyDown(KeyCode.E) && canExtendTongue)
        {
            isTongueExtended = true;
            canExtendTongue = false;
            tongueGoingRight = true;
        }

        if (Input.GetKeyDown(KeyCode.Q) && canExtendTongue)
        {
            isTongueExtended = true;
            canExtendTongue = false;
            tongueGoingRight = false;
        }

        float normalisedTongueLength = tongueLength / maxTongueLength;
        if (isTongueExtended)
        {
            tongueLength += TongueLengthCurve.Evaluate(normalisedTongueLength) * tongueExtensionSpeed * Time.deltaTime;
        }
        else
        {
            tongueLength -= TongueLengthCurve.Evaluate(normalisedTongueLength) * tongueExtensionSpeed * Time.deltaTime;
        }

        if(canExtendTongue == false)
        {
            if(tongueLength <= 0)
            {
                canExtendTongue = true;
            }
            else if(tongueLength >= maxTongueLength)
            {
                canExtendTongue = false;
            }
        }

        //Direction tongue is facing (l/r)
        Vector3 tongueDirection = Vector3.zero;
        if (tongueGoingRight)
        {
            tongueDirection = Vector3.right;
        }
        else
        {
            tongueDirection = Vector3.right * -1.0f;
        }

        tongueLength = Mathf.Min(tongueLength, maxTongueLength);
        tongueLength = Mathf.Max(0, tongueLength);
        //Calculate current length of the tongue.
        float length = tongueLength * tongueScale;

        if (length > 0)
        {
            //Calculate tongue mid and end points.
            Vector2 midPoint = tongueDirection * (length / 2.0f);
            Vector2 endPosition = tongueDirection * length;

            tongueSprite.transform.localPosition = midPoint;
            tongueEndPosition.transform.localPosition = endPosition;
        }
        else
        {
            tongueSprite.transform.localPosition = mouthTransform.localPosition;
            tongueEndPosition.transform.localPosition = mouthTransform.localPosition;
        }

        //tongueSprite.transform.localScale = Vector3.one;
        //tongueSprite.transform.localRotation = Quaternion.identity;

        Vector3 scale = tongueSprite.transform.localScale;
        scale.x = length;
        tongueSprite.transform.localScale = scale;


        if (Input.GetKeyUp(KeyCode.E))
        {
            isTongueExtended = false;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isTongueExtended = false;
        }

        if (tongueDetailLog)
        {
            Debug.Log("Tongue Length: " + tongueLength);
            Debug.Log("Normalised Tongue Length: " + normalisedTongueLength);
        }


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
        int originalCount = bubbleCount;

        bubbleCount += count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));
        
        BubbleUIContainer.AddBubble(bubbleCount - originalCount);
    }

    public void RemoveBubble(int count)
    {
        int originalCount = bubbleCount;

        bubbleCount -= count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));

        BubbleUIContainer.RemoveBubble(originalCount - bubbleCount);
    }

    public int GetBubbleCount()
    {
        return bubbleCount;
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

        if (grounded)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rigidbody.AddForce(new Vector2(-1000 * Time.deltaTime, 0));
            }

            if (Input.GetKey(KeyCode.D))
            {
                rigidbody.AddForce(new Vector2(1000 * Time.deltaTime, 0));
            }

            grounded = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Refill to max bubbles when the player lands.
        if (collision.gameObject.CompareTag("JohnTestGround"))
        {
            if (collision.transform.position.y < this.transform.position.y)
            {
                int missingBubbles = (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod) - bubbleCount;
                AddBubble(missingBubbles);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Refill to max bubbles when the player lands.
        if (collision.gameObject.CompareTag("JohnTestGround"))
        {
            if (collision.transform.position.y < this.transform.position.y)
            {
                grounded = true;
            }
        }
    }
}


using System;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Collider2D bubbleDetector;
    Rigidbody2D rigidbody;

    [Header("Player Movement")]
    public bool CanRotate;
    private bool isRotating;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxRadiansDelta;
    [SerializeField]
    private float maxMagnitudeDelta;
    [SerializeField]
    private float maxRotationAngle;
    private Vector2 targetRotationDirection;
    [SerializeField]
    private float WalkingSpeed;
    private bool isFacingRight;
    private float WalkCycleLength;
    private float WalkCycleTimer;
    private bool isWalking;
    [SerializeField]
    private Transform leftFootTransform;
    [SerializeField]
    private Transform rightFootTransform;
    [SerializeField]
    private float groundCheckDistance;


    [Space(10)]
    [Header("Bubble Movement")]
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
    public int MaxBubbleCount { get; set; } = 3;
    private int bubbleCount { get; set; }

    private bool waitingForBubbleKeyLift;

    [Header("Player Data")]
    [HideInInspector] public PlayerStats PlayerInfo;
    private UiManager HUDUIContainer;
    private bool isGrounded;
    [SerializeField]
    Animator spriteAnimator;

    [Space(10)]
    [Header("Shooting")]
    [SerializeField]
    private GameObject crosshairSprite;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    private int fireForceStrength;
    [SerializeField]
    private int startingAmmoCount;
    public int MaxAmmoCount { get; set; } = 3;
    private int ammoCount { get; set; }
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
    [SerializeField] private bool ammoDetailLog;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbleDetector = GetComponentInChildren<Collider2D> ();
        rigidbody = GetComponent<Rigidbody2D>();
        bubbleSize = 0;
        ammoCount = 0;
        bubbleCount = MaxBubbleCount;
        waitingForBubbleKeyLift = false;
        BubbleRechargeTimer = 0.0f;
        PlayerInfo = GetComponent<PlayerStats>();
        PlayerInfo.LoadStats();
        HUDUIContainer = GameObject.FindGameObjectWithTag("BubbleUIContainer").GetComponent<UiManager>();

        for (int i = 0; i < startingAmmoCount; i++)
        {
            AddAmmo(1);
        }

        if(crosshairSprite == null)
        {
            for (int i = 0; i < 50; i++)
            {
                Debug.Log("Need to set the inspector value for crosshairSprite in the PlayerController script to be the crosshair provided under the UICanvas prefab.");
            }

            Application.Quit();
        }
    }

    private void FireBullet(Vector2 direction)
    { 
        GameObject bullet = Instantiate(bulletPrefab);
        if(bullet != null)
        {
            bullet.transform.position = mouthTransform.position;
            Vector2 fireDirection = (Camera.main.ScreenToWorldPoint(crosshairSprite.transform.position) - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(fireForceStrength * fireDirection, ForceMode2D.Impulse);
            RemoveAmmo(1);
        }
    }

    private void AddBubble(int count)
    {
        int originalCount = bubbleCount;

        bubbleCount += count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));
        
        HUDUIContainer.AddBubble(bubbleCount - originalCount);
    }

    public void RemoveBubble(int count)
    {
        int originalCount = bubbleCount;

        bubbleCount -= count;
        bubbleCount = Mathf.Clamp(bubbleCount, 0, (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));

        HUDUIContainer.RemoveBubble(originalCount - bubbleCount);
    }

    public void AddAmmo(int count)
    {
        int originalAmmoCount = ammoCount;
        ammoCount += count;
        ammoCount = Mathf.Clamp(ammoCount, 0, (MaxAmmoCount + PlayerInfo.MaxFlyCountMod));
        HUDUIContainer.AddAmmo(ammoCount - originalAmmoCount); 
    }

    public void RemoveAmmo(int count)
    {
        int originalAmmoCount = ammoCount;
        ammoCount -= count;
        ammoCount = Mathf.Clamp(ammoCount, 0, (MaxAmmoCount + PlayerInfo.MaxFlyCountMod));
        HUDUIContainer.RemoveAmmo(originalAmmoCount - ammoCount);
    }

    public int GetBubbleCount()
    {
        return bubbleCount;
    }

    public Vector2 RotateTowards(Vector2 current, Vector2 target)
    {
        if (current.x + current.y == 0)
            return target.normalized * maxMagnitudeDelta;

        float signedAngle = Vector2.SignedAngle(current, target);
        float stepAngle = Mathf.MoveTowardsAngle(0, signedAngle, maxRadiansDelta * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        Vector2 rotated = new Vector2(
            current.x * Mathf.Cos(stepAngle) - current.y * Mathf.Sin(stepAngle),
            current.x * Mathf.Sin(stepAngle) + current.y * Mathf.Cos(stepAngle)
        );
        if (maxMagnitudeDelta == 0)
            return rotated;

        float magnitude = current.magnitude;
        float targetMagnitude = target.magnitude;
        targetMagnitude = Mathf.MoveTowards(magnitude, targetMagnitude, maxMagnitudeDelta);
        return rotated.normalized * targetMagnitude;
    }

    private bool CheckForGroundHit(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, groundCheckDistance);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];

            //We have a collision.
            if (hit.collider != null)
            {
                // Refill to max bubbles when the player lands.
;                if (hit.collider.gameObject.CompareTag("JohnTestGround"))
                {
                    if (hit.point.y < this.transform.position.y)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void CheckIsGrounded()
    {
        Vector2 leftOrigin = new Vector2(leftFootTransform.position.x, leftFootTransform.position.y);
        Vector2 leftDirection = new Vector2(-leftFootTransform.up.x, -leftFootTransform.up.y).normalized;

        bool leftFootHit = CheckForGroundHit(leftOrigin, leftDirection);

        Vector2 rightOrigin = new Vector2(rightFootTransform.position.x, rightFootTransform.position.y);
        Vector2 rightDirection = new Vector2(-rightFootTransform.up.x, -rightFootTransform.up.y).normalized;
        bool rightFootHit = CheckForGroundHit(rightOrigin, rightDirection);

        if (leftFootHit && rightFootHit)
        {
            rigidbody.freezeRotation = true;
            CanRotate = true;
            isGrounded = true;
        }
        else if(leftFootHit == true || rightFootHit == true)
        {
            CanRotate = false;
            rigidbody.freezeRotation = false;
        }
        else
        {
            CanRotate = true;
            isGrounded = false;
        }

        spriteAnimator.SetBool("Grounded", isGrounded);
    }

    private void ApplyMovement()
    {
        if (bubbleSize > 0)
        {
            Vector2 direction = this.transform.up.normalized;
            rigidbody.AddForce(direction * bubbleForceMultiplier * bubbleSize * Time.deltaTime);
        }

        if (isGrounded)
        {
            transform.eulerAngles = Vector3.zero;

            if (Input.GetKey(KeyCode.D))
            {
                isFacingRight = true;
                isWalking = true;
                rigidbody.transform.Translate(+1.0f * Vector2.right * Time.deltaTime * WalkingSpeed);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                isFacingRight = false;
                isWalking = true;
                rigidbody.transform.Translate(-1.0f * Vector2.right * Time.deltaTime * WalkingSpeed);
            }
            else
            {
                isWalking = false;
            }
        }
        else
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGrounded();

        spriteAnimator.SetBool("IsWalking", isWalking);
        spriteAnimator.SetBool("IsFacingRight", isFacingRight);

        ApplyMovement();

        if (isGrounded)
        {
            int missingBubbles = (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod) - bubbleCount;
            if (missingBubbles > 0)
            {
                AddBubble(missingBubbles);
            }
        }

        timeSinceInput += Time.deltaTime;
        BubbleRechargeTimer += Time.deltaTime;
        WalkCycleTimer += Time.deltaTime;

        if(WalkCycleTimer > WalkCycleLength)
        {
            WalkCycleTimer = 0.0f;
        }

        if (BubbleRechargeTimer > (BubbleRechargeTimeSeconds + PlayerInfo.BubbleRechargeTimeMod))
        {
            AddBubble(1);
            BubbleRechargeTimer = 0.0f;
        }

        CalculateBubbleSize();

        ProcessTongueControls();
       
        ProcessShooting();

        CheckIfGameOver();
        ProcessDebugInputs();
    }

    private void CalculateBubbleSize()
    {    
        float normalisedBubbleSize = bubbleSize / (MaxBubbleSize + PlayerInfo.MaxBubbleSizeMod);

        //If we are not currently blowing a bubble, and we have one ready.
        if (waitingForBubbleKeyLift == false && bubbleCount > 0)
        {
            //If key pressed after cooldown.
            if (Input.GetKey(KeyCode.Space) && (timeSinceInput > inputDelay))
            {
                //Inflating Bubble based on curve value + scaling.
                bubbleSize += (animationCurve.Evaluate(normalisedBubbleSize) * (inflatingSpeedScale + PlayerInfo.InflatingSpeedMod) * Time.deltaTime);

                //If it goes too big, pop it.
                if (normalisedBubbleSize >= 1)
                {
                    // POP
                    rigidbody.AddForceY(MaxBubbleSize * -10000 * Time.deltaTime);
                    bubbleSize = 0;
                    timeSinceInput = Mathf.Min(timeSinceInput, 0.0f);
                    waitingForBubbleKeyLift = true;
                }

                spriteAnimator.SetBool("BlowingBubble", true);
            }
        }

        //Stop inflation (cc: Bank of England)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            bubbleSize = 0;
            timeSinceInput = Mathf.Min(timeSinceInput, 0);
            waitingForBubbleKeyLift = false;
            spriteAnimator.SetBool("BlowingBubble", false);
            RemoveBubble(1);
        }

        //Scale a sprite appropriately.
        bubbleSpriteObject.GetComponent<Transform>().localScale = new Vector3(normalisedBubbleSize * 2, normalisedBubbleSize * 2, normalisedBubbleSize * 2);
    }

    private void ProcessTongueControls()
    {
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

        if (canExtendTongue == false)
        {
            if (tongueLength <= 0)
            {
                canExtendTongue = true;
            }
            else if (tongueLength >= maxTongueLength)
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

        Vector3 scale = tongueSprite.transform.localScale;
        scale.x = length;
        scale.y = length / 5.0f;
        tongueSprite.transform.localScale = scale;

        if (Input.GetKeyUp(KeyCode.E))
        {
            isTongueExtended = false;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isTongueExtended = false;
        }
    }

    private void ProcessShooting()
    {
        //Update crosshair position.
        crosshairSprite.transform.position = Input.mousePosition;

        if (ammoCount > 0)
        {
            //Show crosshair while we are aiming.
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                crosshairSprite.SetActive(true);
            }

            //Hide crosshair on fire, and instantiate bullet.
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                crosshairSprite.SetActive(false);
                FireBullet(Vector2.left);
            }
        }
    }

    private void CheckIfGameOver()
    {
        if (PlayerInfo.Health <= 0)
        {
            PlayerInfo.SaveStats();
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    private void ProcessDebugInputs()
    {
        if (Application.isEditor == false)
            return;
        
        if (bubbleDetailLog)
        {
            Debug.Log("Bubbles left: " + bubbleCount + "/" + (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod));
            Debug.Log("Bubble size: " + bubbleSize);
        }

        if (tongueDetailLog)
        {
            float normalisedTongueLength = tongueLength / maxTongueLength;
            Debug.Log("Tongue Length: " + tongueLength);
            Debug.Log("Normalised Tongue Length: " + normalisedTongueLength);
        }

        if (ammoDetailLog)
        {
            Debug.Log("Ammo left: " + ammoCount + "/" + ((MaxAmmoCount + PlayerInfo.MaxFlyCountMod)));
        }

        if (Input.GetKeyDown("o"))
        {
            AddBubble(1);
        }

        if (Input.GetKeyDown("p"))
        {
            RemoveBubble(1);
        }

        if (Input.GetKeyDown("k"))
        {
            AddAmmo(1);
        }

        if (Input.GetKeyDown("l"))
        {
            RemoveAmmo(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.gameObject.tag)
        {
            case "ResetBubble":
            {
                AddBubble(1);
                Destroy(collider.gameObject);
            }
            break;

            case "FlyPickup":
            {
                AddAmmo(1);
                Destroy(collider.gameObject);
            }
            break;

            default:
                break;

        }
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 origin = new Vector2(leftFootTransform.position.x, leftFootTransform.position.y);
        Vector2 dir = new Vector2(-leftFootTransform.up.x, -leftFootTransform.up.y).normalized;
        Gizmos.DrawLine(origin, origin + (dir * groundCheckDistance));


        origin = new Vector2(rightFootTransform.position.x, rightFootTransform.position.y);
        dir = new Vector2(-rightFootTransform.up.x, -rightFootTransform.up.y).normalized;
        Gizmos.DrawLine(origin, origin + (dir * groundCheckDistance));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Refill to max bubbles when the player lands.
    //    if (collision.gameObject.CompareTag("JohnTestGround"))
    //    {
    //        if (collision.transform.position.y < this.transform.position.y)
    //        {
    //            int missingBubbles = (MaxBubbleCount + PlayerInfo.MaxBubbleCountMod) - bubbleCount;
    //            AddBubble(missingBubbles);
    //        }
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // Refill to max bubbles when the player lands.
    //    if (collision.gameObject.CompareTag("JohnTestGround"))
    //    {
    //        if (collision.transform.position.y < this.transform.position.y)
    //        {
    //            isGrounded = true;
    //            spriteAnimator.SetBool("Grounded", true);
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // Refill to max bubbles when the player lands.
    //    if (collision.gameObject.CompareTag("JohnTestGround"))
    //    {
    //        if (collision.transform.position.y < this.transform.position.y)
    //        {
    //            isGrounded = true;
    //            spriteAnimator.SetBool("Grounded", false);
    //        }
    //    }
    //}
}


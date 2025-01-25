using UnityEngine;

public class BubbleGuyTest : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    bool activateBubble;
    public float inflatingTime = 0;
    public float inflatingSpeed = 200;
    public float maximumBubbleSize = 1000;
    public AnimationCurve AnimationCurve;
    public GameObject bubbleSprite;
    public int remainingBubbles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        inflatingTime = 0;
        activateBubble = false;
        remainingBubbles = 5;
    }

    public float inputDelay = 0.2f;
    public float timeSinceInput = 0;

    // Update is called once per frame
    void Update()
    {
        float normalizedTime = inflatingTime / maximumBubbleSize;
        timeSinceInput += Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Space) && (timeSinceInput > inputDelay))
        {

            // Inflating bubble
            if (!activateBubble)
            {
                inflatingTime += AnimationCurve.Evaluate(normalizedTime) * inflatingSpeed * Time.deltaTime;
            }

            if (normalizedTime >= 1)
            {
                // POP
                rigidbody2D.AddForceY(maximumBubbleSize * -10000 * Time.deltaTime);
                inflatingTime = 0;
                timeSinceInput = Mathf.Min(timeSinceInput, -5);
                remainingBubbles--;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // stop inflating bubble
            if (inflatingTime > 0.1f)
            {
                remainingBubbles--;
            }

            inflatingTime = 0;
            timeSinceInput = Mathf.Min(timeSinceInput, 0);
        }

        bubbleSprite.GetComponent<Transform>().localScale = new Vector3(normalizedTime * 2, normalizedTime * 2, normalizedTime * 2);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody2D.AddForceX(-500 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigidbody2D.AddForceX(500 * Time.deltaTime);
        }

        if (inflatingTime > 0)
        {
            rigidbody2D.AddForceY(inflatingTime * 1000 * Time.deltaTime);
            //inflatingTime = 0;
            activateBubble = false;
        }
    }
}

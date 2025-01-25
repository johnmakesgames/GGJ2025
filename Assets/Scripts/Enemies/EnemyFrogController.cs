using UnityEngine;

public class EnemyFrogController : MonoBehaviour
{
    [SerializeField]
    Transform pointOne;

    [SerializeField]
    Transform pointTwo;

    [SerializeField]
    float moveSpeed;

    Vector3 nextPoint;

    public float MinimumDistanceToEnd = 0;
    private int direction = 1;

    EnemyStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = pointOne.position;
        nextPoint = pointOne.transform.position;
        CalculateArc(pointOne.position, pointTwo.position);
        stats = this.GetComponent<EnemyStats>();
    }

    Vector2 circleCenter;
    float curTheta = 0;
    float radius = 0;
    void CalculateArc(Vector2 p1, Vector2 p2)
    {
        radius = Mathf.Abs((p1 - p2).magnitude)/2;
        circleCenter = (p1 + p2)/2;

        p1 = (p2 - p1).normalized * radius;
        p2 = (p1 - p2).normalized * radius;

        curTheta = (Vector2.Dot(Vector2.down, (new Vector2(0,0) - p1).normalized));

        if (p1.x < p2.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    // Update is called once per frame
    float waitTimeAfterLanding = 0;
    public float DelayFromLanding = 1;

    void Update()
    {
        waitTimeAfterLanding -= Time.deltaTime;

        if (waitTimeAfterLanding < 0.1f)
        {
            // When going right we want to reduce theta, when going left we want to increase it.
            curTheta += (moveSpeed * Time.deltaTime) * direction;
            Vector2 newPos = new Vector2();
            newPos.x = (radius * Mathf.Cos(curTheta)) + circleCenter.x;
            newPos.y = (radius * Mathf.Sin(curTheta)) + circleCenter.y;

            this.transform.position = newPos;

            // Am I at the end of my arc?
            Vector2 curPos = this.transform.position;
            Vector2 nextPos = nextPoint;
            if (Vector2.Distance(curPos, nextPos) < 0.1f)
            {
                if (nextPoint == pointOne.position)
                {
                    nextPoint = pointTwo.position;
                }
                else
                {
                    nextPoint = pointOne.position;
                }

                if (curPos.x < nextPoint.x)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }

                waitTimeAfterLanding = DelayFromLanding;
            }
        }

        if (!stats.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats)
            {
                stats.Health -= this.stats.GetDamageFromEnemy();
            }
        }
    }
}
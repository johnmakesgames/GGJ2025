using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform Target;
    public Vector3 Offset;
    public float LerpSpeed;
    public bool UseLerp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UseLerp)
        {
            float lerpValue = LerpSpeed * (1 / Vector3.Distance(transform.position, Target.position + Offset) * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, Target.position + Offset, lerpValue);
        }
        else
        {
            transform.position = Target.position + Offset;
        }
        
    }
}

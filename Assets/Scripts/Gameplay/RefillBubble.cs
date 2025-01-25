using Unity.VisualScripting;
using UnityEngine;

public class RefillBubble : MonoBehaviour
{
    //----------------------------
    //
    //Deprecated gameplay element.
    //
    //----------------------------

    //[SerializeField] private float storedAirSize;
    CircleCollider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //storedAirSize = Mathf.Clamp(storedAirSize, 0, Constants.cMaxBubbleSize);
        //float normalisedSize = storedAirSize / Constants.cMaxBubbleSize;
        //transform.localScale = new Vector3(normalisedSize * 2, normalisedSize * 2, normalisedSize * 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public float GetBubbleSize()
    //{
    //    return storedAirSize;
    //}
}

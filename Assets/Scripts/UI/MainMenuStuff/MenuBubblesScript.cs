using UnityEngine;

public class MenuBubblesScript : MonoBehaviour
{
    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float startY;

    [SerializeField]
    float resetY;

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = this.transform.position;

        if (this.transform.position.y > resetY)
        {
            nextPos.y = startY;
        }

        nextPos.y += movementSpeed * Time.deltaTime;
        nextPos.x += (Mathf.Sin(nextPos.y) * Time.deltaTime) / 2;


        this.transform.position = nextPos;
    }
}

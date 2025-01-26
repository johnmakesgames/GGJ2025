using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnPrefab;
    GameObject activeSpawn;

    [SerializeField]
    Animator spriteAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeSpawn = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeSpawn)
        {
            if (!spriteAnimator.GetBool("SpawnNewDrop"))
            {
                spriteAnimator.SetBool("SpawnNewDrop", true);
                return;
            }
            else
            {
                spriteAnimator.SetBool("SpawnNewDrop", false);
            }
        }
    }

    public void OnAnimationEnd()
    {
        activeSpawn = Instantiate(SpawnPrefab, this.transform.position, this.transform.rotation);
    }
}

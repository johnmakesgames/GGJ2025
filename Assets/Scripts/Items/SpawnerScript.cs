using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnPrefab;
    GameObject activeSpawn;

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
            activeSpawn = Instantiate(SpawnPrefab, this.transform.position, this.transform.rotation);
        }
    }
}

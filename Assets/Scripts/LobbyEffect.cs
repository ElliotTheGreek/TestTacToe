using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEffect : MonoBehaviour
{
    public Material redMaterial, blueMaterial;
    public GameObject prefabX, prefabO;
    public int maxOfEach = 5;
    float spawnHeight = 45f;
    float xMin = -10f, xMax = 10f;
    float zMin = 5f, zMax = 15f;
    
    private List<GameObject> objectsList;

    void Start()
    {
        objectsList = new List<GameObject>();
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        foreach (GameObject obj in objectsList)
        {
            if (obj.transform.position.y < -2f)
            {
                ResetObject(obj);
            }
        }
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < maxOfEach; i++)
        {
            SpawnObject(prefabX);
            yield return new WaitForSeconds(1f);
            SpawnObject(prefabO);
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Renderer>().material = GetRandomMaterial();
        ApplyRandomNudge(obj);
        objectsList.Add(obj);
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        return new Vector3(x, spawnHeight, z);
    }

    Material GetRandomMaterial()
    {
        return Random.Range(0, 2) == 0 ? redMaterial : blueMaterial;
    }

    void ResetObject(GameObject obj)
    {
        obj.transform.position = GetRandomPosition();
        obj.GetComponent<Renderer>().material = GetRandomMaterial();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ApplyRandomNudge(obj);
        }
    }

    void ApplyRandomNudge(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 nudge = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            rb.AddForce(nudge, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }
    }
}

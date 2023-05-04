using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [Tooltip("Spawn position for the test cube")]
    [SerializeField] Transform _SpawnPosition;
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCubes();
        }else if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnSphere();
        }

    }

    public void SpawnCubes()
    {
        GameObject objectToSetActive = DangryGames.ObjectPooling.ObjectPool.SharedInstance.GetPooledObject("TestCube");
        objectToSetActive.transform.position = _SpawnPosition.position;
        objectToSetActive.transform.rotation = _SpawnPosition.rotation;
        objectToSetActive.SetActive(true);
    }

    public void SpawnSphere()
    {
        GameObject objectToSetActive = DangryGames.ObjectPooling.ObjectPool.SharedInstance.GetPooledObject("TestSphere");
        objectToSetActive.transform.position = _SpawnPosition.position;
        objectToSetActive.transform.rotation = _SpawnPosition.rotation;
        objectToSetActive.SetActive(true);
    }

}

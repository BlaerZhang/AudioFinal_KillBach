using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BandSpawner : MonoBehaviour
{
    public List<GameObject> musicianPrefabs;
    public float spawnInterval;
    public float spawnPointX;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleBandList()
    {
        var count = musicianPrefabs.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = musicianPrefabs[i];
            musicianPrefabs[i] = musicianPrefabs[r];
            musicianPrefabs[r] = tmp;
        }
    }

    public void SpawnBand(float moveSpeed, bool isMovingRight)
    {
        for (int i = 0; i < musicianPrefabs.Count; i++)
        {
            GameObject musician = isMovingRight
                ? Instantiate(musicianPrefabs[i], new Vector3(-(spawnPointX + i * spawnInterval), 0, 0),
                    Quaternion.Euler(Vector3.zero))
                : Instantiate(musicianPrefabs[i], new Vector3(spawnPointX + i * spawnInterval, 0, 0),
                    Quaternion.Euler(Vector3.zero));
            
            musician.GetComponent<Target>().moveSpeed = isMovingRight ? moveSpeed : -moveSpeed;
        }
    }
}

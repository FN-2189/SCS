using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static List<Bullet> _bullets = new List<Bullet>();

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Bullet bullet in _bullets)
        {
            if(bullet.transform.position.magnitude > 20000f)
            {
                _bullets.Remove(bullet);
                Destroy(bullet.gameObject);
            }
        }
        RaycastForAll();
    }

    public static void AddToList(Bullet bullet)
    {
        _bullets.Add(bullet);
    }

    private void RaycastForAll()
    {
        // Perform a raycast per bullet using RaycastCommand and wait for it to complete
        // Setup the command and result buffers
        var results = new NativeArray<RaycastHit>(_bullets.Count, Allocator.TempJob);

        var commands = new NativeArray<RaycastCommand>(_bullets.Count, Allocator.TempJob);

        // Set the data of the first command

        for(int i = 0; i< _bullets.Count; i++)
        {
            commands[i] = new RaycastCommand(_bullets[i].transform.position, _bullets[i].rb.velocity.normalized, _bullets[i].rb.velocity.magnitude * Time.fixedDeltaTime);
        }

        // Schedule the batch of raycasts
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default);

        Debug.Log("Starting Job", this);
        // Wait for the batch processing job to complete
        handle.Complete();

        Debug.Log("Completed Raycast Job", this);

        for(int i = 0; i < _bullets.Count; i++)
        {
            if (results[i].collider)
            {
                _bullets[i].Hit(results[i].collider);
                _bullets.Remove(_bullets[i]);
            }
        }

        // Dispose the buffers
        results.Dispose();
        commands.Dispose();
    }
}

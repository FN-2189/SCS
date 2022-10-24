using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static List<Bullet> _bullets = new List<Bullet>();

    [SerializeField]
    private float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Destroy when to far away
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i].transform.position.magnitude > maxDistance)
            {
                var bullet = _bullets[i];
                RelativeSpace.removeObject(bullet.transform);
                _bullets.RemoveAt(i);
                Destroy(bullet.gameObject);
            }
        }
        RaycastForAll();
    }

    public static void AddToList(Bullet bullet)
    {
        _bullets.Add(bullet);
        RelativeSpace.addObject(bullet.transform);
    }

    private void RaycastForAll()
    {
        // Perform a raycast per bullet using RaycastCommand and wait for it to complete
        // Setup the command and result buffers
        var results = new NativeArray<RaycastHit>(_bullets.Count, Allocator.TempJob);

        var commands = new NativeArray<RaycastCommand>(_bullets.Count, Allocator.TempJob);

        try {
            // Set the data of the first command

            for (int i = 0; i < _bullets.Count; i++)
            {
                commands[i] = new RaycastCommand(_bullets[i].transform.position, _bullets[i].rb.velocity.normalized, _bullets[i].rb.velocity.magnitude * Time.fixedDeltaTime);
                _bullets[i].lineRenderer.SetPositions(new Vector3[] { _bullets[i].transform.position + _bullets[i].rb.velocity * Time.fixedDeltaTime, _bullets[i].transform.position + _bullets[i].rb.velocity * Time.fixedDeltaTime * 2 });// What is this
            }

            // Schedule the batch of raycasts
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default);

            Debug.Log("Starting Job", this);
            // Wait for the batch processing job to complete
            handle.Complete();

            Debug.Log("Completed Raycast Job", this);

            for (int i = 0; i < _bullets.Count; i++)
            {
                if (results[i].collider)
                {
                    _bullets[i].Hit(results[i].collider);

                    RelativeSpace.removeObject(_bullets[i].transform);
                    _bullets.Remove(_bullets[i]);

                }
            }

        } 
        catch (Exception e)
        {
            Debug.LogError($"An Error occured in the Raycast Job!\n{e.StackTrace}");
            return;
        }
        finally
        {
            // Dispose the buffers
            results.Dispose();
            commands.Dispose();
        }
    }
}

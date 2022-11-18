using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletManager : MonoBehaviour
{
    private static List<Bullet> _bullets = new List<Bullet>();

    [SerializeField]
    private float maxDistance;


    private Vector3 v;
    private Bullet b;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastForAll();
    }

    public static void AddToList(Bullet bullet)
    {
        _bullets.Add(bullet);
        RelativeSpace.addObject(bullet.transform);
    }

    public static void RemoveFromList(Bullet bullet)
    {
        _bullets.Remove(bullet);
        RelativeSpace.removeObject(bullet.transform);
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
                b = _bullets[i];
                v = b.rb.velocity;
                commands[i] = new RaycastCommand(b.transform.position, v, v.magnitude * Time.fixedDeltaTime);
                //_bullets[i].lineRenderer.SetPositions(new Vector3[] { _bullets[i].transform.position + _bullets[i].rb.velocity * Time.fixedDeltaTime, _bullets[i].transform.position + _bullets[i].rb.velocity * Time.fixedDeltaTime * 2 });// What is this
            }

            // Schedule the batch of raycasts
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default);

            //Debug.Log(_bullets.Count);
            // Wait for the batch processing job to complete
            handle.Complete();

            //Debug.Log("Completed Raycast Job", this);

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
            // Dispose the buffers
            results.Dispose();
            commands.Dispose();
            return;
        }
        // Dispose the buffers
        results.Dispose();
        commands.Dispose();
    }
}

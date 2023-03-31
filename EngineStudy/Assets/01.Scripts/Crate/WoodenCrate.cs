using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodenCrate : MonoBehaviour
{
    [SerializeField]
    private GameObject _explodeWoodCrate;
    
    private Vector3 _explodeDir;
    private float _power = 8f;

    private void Update()
    {
        InputDir();

        if (Input.GetKeyUp(KeyCode.Space))
            ExplosionCrate();
    }

    private void InputDir()
    {
        _explodeDir.x = Input.GetAxisRaw("Horizontal");
        _explodeDir.z = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Q))
            _explodeDir.y = 1;
        else
            _explodeDir.y = 0;

        _explodeDir.Normalize();
        
    }

    private void ExplosionCrate()
    {
        
        GameObject _newCrate = Instantiate(_explodeWoodCrate, transform.position, transform.rotation);
        Rigidbody[] _rigidBody = _newCrate.GetComponentsInChildren<Rigidbody>();
        
        _newCrate.GetComponentsInChildren<Rigidbody>().ToList().ForEach(
            r => r.AddForce(_explodeDir * _power, ForceMode.Impulse));

        Destroy(transform.gameObject);
    }
}

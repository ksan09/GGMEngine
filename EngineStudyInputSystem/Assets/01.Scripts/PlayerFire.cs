using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.OnFire += BulletFire;
    }

    private void BulletFire()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(_playerInput.MousePos), out hit);
        if (hit.collider)
        {
            Vector3 pos = hit.point;
            Vector3 dir = (pos - transform.position).normalized;
            
            GameObject tempObj = Instantiate(_bulletPrefab, 
                transform.position, Quaternion.identity);
            tempObj.GetComponent<Rigidbody>().velocity = dir * 30f;
            Destroy(tempObj, 2);
        }
    }
}

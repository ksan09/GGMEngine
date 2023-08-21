using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}

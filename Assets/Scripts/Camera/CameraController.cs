using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed = 5;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 0, -10);

    private void Awake()
    {
        if (_player == null)
        {
            enabled = false;

            throw new System.NullReferenceException("Missing Player field on CameraController");
        }            
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _player.transform.position, _movementSpeed * Time.deltaTime) + _offset;
    }
}

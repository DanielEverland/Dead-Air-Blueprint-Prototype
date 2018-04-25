using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSpawner : MonoBehaviour {

    [SerializeField]
    private Generator _prefab;

    private const float Z_DEPTH = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generator instance = Instantiate(_prefab);

            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosInWorld.z = Z_DEPTH;

            instance.transform.position = mousePosInWorld;
        }
    }
}

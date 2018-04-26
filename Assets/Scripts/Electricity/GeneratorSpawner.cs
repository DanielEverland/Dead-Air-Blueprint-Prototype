using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSpawner : MonoBehaviour, IPlayerAction {

    [SerializeField]
    private Generator _prefab;

    private const float Z_DEPTH = 0;

    public KeyCode ActivationKey { get { return KeyCode.G; } }

    private Generator _instance;

    public void DoUpdate()
    {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosInWorld.z = Z_DEPTH;

        _instance.transform.position = mousePosInWorld;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _instance = null;

            PlayerController.ResetAction();
        }
    }    
    public void OnSelected()
    {
        _instance = Instantiate(_prefab);
    }
    public void OnDeselected()
    {
        if (_instance != null)
            Destroy(_instance);
    }
}

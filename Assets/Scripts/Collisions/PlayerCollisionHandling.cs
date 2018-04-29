using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandling : MonoBehaviour {

    [SerializeField]
    private CharacterController _controller;

    private HashSet<Transform> _oldCollisionList;
    private HashSet<Transform> _currentCollisionList;

    private void Awake()
    {
        _oldCollisionList = new HashSet<Transform>();
        _currentCollisionList = new HashSet<Transform>();
    }
    private void Update()
    {
        _currentCollisionList = new HashSet<Transform>();
        
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, _controller.radius, Vector3.forward))
        {
            _currentCollisionList.Add(hit.transform);
        }

        foreach (Transform toPoll in _currentCollisionList.Union(_oldCollisionList))
        {
            PollObject(toPoll);
        }

        _oldCollisionList = _currentCollisionList;
    }
    private void PollObject(Transform obj)
    {
        ItemObject item = obj.gameObject.GetComponent<ItemObject>();

        if (item == null)
            return;

        bool isOld = _oldCollisionList.Contains(obj);
        bool isCurrent = _currentCollisionList.Contains(obj);

        if(IsEnter(isOld, isCurrent))
        {
            item.RaiseEvent(PropertyEventTypes.OnCollisionEnter, transform);
        }
        else if(IsStay(isOld, isCurrent))
        {
            item.RaiseEvent(PropertyEventTypes.OnCollisionStay, transform);
        }
        else if(IsExit(isOld, isCurrent))
        {
            item.RaiseEvent(PropertyEventTypes.OnCollisionExit, transform);
        }
    }
    private bool IsEnter(bool isOld, bool isCurrent)
    {
        return !isOld && isCurrent;
    }
    private bool IsStay(bool isOld, bool isCurrent)
    {
        return isOld && isCurrent;
    }
    private bool IsExit(bool isOld, bool isCurrent)
    {
        return isOld && !isCurrent;
    }
}

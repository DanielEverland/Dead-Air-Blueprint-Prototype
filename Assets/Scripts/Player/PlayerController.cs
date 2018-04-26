using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed = 10;
    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private List<Component> _actions;
    [SerializeField]
    private Component _defaultAction;

    private static List<IPlayerAction> _allActions;
    private static IPlayerAction _resetAction;
    private static IPlayerAction _currentAction;

    private void Awake()
    {
        _resetAction = _defaultAction as IPlayerAction;
        _allActions = new List<IPlayerAction>(_actions.Where(x => x is IPlayerAction).Select(x => x as IPlayerAction));

        ResetAction();
    }
    private void Update()
    {
        PollInput();
        PollAction();
    }
    private void PollAction()
    {
        if (_currentAction != null)
            _currentAction.DoUpdate();

        foreach (IPlayerAction action in _allActions)
        {
            if (Input.GetKeyDown(action.ActivationKey))
                SelectAction(action);
        }
    }
    public static void ResetAction()
    {
        SelectAction(_resetAction);
    }
    private static void SelectAction(IPlayerAction action)
    {
        if (_currentAction != null)
            _currentAction.OnDeselected();

        _currentAction = action;
        _currentAction.OnSelected();
    }
    private void PollInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= 1;
        }

        Move(direction);
    }
    private void Move(Vector2 direction)
    {
        _controller.Move(direction * _movementSpeed * Time.deltaTime);
    }
    private void OnValidate()
    {
        _controller = GetComponent<CharacterController>();
    }
}

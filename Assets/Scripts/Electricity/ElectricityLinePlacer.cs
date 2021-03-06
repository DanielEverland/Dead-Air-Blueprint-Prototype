﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityLinePlacer : MonoBehaviour, IPlayerAction
{
    public KeyCode ActivationKey { get { return KeyCode.E; } }

    private ElectricityLineHandler _currentHandler;

    public void DoUpdate()
    {
        PollInput();
        DoRendering();
    }
    private void DoRendering()
    {
        if (_currentHandler == null)
            return;

        _currentHandler.ResolveLineRendering();
    }
    private void PollInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(_currentHandler == null)
            {
                _currentHandler = new ElectricityLineHandler();
            }
            else
            {
                PushLine();
            }            
        }
    }
    private void PushLine()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currentHandler.AddPosition(mousePosInWorld);

        if (_currentHandler.IsReadyToPlace)
            PlaceLine();
    }
    private void PlaceLine()
    {
        _currentHandler.Place();
        _currentHandler = null;

        PlayerController.ResetAction();
    }
    public void OnDeselected()
    {
    }
    public void OnSelected()
    {
        _currentHandler = new ElectricityLineHandler();
    }
    private class ElectricityLineHandler
    {
        public ElectricityLineHandler()
        {
            _currentLine = Instantiate(ElectricityManager.LinePrefab);
        }

        public ElectricityLine CurrentLine { get { return _currentLine; } }
        public bool IsReadyToPlace { get { return _placedStartPos.HasValue && _placedEndPos.HasValue; } }
        
        private Vector2 StartPosition
        {
            get
            {
                if (_placedStartPos.HasValue)
                    return _placedStartPos.Value;

                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        private Vector2 EndPosition
        {
            get
            {
                if (_placedEndPos.HasValue)
                    return _placedEndPos.Value;

                if(_placedStartPos.HasValue)
                    return Camera.main.ScreenToWorldPoint(Input.mousePosition);

                return PlayerController.Player.transform.position;
            }
        }

        private ElectricityLine _currentLine;
        private Vector2? _placedStartPos;
        private Vector2? _placedEndPos;

        public void Place()
        {
            if (!IsReadyToPlace)
                throw new System.InvalidOperationException();

            _currentLine.Initialize(_placedStartPos.Value, _placedEndPos.Value);
        }
        public void AddPosition(Vector2 pos)
        {
            if (!_placedStartPos.HasValue)
            {
                _placedStartPos = pos;
            }
            else if (!_placedEndPos.HasValue)
            {
                _placedEndPos = pos;
            }
        }
        public void ResolveLineRendering()
        {
            Vector2 fixedA, fixedB;
            ElectricityManager.ResolvePositions(StartPosition, EndPosition, out fixedA, out fixedB);

            _currentLine.Renderer.SetPosition(0, fixedA);
            _currentLine.Renderer.SetPosition(1, fixedB);
        }
    }
}

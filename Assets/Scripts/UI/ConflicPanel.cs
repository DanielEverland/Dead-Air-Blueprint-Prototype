using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConflicPanel : MonoBehaviour {

    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Text _elementPrefab;
    [SerializeField]
    private Transform _elementParent;

    private void Awake()
    {
        PropertyPanel.OnChanged += Poll;
    }
    public void Poll()
    {
        foreach (Transform child in _elementParent)
        {
            Destroy(child.gameObject);
        }

        PropertyConflictManager.Result result = PropertyConflictManager.Resolve(PropertyPanel.SelectedProperties);

        foreach (string message in result.Messages)
        {
            Text text = Instantiate(_elementPrefab);
            text.transform.SetParent(_elementParent);

            text.text = message;
        }

        _createButton.interactable = result.Succeeded;
    }
}

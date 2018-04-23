using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationElement : MonoBehaviour {

    [SerializeField]
    private Text _textElement;

    private IWorldObject _obj;

    public void Initialize(IWorldObject obj)
    {
        _obj = obj;
    }
    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_obj.Point);

        _textElement.text = _obj.GetInformationString();
    }
}

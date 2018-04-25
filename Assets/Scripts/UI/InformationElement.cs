using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationElement : MonoBehaviour {

    [SerializeField]
    private Text _textElement;

    private IInformationObject _obj;

    public void Initialize(IInformationObject obj)
    {
        _obj = obj;
    }
    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_obj.Point);

        _textElement.text = _obj.GetInformationString();
    }
}

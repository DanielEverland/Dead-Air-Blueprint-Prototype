using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelectionPanel : MonoBehaviour {

    [SerializeField]
    private IconSelectionButton _iconButton;
    [SerializeField]
    private RectTransform _buttonParent;

    public System.Action<Texture2D> Callback;

    public void Initialize()
    {
        foreach (Texture2D icon in IconManager.Icons)
        {
            IconSelectionButton button = Instantiate(_iconButton);
            button.transform.SetParent(_buttonParent);

            button.Initialize(icon, this);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonParent);
    }
    public void Select(Texture2D icon)
    {
        if (Callback != null)
            Callback.Invoke(icon);

        Destroy(gameObject);
    }
}

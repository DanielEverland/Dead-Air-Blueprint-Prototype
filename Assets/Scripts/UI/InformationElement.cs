using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationElement : MonoBehaviour {

    [SerializeField]
    private Text _textElement;

    private ItemObject _obj;

    public void Initialize(ItemObject obj)
    {
        _obj = obj;
    }
    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_obj.transform.position);

        _textElement.text = GetString(_obj.Item);
    }
    private string GetString(ItemBase item)
    {
        StringBuilder builder = new StringBuilder();

        GetProperties(item, builder);
        
        return builder.ToString();
    }
    private void GetProperties(ItemBase item, StringBuilder builder)
    {
        if(item.Properties.Count() == 0)
        {
            builder.Append("NO PROPERTIES");
        }
        else
        {
            foreach (PropertyBase property in item.Properties)
            {
                builder.Append(property.GetType().Name);
                builder.Append(':');
                Indent(builder);

                string[] propertyInfo = property.GetInformation();

                if(propertyInfo.Length != 0)
                {
                    builder.Append(string.Join(",    ", propertyInfo));
                }
                else
                {
                    builder.Append("NONE");
                }
                
                builder.AppendLine();
            }
        }        
    }
    private void Indent(StringBuilder builder)
    {
        builder.Append("    ");
    }
}

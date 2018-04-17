using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {

    public ItemBase Item { get; set; }

    private void Update()
    {
        Item.Update();
    }
}

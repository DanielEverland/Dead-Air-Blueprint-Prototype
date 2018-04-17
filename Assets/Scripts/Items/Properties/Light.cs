﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items.Properties
{
    public class Light : PropertyBase, IPropertyInput
    {
        public Light(ItemBase owner) : base(owner) { }

        private UnityEngine.Light _light;

        public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnElectricalInput | PropertyEventTypes.OnTrigger; } }

        private void OnElectricalInput()
        {
            _light.enabled = true;
        }
        private void OnTrigger()
        {
            _light.enabled = true;
        }
        public override void CreateInstance(GameObject obj)
        {
            _light = obj.AddComponent<UnityEngine.Light>();
            _light.enabled = false;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items.Properties
{
    public class LightProperty : PropertyBase, IPropertyInput
    {
        public LightProperty(ItemBase owner) : base(owner) { }

        private Light _light;

        public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnElectricalInputChanged; } }

        private void OnElectricalInputChanged()
        {
            _light.enabled = Owner.IsElectricallyCharged;
        }
        public override void CreateInstance(GameObject obj)
        {
            IEnumerable<Component> components = PropertyHelper.CopyComponents("Light", obj);

            _light = components.First<Light>();
            _light.enabled = Owner.IsElectricallyCharged;
        }
    }
}
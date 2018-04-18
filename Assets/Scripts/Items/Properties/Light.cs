using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items.Properties
{
    public class Light : PropertyBase, IPropertyInput
    {
        public Light(ItemBase owner) : base(owner) { }

        private UnityEngine.Light _light;

        public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnElectricalInputChanged; } }

        private void OnElectricalInputChanged()
        {
            _light.enabled = Owner.IsElectricallyCharged;
        }
        public override void CreateInstance(GameObject obj)
        {
            IEnumerable<Component> components = PropertyHelper.CopyComponents("LightProperty", obj);

            _light = components.First<UnityEngine.Light>();
            _light.enabled = Owner.IsElectricallyCharged;
        }
    }
}

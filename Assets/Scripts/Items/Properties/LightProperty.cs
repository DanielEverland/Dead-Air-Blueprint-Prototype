using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items.Properties
{
    public class LightProperty : PropertyBase, IPropertyInput
    {
        public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnElectricalInputChanged; } }

        public override string Name { get { return "Light"; } }

        private Light _light;

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
        public override string[] GetInformation()
        {
            return new string[1]
            {
                "Enabled: " + _light.enabled,
            };
        }
    }
}

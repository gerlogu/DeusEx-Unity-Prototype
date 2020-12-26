using UnityEngine;

namespace Febucci.UI.Core
{
    [System.Serializable]
    public class PresetBehaviorValues : PresetBaseValues
    {

        [SerializeField] public EmissionControl emission;

        public override void Initialize(bool isAppearance)
        {
            base.Initialize(isAppearance);
            emission.Initialize(GetMaxDuration());

        }
    }

}
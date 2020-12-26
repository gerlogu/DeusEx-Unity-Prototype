using System;
using UnityEngine;

namespace MPUIKIT {
    /// <summary>
    /// Basic Rectangle shape with the same width and height of the rect-transform
    /// </summary>
    [Serializable]
    public struct Rectangle : IMPUIComponent{
        [SerializeField] private Vector4 m_CornerRadius;
#if UNITY_EDITOR
        [SerializeField] private bool m_UniformCornerRadius;
#endif
        /// <summary>
        /// <para>Radius of the four corners. Counter-Clockwise from bottom-left</para>
        /// <para>x => bottom-left, y => bottom-right</para>
        /// <para>z => top-right, w => top-left</para>
        /// </summary>
        public Vector4 CornerRadius {
            get => m_CornerRadius;
            set {
                m_CornerRadius = Vector4.Max(value, Vector4.zero);
                if (ShouldModifySharedMat) {
                    SharedMat.SetVector(SpRectangleCornerRadius, m_CornerRadius);
                }
                OnComponentSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Material SharedMat { get; set; }
        public bool ShouldModifySharedMat { get; set; }
        public RectTransform RectTransform { get; set; }
        
        private static readonly int SpRectangleCornerRadius = Shader.PropertyToID("_RectangleCornerRadius");

        public void Init(Material sharedMat, Material renderMat, RectTransform rectTransform) {
            SharedMat = sharedMat;
            ShouldModifySharedMat = sharedMat == renderMat;
            RectTransform = rectTransform;
        }

        public event EventHandler OnComponentSettingsChanged;
        
        public void OnValidate() {
            CornerRadius = m_CornerRadius;
        }

        public void InitValuesFromMaterial(ref Material material) {
            m_CornerRadius = material.GetVector(SpRectangleCornerRadius);
        }

        public void ModifyMaterial(ref Material material, params object[] otherProperties) {
            material.SetVector(SpRectangleCornerRadius, m_CornerRadius);
        }
    }
}
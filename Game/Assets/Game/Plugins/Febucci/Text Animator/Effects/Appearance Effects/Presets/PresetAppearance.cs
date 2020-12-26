using UnityEngine;
namespace Febucci.UI.Core
{
    class PresetAppearance : AppearanceBase
    {
        bool enabled;

        //management
        Matrix4x4 matrix;
        Vector3 movementVec;
        Vector3 scaleVec;
        Vector3 offset;
        Vector3 rotationEuler;
        Quaternion rotationQua;

        bool hasTransformEffects;

        //movement
        bool setMovement;
        EffectEvaluator movementX;
        EffectEvaluator movementY;
        EffectEvaluator movementZ;

        //scale
        bool setScale;
        float scaleXDuration;
        float scaleYDuration;
        EffectEvaluator scaleX;
        EffectEvaluator scaleY;

        //rotation
        bool setRotation;
        EffectEvaluator rotX;
        EffectEvaluator rotY;
        EffectEvaluator rotZ;

        bool setColor;
        Color32 color;
        ColorCurve colorCurve;

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            movementVec = Vector3.zero;

            showDuration = 0;

            enabled = false;

            void AssignValues(PresetAppearanceValues result)
            {
                enabled = SetPreset(
                    true,
                    result,
                    ref movementX,
                    ref movementY,
                    ref movementZ,
                    ref setMovement,
                    ref showDuration,
                    ref scaleVec,
                    ref setScale,
                    ref scaleX,
                    ref scaleY,
                    ref scaleXDuration,
                    ref scaleYDuration,
                    ref setRotation,
                    ref rotationQua,
                    ref rotX,
                    ref rotY,
                    ref rotZ,
                    ref hasTransformEffects,
                    ref setColor,
                    ref colorCurve);
            }

            void TryAssigningPreset()
            {
                PresetAppearanceValues result;
                //searches for local presets first, which override global presets
                if (TAnimBuilder.GetPresetFromArray(effectTag, data.presets, out result))
                {
                    AssignValues(result);
                    return;
                }

                //global presets
                if (TAnimBuilder.TryGetGlobalPresetAppearance(effectTag, out result))
                {
                    AssignValues(result);
                    return;
                }

            }

            TryAssigningPreset();
        }

        //TODO: REFACTOR THIS !!

        public static bool SetPreset<T>(
            bool isAppearance,
            T values,
            ref EffectEvaluator movementX,
            ref EffectEvaluator movementY,
            ref EffectEvaluator movementZ,
            ref bool setMovement,
            ref float showDuration,
            ref Vector3 scaleVec,
            ref bool setScale,
            ref EffectEvaluator scaleX,
            ref EffectEvaluator scaleY,
            ref float scaleXDuration,
            ref float scaleYDuration,
            ref bool setRotation,
            ref Quaternion rotationQua,
            ref EffectEvaluator rotX,
            ref EffectEvaluator rotY,
            ref EffectEvaluator rotZ,
            ref bool hasTransformEffects,
            ref bool setColor,
            ref ColorCurve colorCurve
            ) where T : PresetBaseValues
        {

            values.Initialize(isAppearance);
            showDuration = values.GetMaxDuration();

            setMovement = values.movementX.enabled || values.movementY.enabled || values.movementZ.enabled;
            if (setMovement)
            {
                movementX = values.movementX;
                movementY = values.movementY;
                movementZ = values.movementZ;
            }

            scaleVec = Vector3.one;
            setScale = values.scaleX.enabled || values.scaleY.enabled;
            if (setScale)
            {
                scaleX = values.scaleX;
                scaleY = values.scaleY;

                scaleVec.z = 1;

                scaleXDuration = scaleX.GetDuration();
                scaleYDuration = scaleY.GetDuration();
            }

            setRotation = values.rotX.enabled || values.rotY.enabled || values.rotZ.enabled;
            rotationQua = Quaternion.identity;
            if (setRotation)
            {
                rotX = values.rotX;
                rotY = values.rotY;
                rotZ = values.rotZ;
            }

            hasTransformEffects = setMovement || setRotation || setScale;

            setColor = values.color.enabled;
            if (setColor)
            {
                colorCurve = values.color;
                colorCurve.Initialize(isAppearance);
            }

            return hasTransformEffects || setColor;
        }



        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            if (!enabled)
                return;

            if (hasTransformEffects)
            {
                offset = (data.vertices[0] + data.vertices[2]) / 2f;

                #region Movement

                if (setMovement)
                {
                    movementVec.x = movementX.Evaluate(data.passedTime, charIndex);
                    movementVec.y = movementY.Evaluate(data.passedTime, charIndex);
                    movementVec.z = movementZ.Evaluate(data.passedTime, charIndex);

                    movementVec *= effectIntensity;

                }
                #endregion

                #region Scale
                if (setScale)
                {
                    scaleVec.x = scaleX.Evaluate(data.passedTime, charIndex);
                    scaleVec.y = scaleY.Evaluate(data.passedTime, charIndex);
                }

                #endregion

                #region Rotation
                if (setRotation)
                {

                    rotationEuler.x = rotX.Evaluate(data.passedTime, charIndex);
                    rotationEuler.y = rotY.Evaluate(data.passedTime, charIndex);
                    rotationEuler.z = rotZ.Evaluate(data.passedTime, charIndex);

                    rotationQua.eulerAngles = rotationEuler;
                }
                #endregion

                matrix.SetTRS(movementVec, rotationQua, scaleVec);

                for (byte i = 0; i < data.vertices.Length; i++)
                {
                    data.vertices[i] -= offset;
                    data.vertices[i] = matrix.MultiplyPoint3x4(data.vertices[i]);
                    data.vertices[i] += offset;
                }
            }

            if (setColor)
            {
                color = colorCurve.GetColor(data.passedTime, charIndex);
                data.colors.LerpUnclamped(color, 1 - data.passedTime / showDuration);
            }

        }
    }
}
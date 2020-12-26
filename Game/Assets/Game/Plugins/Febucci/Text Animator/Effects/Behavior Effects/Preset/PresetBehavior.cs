using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Febucci.UI.Core
{
    class PresetBehavior : BehaviorBase
    {
        bool enabled;

        //modifiers
        float timeSpeed;
        float weightMult;

        //management
        Matrix4x4 matrix;
        Vector3 movementVec;
        Vector3 scaleVec;
        Vector3 offset;
        Vector3 rotationEuler;
        Quaternion rotationQua;

        float uniformEffectTime;

        bool hasTransformEffects;

        bool isOnOneCharacter;

        float weight = 1;
        EmissionControl emissionControl;

        //movement
        bool setMovement;
        EffectEvaluator movementX;
        EffectEvaluator movementY;
        EffectEvaluator movementZ;

        //scale
        bool setScale;
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

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            weightMult = 1;
            timeSpeed = 1;

            uniformEffectTime = 0;
            weight = 0;
            isOnOneCharacter = false;


            movementVec = Vector3.zero;

            float showDuration = 0;
            float scaleXDuration = 0;
            float scaleYDuration = 0;

            enabled = false;


            void AssignValues(PresetBehaviorValues result)
            {
                emissionControl = result.emission;

                enabled = PresetAppearance.SetPreset(
                    false,
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

                emissionControl.Initialize(showDuration);
            }


            void TryAssigningPreset()
            {
                PresetBehaviorValues result;

                //searches for local presets first, which override global presets
                if (TAnimBuilder.GetPresetFromArray(effectTag, data.presets, out result))
                {
                    AssignValues(result);
                    return;
                }

                //global presets
                if (TAnimBuilder.TryGetGlobalPresetBehavior(effectTag, out result))
                {
                    AssignValues(result);
                    return;
                }

            }

            TryAssigningPreset();
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                case "f": //frequency, increases the time speed
                    ApplyModifierTo(ref timeSpeed, modifierValue);
                    break;

                case "a": //increase the amplitude
                    ApplyModifierTo(ref weightMult, modifierValue);
                    break;
            }
        }

        public override void Calculate()
        {
            if (!isOnOneCharacter)
                return;

            uniformEffectTime = emissionControl.IncreaseEffectTime(animatorDeltaTime * timeSpeed);

        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            if (!enabled)
                return;

            if (!isOnOneCharacter)
                isOnOneCharacter = data.passedTime > 0;

            weight = emissionControl.effectWeigth * weightMult;

            if (weight == 0) //no effect
                return;

            if (hasTransformEffects)
            {
                offset = (data.vertices[0] + data.vertices[2]) / 2f;

                #region Movement
                if (setMovement)
                {
                    movementVec.x = movementX.Evaluate(uniformEffectTime, charIndex);
                    movementVec.y = movementY.Evaluate(uniformEffectTime, charIndex);
                    movementVec.z = movementZ.Evaluate(uniformEffectTime, charIndex);

                    movementVec *= effectIntensity * weight;  //movement also needs effects intensity (might change depending on fonts etc.)

                }
                #endregion

                #region Scale
                if (setScale)
                {
                    scaleVec.x = scaleX.Evaluate(uniformEffectTime, charIndex);
                    scaleVec.y = scaleY.Evaluate(uniformEffectTime, charIndex);

                    //weighted scale
                    scaleVec = Vector3.LerpUnclamped(Vector3.one, scaleVec, weight);

                }
                #endregion

                #region Rotation
                if (setRotation)
                {

                    rotationEuler.x = rotX.Evaluate(uniformEffectTime, charIndex);
                    rotationEuler.y = rotY.Evaluate(uniformEffectTime, charIndex);
                    rotationEuler.z = rotZ.Evaluate(uniformEffectTime, charIndex);

                    //weighted rotation
                    rotationQua.eulerAngles = rotationEuler * weight;
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
                color = colorCurve.GetColor(uniformEffectTime, charIndex);
                data.colors.LerpUnclamped(color, Mathf.Clamp(weight, -1, 1));
            }

        }
    }
}
namespace Febucci.UI.Core
{
    /// <summary>
    /// Base class for all appearance effects
    /// </summary>
    public abstract class AppearanceBase : EffectsBase
    {
        protected float showDuration = .3f;

        public abstract void SetDefaultValues(AppearanceDefaultValues data);

        public virtual bool CanShowAppearanceOn(float timePassed)
        {
            return timePassed <= showDuration;
        }
    }
}
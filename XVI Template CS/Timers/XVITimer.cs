using Godot;

namespace DragonXVI.Timers
{
    /// <summary>
    /// Base class for custom timers.
    /// </summary>
    public abstract partial class XVITimer : Timer
    {
        /// <summary>
        /// Same as autostart in the timer class, but i have more control over this one.
        /// If true, the timer starts itself when in the tree.
        /// </summary>
        [Export]
        public bool StartAutomatically = false;

        /// <summary>
        /// Godot rng object used to control the random state of this.
        /// If not provided then itll use the global random functions.
        /// </summary>
        public RandomNumberGenerator Rng = null;

        public override void _Ready()
        {

        }

        public abstract void StartExt();

        protected void OnSelfTimeout()
        {
            if (!OneShot)
            {
                StartExt();
            }
        }
    }
}

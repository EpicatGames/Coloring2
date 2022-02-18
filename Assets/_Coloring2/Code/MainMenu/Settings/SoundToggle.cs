
namespace Coloring2.MainMenu.Settings
{
    public class SoundToggle : AudioToggle
    {
        protected override void OnToggle(bool value)
        {
            SoundsManager.SoundOn = value;
            base.OnToggle(value);
        }
    }
}
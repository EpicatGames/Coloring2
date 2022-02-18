
namespace Coloring2.MainMenu.Settings
{
    public class MusicToggle : AudioToggle
    {
        protected override void OnToggle(bool value)
        {
            SoundsManager.MusicOn = value;
            base.OnToggle(value);
        }
    }
}

namespace Coloring2.MainMenu.Settings
{
    public class MusicToggle : AudioToggle
    {
        protected override void Start()
        {
            base.Start();
            
            if(_toggle.isOn == SoundsManager.MusicOn)
                OnToggle(SoundsManager.MusicOn);
            else
                _toggle.isOn = SoundsManager.MusicOn;
        }
        
        protected override void OnToggle(bool value)
        {
            SoundsManager.MusicOn = value;
            base.OnToggle(value);
        }
    }
}
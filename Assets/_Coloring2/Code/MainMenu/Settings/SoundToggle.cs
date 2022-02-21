
namespace Coloring2.MainMenu.Settings
{
    public class SoundToggle : AudioToggle
    {
        protected override void Start()
        {
            base.Start();

            if(_toggle.isOn == SoundsManager.SoundOn)
                OnToggle(SoundsManager.SoundOn);
            else
                _toggle.isOn = SoundsManager.SoundOn;
        }
        
        protected override void OnToggle(bool value)
        {
            SoundsManager.SoundOn = value;
            base.OnToggle(value);
        }
    }
}
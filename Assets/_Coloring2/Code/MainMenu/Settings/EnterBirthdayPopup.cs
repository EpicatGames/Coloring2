using System;
using Coloring2.DataServices;
using Coloring2.Localization;
using Coloring2.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coloring2.MainMenu.Settings
{
    public class EnterBirthdayPopup : AbstractModalPopup
    {
        private static bool CheckAge(string yearStr)
        {
            var birthYear = Convert.ToDateTime(yearStr);
            var now = DateTime.Now;
            var ticks = DateTime.Now.Subtract(birthYear).Ticks;
            if (ticks < 0)
                return false;

            var years = new DateTime(ticks).Year - 1;
            var pastYearDate = birthYear.AddYears(years);  
            var months = 0;  
            for (var i = 1; i <= 12; i++)
            {
                if (pastYearDate.AddMonths(i) == now)
                {  
                    months = i;  
                    break;  
                }
                if (pastYearDate.AddMonths(i) >= now)
                {  
                    months = i - 1;  
                    break;  
                }
            }  
            var days = now.Subtract(pastYearDate.AddMonths(months)).Days;  
            //var Hours = now.Subtract(pastYearDate).Hours;  
            //var minutes = now.Subtract(pastYearDate).Minutes;  
            //var seconds = now.Subtract(pastYearDate).Seconds;
            var age = $"Age: {years} Year(s), {months} Month(s), {days} Day(s)";
            Debug.Log($"age: {age}");
            return years < 110 && years > 18;
        }

        public Action<EnterBirthdayPopup> Success;
        
        [SerializeField] private TextMeshProUGUI[] _inputFields;
        [SerializeField] private KeyboardButton[] _keys;

        [Space(10)] 
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Button _backspaceButton;

        private int _keyTapsCounter;

        protected override void Awake()
        {
            base.Awake();
            
            foreach (var key in _keys)
                key.Init(OnKeyTap);
            
            _backspaceButton.onClick.AddListener(OnBackspaceTap);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _backspaceButton.onClick.RemoveListener(OnBackspaceTap);
        }

        public override void Show()
        {
            base.Show();
            _title.text = LocalizationManager.GetLocalization(LocalizationManager.Keys.EnterBirthYear);
        }

        private void OnKeyTap(int keyValue)
        {
            if (_keyTapsCounter > 3)
                return;

            SoundsManager.PlayClick();
            _inputFields[_keyTapsCounter].text = keyValue.ToString();
            
            if (_keyTapsCounter == 3)
            {
                if (CheckAge(GetYearString()))
                {
                    Success?.Invoke(this);
                    return;
                }
                Close();
            }
            _keyTapsCounter++;
        }

        private string GetYearString()
        {
            var yearStr = "";
            foreach (var input in _inputFields) yearStr += input.text;
            return $"{yearStr}/06/01";
        }

        private void OnBackspaceTap()
        {
            if(_keyTapsCounter <= 0)
                return;
            
            _keyTapsCounter--;
            _keyTapsCounter = Mathf.Clamp(_keyTapsCounter, 0, 3);
            _inputFields[_keyTapsCounter].text = "";
            SoundsManager.PlayClick();
        }
    }
}
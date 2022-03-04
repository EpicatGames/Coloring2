using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "LDN/Create LDNAsset Instance")]
public class LDNAsset : ScriptableObject {
    public bool _useAAR;

    [SerializeField]
    public List<InputOutputData> _localizedNames;

    static void CreateExampleAsset() {
        var aLDNAsset = CreateInstance<LDNAsset>();

        AssetDatabase.CreateAsset(aLDNAsset, "Assets/LocalizedDisplayName/Editor/LDNAsset.asset");
        AssetDatabase.Refresh();
    }


    public void Add_ModifyToAsset(string lang, string cont) {
        bool has = false;
        for (int ii = 0; ii < _localizedNames.Count; ii++) {
            //            UnityEngine.Debug.Log("_aLDNAsset._localizedNames[ii].lang : " + ii + " : " + _aLDNAsset._localizedNames[ii]._lang);
            if (_localizedNames[ii]._lang == lang) {
                _localizedNames[ii]._cont = cont;
                has = true;
            }
        }
        if (!has) {
            _localizedNames.Add(new InputOutputData(lang, cont));
        }
    }
    public string Get_FromAsset(string lang, string defaultStr) {
        for (int ii = 0; ii < _localizedNames.Count; ii++) {
            //            UnityEngine.Debug.Log("_aLDNAsset._localizedNames[ii].lang : " + ii + " : " + _aLDNAsset._localizedNames[ii]._lang);
            if (_localizedNames[ii]._lang == lang) {
                return _localizedNames[ii]._cont;
            }
        }
        return defaultStr;
    }
    public void Del_FromAsset(string lang) {
        for (int ii = 0; ii < _localizedNames.Count; ii++) {
            //            UnityEngine.Debug.Log("_aLDNAsset._localizedNames[ii].lang : " + ii + " : " + _aLDNAsset._localizedNames[ii]._lang);
            if (_localizedNames[ii]._lang == lang) {
                _localizedNames.RemoveAt(ii);
            }
        }
    }
}

[System.Serializable]
public class InputOutputData {
    public string _lang;
    public string _cont;
    public InputOutputData(string lang, string cont) {
        _lang = lang;
        _cont = cont;
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using LdnLib;
using ksthip.UnityEditor.iOS.Xcode;

public class LDNWindow : EditorWindow {
    const string SIZE_WIDTH_KEY = "ExampleWindow size width";
    const string SIZE_HEIGHT_KEY = "ExampleWindow size height";

     public LDNAsset _aLDNAsset;

    List<List<string>> _content;
    Dictionary<string, int> _langIndex;
    int _selectLanguageIndex = 0;
    string _defaultStr;

    bool _isChanged;
    bool _useAAR;

    Vector2 scrollPos;

    private static LDNWindow _window;
    [MenuItem("Window/Localiaed Display Name")]
    static void Open() {
        _window = GetWindow<LDNWindow>();
        _window.titleContent = new GUIContent("Localized Name");
        _window.Show();
    }
    public static LDNWindow getInstance() {
        return _window;
    }

    void Apply() {
        // Debug.Log(" ++++ Apply ++++++ ");
        string pathLDN = Path.Combine(Application.dataPath, "LDN");
        // Process ====
        DirectoryInfo pathLDNdir = new DirectoryInfo(pathLDN);
        LdnCore.RD(pathLDN);

        LdnCore.MakeLocalizedFiles_ForAndroid(Application.dataPath, _content, _langIndex, LangDatas._folder_android);
        string pathAAR = Path.Combine(Application.dataPath, "Plugins", "Android", "LDN.aar");
        string androidPath = Path.Combine(Application.dataPath, "LDN", "Android");
        string tmpResPath = Path.Combine(androidPath, "res");
        if (_useAAR) {
            // jar
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "jar";
            // UnityEngine.Debug.Log("pathRes! :" + pathRes);
            startInfo.Arguments = "cf " + pathAAR + " -C " + androidPath + " .";
            // UnityEngine.Debug.Log("startInfo.Arguments! :" + startInfo.Arguments);
            Process process;
            try {
                process = Process.Start(startInfo);
            } catch (Exception ex) {
                UnityEngine.Debug.Log("jar... Exception :" + ex.Message);
                process = null;
            }
            int id = process.Id;
            Process tempProc = Process.GetProcessById(id);
            tempProc.WaitForExit();
            //UnityEngine.Debug.Log("tempProc.WaitForExit");
            // Plugins / Android / res 가 있다면 삭제
            LdnCore.RD(Path.Combine(Application.dataPath, "Plugins", "Android", "res"));
        } else {
            LdnCore.JustRes(Application.dataPath, tmpResPath, androidPath);
        }

        // LDN / Android 폴더 삭제
        LdnCore.RD(androidPath);

        LdnCore.MakeLocalizedFiles_ForIOS(Application.dataPath, _content, _langIndex, LdnLib.LangDatas._folder_ios);

        _aLDNAsset._useAAR = _useAAR;

        AssetDatabase.Refresh();
        EditorUtility.SetDirty(_aLDNAsset);
        AssetDatabase.SaveAssets();

        UnityEngine.Debug.Log("Done ---");

    }

    public float startVal = 0f;
    public float progress = 0f;
    public bool applying = false;
    void OnInspectorUpdate() {
        Repaint();
    }

    void OnGUI() {
        if (_content == null) {
            return;
        }

        // Apply 시에 프로그래스바 나오게 함.
        if (applying) {
//            Debug.Log("progress : " + progress + "  , startVal: " + startVal);
            if (progress < 2f)
                EditorUtility.DisplayProgressBar("Localized Display Name", "Progressing...", progress / 2f);
            else {
                EditorUtility.ClearProgressBar();
                applying = false;
                startVal = 0f;
                progress = 0f;
            }
            progress = (float)(EditorApplication.timeSinceStartup - startVal);
        }

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("");
        if (GUILayout.Button("Apply")) {
            _isChanged = false;
            applying = true;
            startVal = 0f;
            progress = 0f;
            startVal = (float)EditorApplication.timeSinceStartup;
            Apply();
        }
        GUILayout.EndHorizontal();

        // EditorGUILayout.LabelField("");

        EditorGUIUtility.labelWidth = 10f;

        GUILayout.BeginHorizontal();
        GUILayout.Space(30f);
        EditorGUILayout.LabelField("Language", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("iOS", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Localized Name", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        //-----
        GUIStyle horizontalLine;
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;
        GUILayout.Box(GUIContent.none, horizontalLine);
        //-----

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height-160));

        DisplayDefault();
        GUILayout.Box(GUIContent.none, horizontalLine);
        for (int ii = 1; ii < _content.Count; ii++) {
            DisplayList(ii);
        }

        //-----
        GUILayout.Box(GUIContent.none, horizontalLine);
        //-----


        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        GUIStyle addPopupStyle = new GUIStyle();
        addPopupStyle.fontSize = 10;
        _selectLanguageIndex = EditorGUILayout.Popup(_selectLanguageIndex, LdnLib.LangDatas._addLanguageList);
        if (_selectLanguageIndex != 0) {
            int selectedLangIndex = _selectLanguageIndex;
            string selectedLang = LdnLib.LangDatas._langs[selectedLangIndex];
            // 이미 있는 것이 아닌지 확인
            for (int ii=0; ii<_content.Count; ii++) {
                List<string> unit = _content[ii];
                if (unit[0] == selectedLang) {
                    _selectLanguageIndex = 0;
                    if (EditorUtility.DisplayDialog("Invalid Request", "You already have Localized Display Name : " + selectedLang, "OK")) {
                    }
                    return;
                }
            }
            List<string> aCont = new List<string>();
            aCont.Add(selectedLang);
            aCont.Add("");
            _content.Add(aCont);

            _selectLanguageIndex = 0;
        }
        GUILayout.Space(60f);
        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");

        _useAAR = EditorGUILayout.ToggleLeft("Use AAR(Android library)", _useAAR);
        EditorGUILayout.HelpBox("To use aar to build Android, you need to install the \"jar\" command and set \"jar\" path in your system PATH.", MessageType.Info);
    }

    void DisplayDefault() {
        List<string> aCont = _content[0];
        int langIndex = _langIndex[aCont[0]];
        string f_and = LdnLib.LangDatas._folder_android[langIndex];
        string f_ios = LdnLib.LangDatas._folder_ios[langIndex];

        if (_defaultStr != Application.productName) {
            // EditorPrefs.SetString("Default(English)", _defaultStr);
            _aLDNAsset.Add_ModifyToAsset("Default(English)", _defaultStr);
            _isChanged = true;
            // UnityEngine.Debug.Log("Changed!! _defaultStr:" + _defaultStr + " , Application.productName : " + Application.productName);
            aCont[1] = Application.productName;
            // 저장.
            // EditorPrefs.SetString(aCont[0], Application.productName);
            _aLDNAsset.Add_ModifyToAsset(aCont[0], Application.productName);
            _defaultStr = Application.productName;
        }


        GUILayout.BeginHorizontal();
        GUILayout.Space(30f);
        EditorGUILayout.LabelField(aCont[0]);
        EditorGUILayout.LabelField(f_and);
        EditorGUILayout.LabelField(f_ios);
        EditorGUILayout.LabelField(Application.productName, EditorStyles.whiteBoldLabel);

        GUILayout.EndHorizontal();
    }

    void DisplayList(int index) {
        List<string> aCont = _content[index];
        int langIndex = _langIndex[aCont[0]];
        string f_and = LdnLib.LangDatas._folder_android[langIndex];
        string f_ios = LdnLib.LangDatas._folder_ios[langIndex];

        GUILayout.BeginHorizontal();
        if (true == GUILayout.Button("X", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(20f), GUILayout.MaxWidth(20f))) {
            _isChanged = true;
            // Debug.Log(" + lang : " + aCont[0]);
            _content.RemoveAt(index);
            // EditorPrefs.DeleteKey(aCont[0]);
            _aLDNAsset.Del_FromAsset(aCont[0]);
        }

        EditorGUILayout.LabelField(aCont[0]);
        EditorGUILayout.LabelField(f_and);
        EditorGUILayout.LabelField(f_ios);
        string editedCont = EditorGUILayout.TextField(aCont[1]);
        if (aCont[1] != editedCont) {
            _isChanged = true;
            aCont[1] = editedCont;
            // 저장.
            // EditorPrefs.SetString(aCont[0], editedCont);
            _aLDNAsset.Add_ModifyToAsset(aCont[0], editedCont);
        }
        GUILayout.EndHorizontal();
    }

    void OnEnable() {
        _isChanged = false;
        var width = EditorPrefs.GetFloat(SIZE_WIDTH_KEY, 600);
        var height = EditorPrefs.GetFloat(SIZE_HEIGHT_KEY, 400);
        position = new Rect(position.x, position.y, width, height);

        // asset 불러 들이기.
        _aLDNAsset = AssetDatabase.LoadAssetAtPath<LDNAsset>("Assets/LocalizedDisplayName/Editor/LDNAsset.asset");
        // UnityEngine.Debug.Log("_aLDNAsset : " + _aLDNAsset);

        _useAAR = _aLDNAsset._useAAR;
         
        // _defaultStr = EditorPrefs.GetString("Default(English)", Application.productName);
        _defaultStr = _aLDNAsset.Get_FromAsset("Default(English)", Application.productName);
        // EditorPrefs.SetString("Default(English)", _defaultStr);
        _aLDNAsset.Add_ModifyToAsset("Default(English)", _defaultStr);

        if (_content == null) {
            _content = new List<List<string>>();
            // lang Index Dictionary 에 값을 넣어줌(랭키로 인덱스르 빠르게 접근하기 위함)
            _langIndex = new Dictionary<string, int>();
            for (int ii=0; ii< LdnLib.LangDatas._langs.Length; ii++ ) {
                string lan = LdnLib.LangDatas._langs[ii];
                _langIndex.Add(lan, ii);
                //string cont = EditorPrefs.GetString(lan, "");
                string cont = _aLDNAsset.Get_FromAsset(lan, "");
                if (cont != "") {
                    List<string> aCont = new List<string>();
                    aCont.Add(lan);
                    aCont.Add(cont);
                    _content.Add(aCont);
                }
            }
        }
        
        // content 구성
    }

    void OnDisable() {
        EditorPrefs.SetFloat(SIZE_WIDTH_KEY, position.width);
        EditorPrefs.SetFloat(SIZE_HEIGHT_KEY, position.height);
    }

    void OnLostFocus() {
        if (_isChanged) {
            if (EditorUtility.DisplayDialog("Unapplied settings", "Unapplied Localized Display Name settings", "Apply", "Cancel")) {
                _isChanged = false;
                applying = true;
                startVal = 0f;
                progress = 0f;
                startVal = (float)EditorApplication.timeSinceStartup;
                Apply();
            }
        }
    }

    public static void AddLocalizedStringsIOS(string projectPath, string localizedDirectoryPath) {
        DirectoryInfo dir = new DirectoryInfo(localizedDirectoryPath);
        if (!dir.Exists)
            return;
        List<string> locales = new List<string>();
        var localeDirs = dir.GetDirectories("*.lproj", SearchOption.TopDirectoryOnly);
        foreach (var sub in localeDirs)
            locales.Add(Path.GetFileNameWithoutExtension(sub.Name));
        //        Debug.Log("locales : " + locales.Count);

        AddLocalizedStringsIOS(projectPath, localizedDirectoryPath, locales);
    }

    static int RemoveLines(string path, Predicate<string> remove) {
        var removed = 0;
        var lines = File.ReadAllLines(path);
        using (var output = new StreamWriter(path)) {
            foreach (var line in lines) {
                if (remove(line)) {
                    removed++;
                } else {
                    output.WriteLine(line);
                }
            }
        }
        return removed;
    }

    public static void AddLocalizedStringsIOS(string projectPath, string localizedDirectoryPath, List<string> validLocales) {
        string projPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        // 저장 화일 읽어오기
        LDNAsset aLDNAsset;
        aLDNAsset = AssetDatabase.LoadAssetAtPath<LDNAsset>("Assets/LocalizedDisplayName/Editor/LDNAsset.asset");

        foreach (var locale in validLocales) {
            string lproj_name = locale + ".lproj";
            string fileName = projectPath + "/" + PBXProject.GetUnityTestTargetName() + "/" + lproj_name + "/" + "InfoPlist.strings";
            string title = "";
            for (int ii = 0; ii < LangDatas._folder_ios.Length; ii++) {
                if (lproj_name == LangDatas._folder_ios[ii]) {
                    int langIndex = ii;
                    string langKey = LdnLib.LangDatas._langs[ii];
                    title = aLDNAsset.Get_FromAsset(langKey, "");
                }
            }
            if (LdnCore.MkBD(fileName, title, locale, localizedDirectoryPath, projectPath)) {
                string fileRelatvePath = string.Format("Unity-iPhone/{0}.lproj/InfoPlist.strings", locale);
                proj.AddLocalization("InfoPlist.strings", locale, fileRelatvePath);
            }
        }

        proj.WriteToFile(projPath);
    }
}

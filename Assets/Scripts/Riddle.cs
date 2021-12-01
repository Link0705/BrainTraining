using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Riddle : ScriptableObject {
    public const string RED = "#b83e39";
    public const int NONE = 0;
    public const int NUMBER = 1;
    public const int SUBMIT = 2;

    public int resultType;
    public Sprite image;
    public string description;
    public bool interactive;
    public GameObject interactiveArea;

    virtual public void OnEnable() {
    }

    virtual public bool checkResult() { return false; }

    virtual public bool checkResult(int result) { return false; }

    virtual public bool checkResult(string result) { return false; }

}
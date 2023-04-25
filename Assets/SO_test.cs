using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SO_test : ScriptableObject
{
    
}

[System.Serializable]
[CreateAssetMenu(fileName = "newSOtest", menuName = "test/version1", order = 1)]
public class version1 : SO_test
    {
    public string version1Name;
}
[System.Serializable]
[CreateAssetMenu(fileName = "newSOtest2", menuName = "test/version2", order = 1)]
public class version2 : SO_test
{
    public string version2Name;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLogger
{
    private string _logName = "";

    public MLogger(string name)
    {
        Register(name);
    }
    private void Register(string name)
    {
        _logName = name;
    }
    public void Log(params object[] args)
    {
        string message = _logName + ": ";
        foreach (var arg in args)
        {
            message += arg.ToString();
        }
        Debug.Log(message);
    }
    public void Warning(params object[] args)
    {
        string message = _logName + ": ";
        foreach (var arg in args)
        {
            message += arg.ToString();
        }
        Debug.LogWarning(message);
    }
    public void Error(params object[] args)
    {
        string message = _logName + ": ";
        foreach (var arg in args)
        {
            message += arg.ToString();
        }
        Debug.LogError(message);
    }
}

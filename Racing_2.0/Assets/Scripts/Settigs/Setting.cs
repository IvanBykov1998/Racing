using System;
using UnityEngine;

public abstract class Setting : ScriptableObject
{
    [SerializeField] private string title;
    public string Title => title;

    public virtual bool IsMinValue { get; }
    public virtual bool IsMaxValue { get; }

    public virtual void SetNextValue() { }
    public virtual void SetPreviousValue() { }
    public virtual object GetValue() { return default; }

    public virtual string GetStringValue() { return string.Empty; }

    public virtual void Apply() { }

    public virtual void Load() { }
}
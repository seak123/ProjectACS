using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode
{
    public virtual float Delay
    {
        get
        {
            return _delay;
        }
        set
        {
            _delay = value;
        }
    }
    protected float _delay = 0;
    private List<IPerformNode> _followers;
    private List<IPerformNode> _campanions;

    public BaseNode()
    {
        _followers = new List<IPerformNode>();
        _campanions = new List<IPerformNode>();
    }
    public virtual void AddFollower(IPerformNode node)
    {
        _followers.Add(node);
    }
    public virtual void AddCompanion(IPerformNode node)
    {
        _campanions.Add(node);
    }
    public virtual List<IPerformNode> GetFollowers()
    {
        return _followers;
    }
    public virtual List<IPerformNode> GetCompanions()
    {
        return _campanions;
    }
}

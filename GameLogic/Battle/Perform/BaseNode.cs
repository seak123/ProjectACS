using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode
{
    private List<IPerformNode> _followers;
    private List<IPerformNode> _campanions;
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

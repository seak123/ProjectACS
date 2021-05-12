using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public interface IPerformNode
{
    public PerformNodeType Type { get; }
    public float Delay {get;set;}
    public void InjectData(LuaTable table);
    public void Construct();
    public void Play(float deltaTime);
    public bool IsFinished();
    public void AddFollower(IPerformNode node);
    public void AddCompanion(IPerformNode node);
    public List<IPerformNode> GetFollowers();
    public List<IPerformNode> GetCompanions();
}

public enum PerformNodeType
{
    Move = 1,
    Anim = 2,
    Damage = 3
}

public struct DelayPerformNode
{
    float delayTime;
    IPerformNode performNode;
}

public class BattlePerformer
{
    private bool bFinish = true;
    private List<IPerformNode> _playNodes;
    private List<DelayPerformNode> _delayNodes;

    public bool Finished
    {
        get
        {
            return bFinish;
        }
    }
    public void Perform(LuaTable rawTable)
    {
        bFinish = false;
        _playNodes = new List<IPerformNode>();
        ParseRawTable(rawTable);
    }

    public void OnUpdate(float deltaTime)
    {
        if (!bFinish)
        {
            if (_delayNodes.Count >0)
            {
                for (int i=_delayNodes.Count-1;i>=0;--i)
                {
                    _delayNodes[i].delayTime -= deltaTime;
                    if(_delayNodes[i].deltaTime<=0)
                    {
                        _delayNodes.performNode.Construct();
                        _playNodes.Add(_delayNodes[i].performNode);
                        _delayNodes.RemoveAt(i);
                    }
                }
            }
            if (_playNodes.Count > 0)
            {
                List<IPerformNode> readyNodes = new List<IPerformNode>();
                for (int i = _playNodes.Count - 1; i >= 0; --i)
                {
                    _playNodes[i].Play(deltaTime);
                    if (_playNodes[i].IsFinished())
                    {
                        var nextNodes = _playNodes[i].GetFollowers();
                        foreach (var nextNode in nextNodes)
                        {
                            nextNode.Construct();
                            readyNodes.Add(nextNode);
                            var companyNodes = nextNode.GetCompanions();
                            foreach (var coNode in companyNodes)
                            {
                                if(coNode.delay>0)
                                {
                                    var delayData = new DelayPerformNode();
                                    delayData.deltaTime = coNode.delay;
                                    delayData.performNode = coNode;
                                    _delayNodes.Add(delayData);
                                }else{
                                    coNode.Construct();
                                    readyNodes.Add(coNode);
                                }
                            }
                        }
                        _playNodes.RemoveAt(i);
                    }
                }
                _playNodes.AddRange(readyNodes);
            }
            
            if(_delayNodes.Count==0&&_playNodes.Count==0)
            {
                bFinish = true;
            }
        }
    }

    private void ParseRawTable(LuaTable rawTable)
    {
        var rootNode = ParseNode(rawTable);
        rootNode.Construct();
        _playNodes.Add(rootNode);
        var rootCompanions = rootNode.GetCompanions();
        for (int i = 0; i < rootCompanions.Count; ++i)
        {
            rootCompanions[i].Construct();
            _playNodes.Add(rootCompanions[i]);
        }
    }

    private IPerformNode ParseNode(LuaTable table)
    {
        IPerformNode node = CreateNode((PerformNodeType)table.Get<int>("nodeType"));
        node.InjectData(table);
        var delay = table.Get<float>("delay");
        node.Delay = delay!=null?delay:0.f;
        var followers = table.Get<List<LuaTable>>("followers");
        var companions = table.Get<List<LuaTable>>("companions");
        for (int i = 0; i < followers.Count; ++i)
        {
            node.AddFollower(ParseNode(followers[i]));
        }
        for (int i = 0; i < companions.Count; ++i)
        {
            node.AddCompanion(ParseNode(companions[i]));
        }
        return node;
    }

    private IPerformNode CreateNode(PerformNodeType nodeType)
    {
        switch (nodeType)
        {
            case PerformNodeType.Move:
                return new MoveNode();
            case PerformNodeType.Damage:
                return new DamageNode();
            case PerformNodeType.Anim:
                return new AnimNode();
            default:
                return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public interface IPerformNode
{
    public void InjectData(LuaTable table);
    public void Play(float deltaTime);
    public bool IsFinished();
    public void AddFollowNode(IPerformNode node);
    public void AddCompanyNode(IPerformNode node);
    public List<IPerformNode> GetFollowNodes();
    public List<IPerformNode> GetCompanyNodes();
}

public class BattlePerformer
{
    private bool bFinish = false;
    private List<IPerformNode> _playNodes;

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
            if (_playNodes.Count > 0)
            {
                List<IPerformNode> readyNodes = new List<IPerformNode>();
                for (int i = _playNodes.Count - 1; i >= 0; --i)
                {
                    _playNodes[i].Play(deltaTime);
                    if (_playNodes[i].IsFinished())
                    {
                        var nextNodes = _playNodes[i].GetFollowNodes();
                        foreach (var nextNode in nextNodes)
                        {
                            readyNodes.Add(nextNode);
                            var companyNodes = nextNode.GetCompanyNodes();
                            foreach (var coNode in companyNodes)
                            {
                                readyNodes.Add(coNode);
                            }
                        }
                        _playNodes.RemoveAt(i);
                    }
                }
                _playNodes.AddRange(readyNodes);
            }
            else
            {
                bFinish = true;
            }
        }
    }

    private void ParseRawTable(LuaTable rawTable)
    {

    }
}

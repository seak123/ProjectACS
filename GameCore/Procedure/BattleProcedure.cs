using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface ILuaSession
{
    void EnterGame();
    int GetUnitProperty(int uid, string name);
    Vector2Int GetUnitCoord(int uid);
    int GetUnitDirection(int uid);
    bool IsGridMovable(int uid, Vector2Int vector);
    List<LuaTable> GetReachableRegion(int uid, int distance);
    List<LuaTable> GetPathToGoal(int uid, Vector2Int vector);
    void UpdateReadyOrder(bool isReady, BattleOrder order);
    void SelectUnit(int uid);
}

public class BattleProcedure : IProcedure
{
    private MLogger _logger = new MLogger("BattleProcedure");

    private static BattleSession _curSession;
    private static ILuaSession _luaSession;

    private static BattleSessionVO _curSessionVO;

    private GameObject _curShowCard;

    #region Properties
    public static BattleSession CurSession
    {
        get
        {
            return _curSession;
        }
    }

    public static ILuaSession CurLuaSession
    {
        get
        {
            return _luaSession;
        }
    }

    public static BattleSessionVO CurSessionVO
    {
        get
        {
            return _curSessionVO;
        }
        set
        {
            _curSessionVO = value;
        }
    }
    #endregion

    public void OnEnter()
    {
        _logger.Log("enter battle");

        ScenesManager
            .Instance
            .LoadScene("TestScene1",
            () =>
            {
                // Init Presentation layer first
                _logger.Log("init session");
                _curSession = new BattleSession();
                _curSession.InitSession();

                // Init Data layer to drive session
                var luaSessionTable = LuaManager.Instance.LuaEnv.Global.Get<LuaTable>("BattleSession");
                var curSession = luaSessionTable.Get<LuaFunction>("StartBattle").Call(CurSessionVO)[0] as LuaTable;
                _luaSession = curSession.Cast<ILuaSession>();

                _luaSession.EnterGame();
                _curSession.InitBattleUI();
            });
        EventManager.Instance.On(EventConst.ON_SHOW_CARD_DETAIL, OnShowCardDetail);
    }

    public void OnUpdate()
    {
        if (_curSession != null)
        {
            _curSession.OnUpdate(Time.deltaTime);
        }
    }

    public void OnLeave()
    {
        EventManager.Instance.Off(EventConst.ON_SHOW_CARD_DETAIL, OnShowCardDetail);
    }

    private void OnShowCardDetail(object arg1, object arg2)
    {
        bool bShow = (bool)arg1;

        int cardId = int.Parse(arg2.ToString());
        if (bShow)
        {
            _curShowCard = ResourceManager
                    .Instance
                    .LoadUIPrefab(UIConst.STANDARD_CARD, UILayer.Normal_4);
            var luaOperation = _curShowCard.GetComponent<LuaOperation>();
            luaOperation.luaBehaviour.Cast<IStandardCard>().InitCard(cardId);
        }
        else
        {
            if (_curShowCard != null)
            {
                GameObject.Destroy(_curShowCard);
            }
        }
    }
}

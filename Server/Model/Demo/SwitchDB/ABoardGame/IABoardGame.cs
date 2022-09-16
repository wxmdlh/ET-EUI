using System.Collections.Generic;

namespace ET
{
    public interface IABoardGame
    {
        ETTask Insert(ABoardGame aBoardGame, Session session);
        ETTask UpdateFieldContext(ABoardGame aBoardGame, Session session, string field, string updateContext);
        ETTask Remove(ABoardGame aBoardGame, Session session);
    }
}
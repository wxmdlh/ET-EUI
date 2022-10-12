using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType(typeof(RoleInfo))]
    public class RoleInfosComponent: Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 存储游戏服务器下发的角色信息
        /// </summary>
        public List<RoleInfo> RoleInfos = new List<RoleInfo>();

        /// <summary>
        /// 当前玩家所选择进入游戏的角色的Id
        /// </summary>
        public long CurrentRoleId = 0;
    }
}
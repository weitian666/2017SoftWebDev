namespace ELearning.Entities.Common
{
    /// <summary>
    /// 用于定义业务对象状态
    /// </summary>
    public enum BusinessStatus
    {
        Sleep,      // 休眠
        Editting,   // 编辑中
        InProcess,  // 进行中
        Waitting,   // 待审核
        Modify,     // 变更
        Transfered, // 移交
        Stoppe,     // 停止
        Defeated,   // 作废
        Finished    // 已完成
    }
}
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace OutStockRdIntoBarSys
{
    public class ButtonEvents : AbstractBillPlugIn
    {
        Generate generate= new Generate();

        public override void BarItemClick(BarItemClickEventArgs e)
        {
            //订单退回操作
            base.BarItemClick(e);

            //定义获取表头信息对像
            var docScddIds1 = View.Model.DataObject;
            //获取表头中单据编号信息(注:这里的BillNo为单据编号中"绑定实体属性"项中获得)
            var dhstr = docScddIds1["BillNo"].ToString();

            //销售出库单-反审核时执行
            if (e.BarItemKey == "tbReject")
            {
                //执行反审核相关操作
                var result = generate.Reject(dhstr);
                //todo:当出现异常时才提示
                View.ShowMessage(result == "Finish" ? "操作完成" : $"出库数据与条码系统交互操作异常,原因:'{result}'");
            }
            //销售出库单-审核进执行
            else if (e.BarItemKey == "tbApprove")
            {
                //执行审核相关操作
                var result = generate.Approve(dhstr);
                //todo:当出现异常时才提示
                View.ShowMessage(result == "Finish" ? "操作完成" : $"出库数据与条码系统交互操作异常,原因:'{result}'");
            }
        }
    }
}

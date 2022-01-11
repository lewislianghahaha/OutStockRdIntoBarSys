namespace OutStockRdIntoBarSys
{
    //相关SQL语句
    public class Sqllist
    {
        private string _result;

        /// <summary>
        /// 反审核使用
        /// 作用:将条码库.[T_K3SalesOut]对应的'FRemarkid' 'Flastop_time'进行更新
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Reject(string orderno)
        {
            _result = $@"
                            UPDATE T_K3SalesOut SET FRemarkid=1,Flastop_time=GETDATE()
                            WHERE doc_no='{orderno}'
                        ";

            return _result;
        }

        /// <summary>
        /// 审核使用
        /// 作用: (通过View_OUTSTOCKPda视图获取内容)
        /// 1)若判断所查出库单号在[T_K3SalesOut]存在,就更新‘qty_req’‘FRemarkid’‘Flastop_time’这三项
        /// 2)若没有记录,就进行插入(注:插入时不用对Flastop_time进行插入值,只需插入FCreate_time值便可)
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Approve(string orderno)
        {
            _result = $@"
                            
                        ";

            return _result;
        }

        /// <summary>
        /// 根据销售出库单查询View_OUTSTOCKPda内的记录
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Get_SearchK3ViewRecord(string orderno)
        {
            _result = $@"
                            SELECT * FROM dbo.View_OUTSTOCKPda A WHERE doc_no='{orderno}'
                        ";

            return _result;
        }

        /// <summary>
        /// 根据销售出库单查询T_K3SalesOut内的记录
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Get_SearchBarRecord(string orderno)
        {
            _result = $@"SELECT * FROM T_K3SalesOut WHERE doc_no='{orderno}'";
            return _result;
        }

        /// <summary>
        /// 根据表名获取查询表体语句(更新时使用) 只显示TOP 1记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string SearchUpdateTable(string tableName)
        {
            _result = $@"
                          SELECT Top 1 a.*
                          FROM {tableName} a
                        ";
            return _result;
        }

        /// <summary>
        /// 条码库.T_K3SalesOut更新语句
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string UpdateEntry(string tablename)
        {
            switch (tablename)
            {
                case "T_K3SalesOut":
                    _result = @"UPDATE dbo.T_K3SalesOut SET qty_req=@qty_req,FRemarkid=@FRemarkid,Flastop_time=@Flastop_time
                                WHERE doc_no=@doc_no and sku_no=@sku_no";
                    break;
            }
            return _result;
        }

    }
}

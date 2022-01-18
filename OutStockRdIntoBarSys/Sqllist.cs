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

        #region 审核使用
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
            _result = $@"
                            SELECT * FROM T_K3SalesOut WHERE doc_no='{orderno}'
                        ";
            return _result;
        }

        /// <summary>
        /// 根据typeid获取对应的模板表记录 只显示TOP 1记录(更新使用)
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public string SearchUpdateTable(int typeid)
        {
            //0:更新记录 1:更新FRemarkid=1
            if (typeid == 0)
            {
                _result = $@"
                          SELECT Top 1 a.doc_no,a.sku_no,a.qty_req,a.FRemarkid,a.Flastop_time
                          FROM T_K3SalesOut a
                        ";
            }
            else
            {
                _result = $@"
                          SELECT Top 1 a.doc_no,a.sku_no,a.FRemarkid,a.Flastop_time
                          FROM T_K3SalesOut a
                        ";
            }

            return _result;
        }

        /// <summary>
        /// 条码库.T_K3SalesOut更新语句
        /// </summary>
        /// <param name="typeid">0:更新记录 1:更新FRemarkid=1</param>
        /// <returns></returns>
        public string UpdateEntry(int typeid)
        {
            switch (typeid)
            {
                case 0:
                    _result = @"
                                UPDATE dbo.T_K3SalesOut SET qty_req=@qty_req,FRemarkid=@FRemarkid,Flastop_time=@Flastop_time
                                WHERE doc_no=@doc_no and sku_no=@sku_no";
                    break;
                case 1:
                    _result = @"
                                  UPDATE dbo.T_K3SalesOut SET FRemarkid=@FRemarkid,Flastop_time=@Flastop_time
                                  WHERE doc_no=@doc_no and sku_no=@sku_no
                                ";
                    break;
            }
            return _result;
        }

        #endregion
    }
}

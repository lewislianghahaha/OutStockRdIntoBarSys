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
        /// 根据物料编码列表查询View_Material内的记录
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public string Get_SearchK3ViewMaterialRecord(string materiallist)
        {
            _result = $@"SELECT * FROM dbo.View_Material where sku_no in ({materiallist})";
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
        /// 根据物料查询T_K3Material记录
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public string Get_SearchMaterialRecord(string materiallist)
        {
            _result = $@"
                            SELECT * FROM T_K3Material WHERE sku_no in ({materiallist})
                        ";
            return _result;
        }

        /// <summary>
        /// 根据typeid获取对应的模板表记录 只显示TOP 1记录(更新使用)
        /// </summary>
        /// <param name="typeid">0:更新记录 1:更新FRemarkid=1 2:更新T_K3Material记录</param>
        /// <returns></returns>
        public string SearchUpdateTable(int typeid)
        {
            //0:更新记录 1:更新FRemarkid=1
            if (typeid == 0)
            {
                _result = $@"
                          SELECT Top 1 a.doc_no,a.sku_no,a.qty_req,a.line_no,a.FRemarkid,a.Flastop_time,a.op_time
                          FROM T_K3SalesOut a
                        ";
            }
            else if (typeid == 1)
            {
                _result = $@"
                          SELECT Top 1 a.doc_no,a.sku_no,a.FRemarkid,a.Flastop_time
                          FROM T_K3SalesOut a
                        ";
            }
            else
            {
                _result = $@"
                            SELECT Top 1 a.sku_no,a.sku_desc,a.sku_desc_en,
							             a.baseunit_desc,a.stockunit_desc,a.pack_spec ,
							             a.pack_gz ,a.pack_xz ,
							             a.label_number ,a.label_name ,
							             a.sku_catalog ,a.pack_jz ,
							             a.化学品分类,a.配比,a.保质期,a.主要成份,
							             a.储存温度,a.毛重,a.项目名称,a.客户端物料编号,a.配比标题,
							             a.FCreate_time,a.Flastop_time,a.标签打印名称
                            FROM T_K3Material a
                          ";
            }

            return _result;
        }

        /// <summary>
        /// 条码库.T_K3SalesOut更新语句
        /// </summary>
        /// <param name="typeid">0:更新记录 1:更新FRemarkid=1 2:更新T_K3Material记录</param>
        /// <returns></returns>
        public string UpdateEntry(int typeid)
        {
            switch (typeid)
            {
                case 0:
                    _result = @"
                                UPDATE dbo.T_K3SalesOut SET qty_req=@qty_req,line_no=@line_no,FRemarkid=@FRemarkid,Flastop_time=@Flastop_time,op_time=@op_time,customer_desc=@customer_desc
                                WHERE doc_no=@doc_no and sku_no=@sku_no";
                    break;
                case 1:
                    _result = @"
                                  UPDATE dbo.T_K3SalesOut SET FRemarkid=@FRemarkid,Flastop_time=@Flastop_time
                                  WHERE doc_no=@doc_no and sku_no=@sku_no
                                ";
                    break;
                case 2:
                    _result = @"
                                    UPDATE dbo.T_K3Material SET sku_desc=@sku_desc,sku_desc_en=@sku_desc_en,
							                baseunit_desc=@baseunit_desc,stockunit_desc=@stockunit_desc,pack_spec=@pack_spec ,
							                pack_gz=@pack_gz ,pack_xz=@pack_xz ,
							                label_number=@label_number ,label_name=@label_name ,
							                sku_catalog=@sku_catalog ,pack_jz=@pack_jz ,
							                化学品分类=@化学品分类,配比=@配比,保质期=@保质期,主要成份=@主要成份,
							                储存温度=@储存温度,毛重=@毛重,项目名称=@项目名称,客户端物料编号=@客户端物料编号,
                                            配比标题=@配比标题,
							                Flastop_time=@Flastop_time,标签打印名称=@标签打印名称
                                    WHERE sku_no=@sku_no
                               ";
                    break;
            }
            return _result;
        }

        #endregion
    }
}

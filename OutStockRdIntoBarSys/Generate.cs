using System;
using System.Data;
using System.Data.SqlClient;

namespace OutStockRdIntoBarSys
{
    //后台处理(包含数据库连接)
    public class Generate
    {
        Sqllist sqllist=new Sqllist();
        Tempdt tempdt=new Tempdt();

        /// <summary>
        /// 反审核使用
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Reject(string orderno)
        {
            var result = "Finish";

            try
            {
                Generdt(sqllist.Reject(orderno),1);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 审核使用
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public string Approve(string orderno)
        {
            var result = "Finish";

            var inserttemp = tempdt.InsertBarTemp();
            var uptemp = tempdt.UpBarTemp();
            //保存设置条码表不显示的记录
            var canneltemp = tempdt.CannelTemp();

            try
            {
                //根据‘销售出库单号’获取K3-View_OUTSTOCKPda视图记录
                var k3ViewDt = UseSqlSearchIntoDt(0, sqllist.Get_SearchK3ViewRecord(orderno)).Copy();
                //根据‘销售出库单号’获取条码库.T_K3SalesOut相关记录
                var barCode = UseSqlSearchIntoDt(1, sqllist.Get_SearchBarRecord(orderno)).Copy();

                //需k3ViewDt有记录才可以继续
                if (k3ViewDt.Rows.Count > 0)
                {
                    //若从T_K3SalesOut表没有找到相关记录,就使用k3ViewDt进行插入,有就进行更新
                    if (barCode.Rows.Count == 0)
                    {
                        inserttemp.Merge(InsertDtIntoInsertTempDt(k3ViewDt, inserttemp));
                    }
                    //若在T_K3SalesOut表有记录,就需要先使用‘单据编号’及‘物料编码’放到T_K3SalesOut表内作判断,若存在即更新;反之插入记录
                    //注:将当前"判断行"插入至临时表,而不是每次循环都将k3ViewDt表记录插入
                    else
                    {
                        //获取k3ViewDt的表结构
                        var k3Tempdt = k3ViewDt.Clone();

                        //检测barCode内的记录是否不在k3ViewDt存在,是:则插入至cannelTempdt内,在最后将这些记录的FRemarkid设置为1
                        //作用:将K3删除的记录在条码表内设置为不可见
                        foreach (DataRow rows in barCode.Rows)
                        {
                            var dtlrows = k3ViewDt.Select("doc_no='" + Convert.ToString(rows[0]) + "' and sku_no='" + Convert.ToString(rows[8]) + "'").Length;

                            //若存在则继续,反之,即插入至临时表
                            if (dtlrows > 0) continue;
                            k3Tempdt.ImportRow(rows);
                            //若存在,即插入至canneltemp表内
                            canneltemp.Merge(InsertDtIntoCannelTempDt(k3Tempdt, canneltemp));
                            //当前行循环结束后将行记录删除;令k3Tempdt只记录当前循环行信息,不包括以前循环的记录
                            k3Tempdt.Rows.Clear();
                        }

                        //var a3 = canneltemp.Copy();

                        //检测k3ViewDt内的记录是否在barCode内存在,是:更新  否:插入
                        foreach (DataRow rows in k3ViewDt.Rows)
                        {
                            var dtlrows = barCode.Select("doc_no='" + Convert.ToString(rows[0]) + "' and sku_no='" + Convert.ToString(rows[8]) + "'").Length;

                            //将"当前"循环的rows行插入至临时表(K3TempDt) 注:需插入的列与临时表一致(包括列顺序),才可使用ImportRow()方法
                            k3Tempdt.ImportRow(rows);
                            //var a = k3Tempdt.Copy();

                            //若存在,就更新
                            if (dtlrows > 0)
                            {
                                uptemp.Merge(InsertDtIntoUpdateTempdt(k3Tempdt, uptemp));
                            }
                            //反之进行插入操作
                            else
                            {
                                inserttemp.Merge(InsertDtIntoInsertTempDt(k3Tempdt, inserttemp));
                            }
                            //当前行循环结束后将行记录删除;令k3Tempdt只记录当前循环行信息,不包括以前循环的记录
                            k3Tempdt.Rows.Clear();
                        }
                        //var a1 = uptemp.Copy();
                        //var b1 = inserttemp.Copy();
                    }
                    //将得出的结果进行插入或更新
                    if (inserttemp.Rows.Count > 0)
                        ImportDtToDb("T_K3SalesOut", inserttemp);
                    if (uptemp.Rows.Count > 0)
                        UpdateDbFromDt("T_K3SalesOut", uptemp, 0);
                    //将需要取消的记录更新T_K3SalesOut.FRemarkid
                    if (canneltemp.Rows.Count > 0)
                        UpdateDbFromDt("T_K3SalesOut", canneltemp, 1);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 将K3记录插入至临时表(插入使用)
        /// </summary>
        /// <returns></returns>
        private DataTable InsertDtIntoInsertTempDt(DataTable k3ViewDt, DataTable inserttemp)
        {
            //执行插入,将k3ViewDt的值插入至临时表内(用于插入至条码表)
            foreach (DataRow rows in k3ViewDt.Rows)
            {
                var newrow = inserttemp.NewRow();
                newrow[0] = rows[0];     //doc_no
                newrow[1] = rows[1];     //doc_catalog
                newrow[2] = rows[2];     //op_time
                newrow[3] = rows[3];     //line_no
                newrow[4] = rows[4];     //doc_status
                newrow[5] = rows[5];     //customer_no
                newrow[6] = rows[6];     //FNAME
                newrow[7] = rows[7];     //customer_desc
                newrow[8] = rows[8];     //sku_no
                newrow[9] = rows[9];     //sku_desc
                newrow[10] = rows[10];   //sku_catalog
                newrow[11] = rows[11];   //unit
                newrow[12] = rows[12];   //qty_req
                newrow[13] = rows[13];   //pack_spec
                newrow[14] = rows[14];   //pack_gz
                newrow[15] = rows[15];   //pack_xz
                newrow[16] = rows[16];   //pack_jz
                newrow[17] = rows[17];   //site_no1
                newrow[18] = rows[18];   //site_desc1
                newrow[19] = rows[19];   //doc_remark
                newrow[20] = rows[20];   //doc_remarkentry
                newrow[21] = rows[21];   //site_no2
                newrow[22] = rows[22];   //site_desc2
                newrow[23] = rows[23];   //PICI
                newrow[24] = rows[24];   //FRemarkid
                newrow[25] = rows[25];   //FCreate_time
                inserttemp.Rows.Add(newrow);
            }
            return inserttemp;
        }

        /// <summary>
        /// 将K3记录插入至临时表(更新使用)
        /// </summary>
        /// <param name="k3ViewDt"></param>
        /// <param name="uptemp"></param>
        /// <returns></returns>
        private DataTable InsertDtIntoUpdateTempdt(DataTable k3ViewDt, DataTable uptemp)
        {
            //执行插入,将k3ViewDt的值插入至临时表内(用于更新至条码表)
            foreach (DataRow rows in k3ViewDt.Rows)
            {
                var newrow = uptemp.NewRow();
                newrow[0] = Convert.ToString(rows[0]);     //doc_no
                newrow[1] = Convert.ToString(rows[8]);     //sku_no
                newrow[2] = Convert.ToDecimal(rows[12]);   //qty_req
                newrow[3] = Convert.ToInt32(rows[3]);      //line_no
                newrow[4] = Convert.ToInt32(rows[24]);     //FRemarkid
                newrow[5] = Convert.ToDateTime(rows[26]);  //Flastop_time
                uptemp.Rows.Add(newrow);
            }
            return uptemp;
        }

        /// <summary>
        /// 将K3记录插入至临时表(取消不显示使用)
        /// </summary>
        /// <param name="k3ViewDt"></param>
        /// <param name="canneltemp"></param>
        /// <returns></returns>
        private DataTable InsertDtIntoCannelTempDt(DataTable k3ViewDt, DataTable canneltemp)
        {
            foreach (DataRow rows in k3ViewDt.Rows)
            {
                var newrow = canneltemp.NewRow();
                newrow[0] = Convert.ToString(rows[0]);     //doc_no
                newrow[1] = Convert.ToString(rows[8]);     //sku_no
                newrow[2] = 1;                             //FRemarkid
                newrow[3] = DateTime.Now.ToLocalTime();    //Flastop_time
                canneltemp.Rows.Add(newrow);
            }
            return canneltemp;
        }

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="conid">0:连接K3数据库,1:连接条码库</param>
        /// <param name="sqlscript">sql语句</param>
        /// <returns></returns>
        private DataTable UseSqlSearchIntoDt(int conid, string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlcon = GetCloudConn(conid);
                var sqlDataAdapter = new SqlDataAdapter(sqlscript,sqlcon);
                sqlDataAdapter.Fill(resultdt);
            }
            catch (Exception)
            {
                resultdt.Rows.Clear();
                resultdt.Columns.Clear();
            }
            return resultdt;
        }

        /// <summary>
        /// 针对指定表进行数据插入至条码表内
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        public void ImportDtToDb(string tableName, DataTable dt)
        {
            var sqlcon = GetConnectionString(1);
            // sqlcon.Open(); 若返回一个SqlConnection的话,必须要显式打开 
            //注:1)要插入的DataTable内的字段数据类型必须要与数据库内的一致;并且要按数据表内的字段顺序 2)SqlBulkCopy类只提供将数据写入到数据库内
            using (var sqlBulkCopy = new SqlBulkCopy(sqlcon))
            {
                sqlBulkCopy.BatchSize = 1000;                    //表示以1000行 为一个批次进行插入
                sqlBulkCopy.DestinationTableName = tableName;  //数据库中对应的表名
                sqlBulkCopy.NotifyAfter = dt.Rows.Count;      //赋值DataTable的行数
                sqlBulkCopy.WriteToServer(dt);               //数据导入数据库
                sqlBulkCopy.Close();                        //关闭连接 
            }
        }

        /// <summary>
        /// 根据指定条件对数据表进行批量更新
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="dt"></param>
        /// <param name="typeid">0:更新记录 1:更新FRemarkid=1</param>
        public void UpdateDbFromDt(string tablename, DataTable dt, int typeid)
        {
            var sqladpter = new SqlDataAdapter();
            var ds = new DataSet();

            //根据typeid获取对应的模板表记录
            var searList = sqllist.SearchUpdateTable(typeid);

            using (sqladpter.SelectCommand = new SqlCommand(searList, GetCloudConn(1)))
            {
                //将查询的记录填充至ds(查询表记录;后面的更新作赋值使用)
                sqladpter.Fill(ds);
                //建立更新模板相关信息(包括更新语句 以及 变量参数)
                sqladpter = GetUpdateAdapter(typeid, GetCloudConn(1), sqladpter);
                //开始更新(注:通过对DataSet中存在的表进行循环赋值;并进行更新)
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        ds.Tables[0].Rows[0].BeginEdit();
                        ds.Tables[0].Rows[0][j] = dt.Rows[i][j];
                        ds.Tables[0].Rows[0].EndEdit();
                    }
                    sqladpter.Update(ds.Tables[0]);
                }
                //完成更新后将相关内容清空
                ds.Tables[0].Clear();
                sqladpter.Dispose();
                ds.Dispose();
            }
        }

        /// <summary>
        /// 建立更新模板相关信息
        /// </summary>
        /// <param name="typeid">0:更新记录 1:更新FRemarkid=1</param>
        /// <param name="conn"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        private SqlDataAdapter GetUpdateAdapter(int typeid, SqlConnection conn, SqlDataAdapter da)
        {
            //根据tablename获取对应的更新语句
            var sqlscript = sqllist.UpdateEntry(typeid);
            da.UpdateCommand = new SqlCommand(sqlscript, conn);

            //定义所需的变量参数
            switch (typeid)
            {
                case 0:
                    da.UpdateCommand.Parameters.Add("@doc_no", SqlDbType.NVarChar, 100, "doc_no");
                    da.UpdateCommand.Parameters.Add("@sku_no", SqlDbType.NVarChar, 100, "sku_no");
                    da.UpdateCommand.Parameters.Add("@qty_req", SqlDbType.Decimal, 4, "qty_req");
                    da.UpdateCommand.Parameters.Add("@line_no", SqlDbType.Int, 8, "line_no");
                    da.UpdateCommand.Parameters.Add("@FRemarkid", SqlDbType.Int, 8, "FRemarkid");
                    da.UpdateCommand.Parameters.Add("@Flastop_time", SqlDbType.DateTime, 10, "Flastop_time");
                    break;
                case 1:
                    da.UpdateCommand.Parameters.Add("@doc_no", SqlDbType.NVarChar, 100, "doc_no");
                    da.UpdateCommand.Parameters.Add("@sku_no", SqlDbType.NVarChar, 100, "sku_no");
                    da.UpdateCommand.Parameters.Add("@FRemarkid", SqlDbType.Int, 8, "FRemarkid");
                    da.UpdateCommand.Parameters.Add("@Flastop_time", SqlDbType.DateTime, 10, "Flastop_time");
                    break;
            }
            return da;
        }


        /// <summary>
        /// 按照指定的SQL语句执行记录(反审核时使用)
        ///  <param name="conid">0:连接K3数据库,1:连接条码库</param>
        /// </summary>
        private void Generdt(string sqlscript,int conid)
        {
            using (var sql = GetCloudConn(conid))
            {
                sql.Open();
                var sqlCommand = new SqlCommand(sqlscript, sql);
                sqlCommand.ExecuteNonQuery();
                sql.Close();
            }
        }

        /// <summary>
        /// 获取连接返回信息
        /// <param name="conid">0:连接K3数据库,1:连接条码库</param>
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetCloudConn(int conid)
        {
            var sqlcon = new SqlConnection(GetConnectionString(conid));
            return sqlcon;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="conid">0:连接K3数据库,1:连接条码库</param>
        /// <returns></returns>
        private string GetConnectionString(int conid)
        {
            var strcon = string.Empty;

            if (conid == 0)
            {
                strcon = @"Data Source='192.168.1.228';Initial Catalog='AIS20211022091225';Persist Security Info=True;User ID='sa'; Password='kingdee';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            }
            else
            {
                strcon = @"Data Source='172.16.4.249';Initial Catalog='RTIM_YATU';Persist Security Info=True;User ID='sa'; Password='Yatu8888';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            }

            return strcon;
        }
    }
}

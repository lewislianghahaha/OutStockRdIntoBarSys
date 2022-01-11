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
                        //执行插入,将k3ViewDt的值插入至临时表内(用于插入至条码表)
                        foreach (DataRow rows in k3ViewDt.Rows)
                        {
                            var newrow = inserttemp.NewRow();
                            newrow[1] = rows[0];     //doc_no
                            newrow[2] = rows[1];     //doc_catalog
                            newrow[3] = rows[2];     //op_time
                            newrow[4] = rows[3];     //line_no
                            newrow[5] = rows[4];     //doc_status
                            newrow[6] = rows[5];     //customer_no
                            newrow[7] = rows[6];     //FNAME
                            newrow[8] = rows[7];     //customer_desc
                            newrow[9] = rows[8];     //sku_no
                            newrow[10] = rows[9];    //sku_desc
                            newrow[11] = rows[10];   //sku_catalog
                            newrow[12] = rows[11];   //unit
                            newrow[13] = rows[12];   //qty_req
                            newrow[14] = rows[13];   //pack_spec
                            newrow[15] = rows[14];   //pack_gz
                            newrow[16] = rows[15];   //pack_xz
                            newrow[17] = rows[16];   //pack_jz
                            newrow[18] = rows[17];   //site_no1
                            newrow[19] = rows[18];   //site_desc1
                            newrow[20] = rows[19];   //doc_remark
                            newrow[21] = rows[20];   //doc_remarkentry
                            newrow[22] = rows[21];   //site_no2
                            newrow[23] = rows[22];   //site_desc2
                            newrow[24] = rows[23];   //PICI
                            newrow[25] = rows[24];   //FRemarkid
                            newrow[26] = rows[25];   //FCreate_time
                            inserttemp.Rows.Add(newrow);
                        }
                    }
                    else
                    {
                        //执行插入,将k3ViewDt的值插入至临时表内(用于更新至条码表)
                        foreach (DataRow rows in k3ViewDt.Rows)
                        {
                            var newrow = uptemp.NewRow();
                            newrow[0] = rows[0];      //doc_no
                            newrow[1] = rows[8];      //sku_no
                            newrow[2] = rows[12];     //qty_req
                            newrow[3] = rows[24];     //FRemarkid
                            newrow[4] = rows[26];     //Flastop_time
                            uptemp.Rows.Add(newrow);
                        }
                    }
                    //最后将得出的结果进行插入或更新
                    if (inserttemp.Rows.Count > 0)
                        ImportDtToDb("T_K3SalesOut", inserttemp);
                    if (uptemp.Rows.Count > 0)
                        UpdateDbFromDt("T_K3SalesOut", uptemp);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
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
        public void UpdateDbFromDt(string tablename, DataTable dt)
        {
            var sqladpter = new SqlDataAdapter();
            var ds = new DataSet();

            //根据表格名称获取对应的模板表记录
            var searList = sqllist.SearchUpdateTable(tablename);

            using (sqladpter.SelectCommand = new SqlCommand(searList, GetCloudConn(1)))
            {
                //将查询的记录填充至ds(查询表记录;后面的更新作赋值使用)
                sqladpter.Fill(ds);
                //建立更新模板相关信息(包括更新语句 以及 变量参数)
                sqladpter = GetUpdateAdapter(tablename, GetCloudConn(1), sqladpter);
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
        /// <param name="tablename"></param>
        /// <param name="conn"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        private SqlDataAdapter GetUpdateAdapter(string tablename, SqlConnection conn, SqlDataAdapter da)
        {
            //根据tablename获取对应的更新语句
            var sqlscript = sqllist.UpdateEntry(tablename);
            da.UpdateCommand = new SqlCommand(sqlscript, conn);

            //定义所需的变量参数
            switch (tablename)
            {
                case "T_K3SalesOut":
                    da.UpdateCommand.Parameters.Add("@doc_no", SqlDbType.NVarChar, 100, "doc_no");
                    da.UpdateCommand.Parameters.Add("@sku_no", SqlDbType.NVarChar, 100, "sku_no");
                    da.UpdateCommand.Parameters.Add("@qty_req", SqlDbType.Decimal, 4, "qty_req");
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

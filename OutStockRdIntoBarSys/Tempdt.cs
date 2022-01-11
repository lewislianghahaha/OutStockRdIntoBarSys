using System;
using System.Data;

namespace OutStockRdIntoBarSys
{
    //所需插入数据
    public class Tempdt
    {
        /// <summary>
        /// 插入数据至T_K3SalesOut表使用
        /// </summary>
        /// <returns></returns>
        public DataTable InsertBarTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 28; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //记录每行的行ID
                    case 0:
                        dc.ColumnName = "Id";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 1:
                        dc.ColumnName = "doc_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "doc_catalog";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 3:
                        dc.ColumnName = "op_time";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    case 4:
                        dc.ColumnName = "line_no";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 5:
                        dc.ColumnName = "doc_status";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 6:
                        dc.ColumnName = "customer_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 7:
                        dc.ColumnName = "FNAME";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 8:
                        dc.ColumnName = "customer_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 9:
                        dc.ColumnName = "sku_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 10:
                        dc.ColumnName = "sku_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 11:
                        dc.ColumnName = "sku_catalog";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 12:
                        dc.ColumnName = "unit";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 13:
                        dc.ColumnName = "qty_req";
                        dc.DataType = Type.GetType("System.Decimal"); 
                        break;
                    case 14:
                        dc.ColumnName = "pack_spec";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 15:
                        dc.ColumnName = "pack_gz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 16:
                        dc.ColumnName = "pack_xz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 17:
                        dc.ColumnName = "pack_jz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 18:
                        dc.ColumnName = "site_no1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 19:
                        dc.ColumnName = "site_desc1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 20:
                        dc.ColumnName = "doc_remark";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 21:
                        dc.ColumnName = "doc_remarkentry";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 22:
                        dc.ColumnName = "site_no2";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 23:
                        dc.ColumnName = "site_desc2";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 24:
                        dc.ColumnName = "PICI";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 25:
                        dc.ColumnName = "FRemarkid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //创建日期(插入时使用)
                    case 26:
                        dc.ColumnName = "FCreate_time";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    //最后修改日期(更新时使用)
                    case 27:
                        dc.ColumnName = "Flastop_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 更新T_K3SalesOut表记录
        /// </summary>
        /// <returns></returns>
        public DataTable UpBarTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 5; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    //记录每行的行ID
                    case 0:
                        dc.ColumnName = "doc_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 1:
                        dc.ColumnName = "sku_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "qty_req";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 3:
                        dc.ColumnName = "FRemarkid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //最后修改日期(更新时使用)
                    case 4:
                        dc.ColumnName = "Flastop_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }
    }
}

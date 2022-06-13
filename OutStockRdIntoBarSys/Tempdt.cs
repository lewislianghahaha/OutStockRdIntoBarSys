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
            for (var i = 0; i < 27; i++)
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
                        dc.ColumnName = "doc_catalog";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "op_time";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    case 3:
                        dc.ColumnName = "line_no";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 4:
                        dc.ColumnName = "doc_status";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 5:
                        dc.ColumnName = "customer_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 6:
                        dc.ColumnName = "FNAME";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 7:
                        dc.ColumnName = "customer_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 8:
                        dc.ColumnName = "sku_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 9:
                        dc.ColumnName = "sku_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 10:
                        dc.ColumnName = "sku_catalog";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 11:
                        dc.ColumnName = "unit";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 12:
                        dc.ColumnName = "qty_req";
                        dc.DataType = Type.GetType("System.Decimal"); 
                        break;
                    case 13:
                        dc.ColumnName = "pack_spec";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 14:
                        dc.ColumnName = "pack_gz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 15:
                        dc.ColumnName = "pack_xz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 16:
                        dc.ColumnName = "pack_jz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 17:
                        dc.ColumnName = "site_no1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 18:
                        dc.ColumnName = "site_desc1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 19:
                        dc.ColumnName = "doc_remark";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 20:
                        dc.ColumnName = "doc_remarkentry";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 21:
                        dc.ColumnName = "site_no2";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 22:
                        dc.ColumnName = "site_desc2";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 23:
                        dc.ColumnName = "PICI";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 24:
                        dc.ColumnName = "FRemarkid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //创建日期(插入时使用)
                    case 25:
                        dc.ColumnName = "FCreate_time";
                        dc.DataType = Type.GetType("System.DateTime"); 
                        break;
                    //最后修改日期(更新时使用)
                    case 26:
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
            for (var i = 0; i < 9; i++)
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
                        dc.ColumnName = "line_no";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 4:
                        dc.ColumnName = "FRemarkid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //最后修改日期(更新时使用)
                    case 5:
                        dc.ColumnName = "Flastop_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                    //op_time(操作时间,更新时使用)
                    case 6:
                        dc.ColumnName = "op_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                    //customer_desc
                    case 7:
                        dc.ColumnName = "customer_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    //site_desc2
                    case 8:
                        dc.ColumnName = "site_desc2";
                        dc.DataType = Type.GetType("System.String"); 
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 更新T_K3SalesOut需要取消的记录
        /// </summary>
        /// <returns></returns>
        public DataTable CannelTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 4; i++)
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
                        dc.ColumnName = "FRemarkid";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    //最后修改日期(更新时使用)
                    case 3:
                        dc.ColumnName = "Flastop_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 物料临时表(插入及更新使用)
        /// </summary>
        /// <returns></returns>
        public DataTable MaterialTemp()
        {
            var dt = new DataTable();
            for (var i = 0; i < 24; i++)
            {
                var dc = new DataColumn();

                switch (i)
                {
                    case 0:
                        dc.ColumnName = "sku_no";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 1:
                        dc.ColumnName = "sku_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 2:
                        dc.ColumnName = "sku_desc_en";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 3:
                        dc.ColumnName = "baseunit_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 4:
                        dc.ColumnName = "stockunit_desc";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 5:
                        dc.ColumnName = "pack_spec";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 6:
                        dc.ColumnName = "pack_gz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 7:
                        dc.ColumnName = "pack_xz";
                        dc.DataType = Type.GetType("System.Decimal");
                        break;
                    case 8:
                        dc.ColumnName = "label_number";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 9:
                        dc.ColumnName = "label_name";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 10:
                        dc.ColumnName = "sku_catalog";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 11:
                        dc.ColumnName = "pack_jz";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 12:
                        dc.ColumnName = "化学品分类";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 13:
                        dc.ColumnName = "配比";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 14:
                        dc.ColumnName = "保质期";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 15:
                        dc.ColumnName = "主要成份";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 16:
                        dc.ColumnName = "储存温度";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 17:
                        dc.ColumnName = "毛重";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 18:
                        dc.ColumnName = "项目名称";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 19:
                        dc.ColumnName = "客户端物料编号";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 20:
                        dc.ColumnName = "配比标题";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 21:
                        dc.ColumnName = "FCreate_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                    case 22:
                        dc.ColumnName = "Flastop_time";
                        dc.DataType = Type.GetType("System.DateTime");
                        break;
                    case 23:
                        dc.ColumnName = "标签打印名称";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }
    }
}

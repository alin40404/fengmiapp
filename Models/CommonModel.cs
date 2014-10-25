using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class CommonModel
    {
        protected string _table="";

        /// <summary>
        /// 表名
        /// </summary>
        public string Table
        {
            get { return _table; }
            set { 
                _table=value;
                _table = _table.Trim(new char[]{ '[',']'});
                _table = "[" + _table + "]";
            }
        }

        public CommonModel()
        {
        }
        
        public int Add(string value, SqlParameter[] para)
        {
            value = value.Trim();
            value = value.Trim(',');
            string tempValue = "," + value;
            string AtValue = tempValue.Replace(",", ",@");
            AtValue = AtValue.Trim(',');
            string sql =
            " INSERT INTO " + this._table + " ("+value+") " +
            " VALUES (" + AtValue + ") ";



            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        public int AddBackId(string value, SqlParameter[] para)
        {
            value = value.Trim(',');
            string tempValue = "," + value;
            string AtValue = tempValue.Replace(",", ",@");
            AtValue = AtValue.Trim(',');
            string sql = "";
            sql += " INSERT INTO " + this._table + " (" + value + ") ";
            sql += " output inserted.Id ";
            sql += " VALUES (" + AtValue + "); ";
            sql += " select @@identity ";

            return SQLHelper.ExecuteScalar(CommandType.Text, sql, para);
        }

        public int Modify(string set,  SqlParameter[] para)
        {
            string sql = " UPDATE " + this._table + " set " +set +
                    "   Where Id=@Id  ";


            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        public int Delete(int Id)
        {
            string sql = " delete " + this._table + " Where Id=@Id ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        public int Delete(string Id)
        {
            string sql = " delete " + this._table + "  Where Id in (@Id)  ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return SQLHelper.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        public DataTable GetDataList()
        {
            string strSql = " Select * from " + this._table + " where 1=1  ";
            SqlParameter[] para = new SqlParameter[]
			{
			};
            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable GetDataList(string strSql)
        {
            if (string.IsNullOrEmpty(strSql))
            {
                strSql = " Select * from " + this._table + " where 1=1  ";
            }
            
            SqlParameter[] para = new SqlParameter[]
			{
			};
            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable GetDataList(string strSql, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable GetDataList(string where,string order, SqlParameter[] para)
        {
            string strSql = " Select * from " + this._table + " where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                strSql += " and " + where;
            }

            if (!string.IsNullOrEmpty(order))
            {
                strSql += " order by " + order;
            }

            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable GetDataById(int? Id)
        {
            string strSql = " Select * from " + this._table + " where 1=1 and Id=@Id ";
            SqlParameter[] para = new SqlParameter[]
			{
                 new SqlParameter("@Id", Id), 
			};
            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        public DataTable GetDataById(string Id)
        {
            string strSql = " Select * from " + this._table + " where 1=1 and Id=@Id ";
            SqlParameter[] para = new SqlParameter[]
			{
                 new SqlParameter("@Id", Id), 
			};
            DataTable dt = new DataTable();
            DataSet ds = SQLHelper.ExecuteToDataSet(CommandType.Text, strSql, para);
            if (ds == null || ds.Tables[0] == null)
            {
                return null;
            }
            else
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

    }
}
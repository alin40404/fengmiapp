using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class CommonModel:_SQLHelper
    {
        #region 属性
        /// <summary>
        /// 表名
        /// </summary>
        protected string _table="";

        /// <summary>
        /// 表名
        /// </summary>
        public string Table
        {
            get { return this._table; }
            set {
                this._table = value;
                this._table = _table.Trim(new char[] { '[', ']' });
                this._table = "[" + _table + "]";
            }
        }
        #endregion

        public CommonModel()
        {
        }

        #region 添加
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

            return this.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        /// <summary>
        /// 添加到数据库，同时返回插入的Id号
        /// </summary>
        /// <param name="value"></param>
        /// <param name="para"></param>
        /// <returns></returns>
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

            return this.ExecuteScalar(CommandType.Text, sql, para);
        }
        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="set"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public int Modify(string set,  SqlParameter[] para)
        {
            string sql = " UPDATE " + this._table + " set " +set +
                    "   Where Id=@Id  ";


            return this.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="set"></param>
        /// <param name="where"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public int Modify(string set,string where, SqlParameter[] para)
        {

            if (!string.IsNullOrEmpty(where))
            {
                where = " Where 1=1 and " + where;
            }
            string sql = " UPDATE " + this._table + " set " + set +
                    "  " + where;


            return this.ExecuteNonQuery(CommandType.Text, sql, para);

        }

        #endregion
        
        #region 删除
        public int Delete(int Id)
        {
            string sql = " delete " + this._table + " Where Id=@Id ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return this.ExecuteNonQuery(CommandType.Text, sql, para);
        }

        public int Delete(string Id)
        {
            string sql = " delete " + this._table + "  Where Id in (@Id)  ";
            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("@Id", Id),
			};
            return this.ExecuteNonQuery(CommandType.Text, sql, para);
        }
        #endregion

        #region 查询
        public DataTable GetDataList()
        {
            string strSql = " Select * from " + this._table + " where 1=1  ";
            SqlParameter[] para = new SqlParameter[]
			{
			};
            DataTable dt = new DataTable();
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
            DataSet ds = this.ExecuteToDataSet(CommandType.Text, strSql, para);
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
        #endregion
    }
}
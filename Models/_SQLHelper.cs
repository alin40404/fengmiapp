using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace fengmiapp.Models
{
    public class _SQLHelper
    {
        #region 属性
        /// <summary>
        /// 数据库连接字符串，写在配置文件里
        /// </summary>
        protected SqlConnection _ConnectionString;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected SqlConnection ConnectionString
        {
            set { this._ConnectionString = value; }
            get { return this._ConnectionString; }
        }
        #endregion

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        protected _SQLHelper()
        {
            this.open();
        }
        /// <summary>
        /// 析构函数，关闭数据库链接
        /// </summary>
        ~_SQLHelper()
        {
            this.close();
        }

        #region 打开、关闭数据库
        /// <summary>
        ///  获取连接字符串
        /// </summary>
        /// <returns></returns>
        protected  string getConnectionString()
        {
            string constr;
            constr = ConfigurationManager.AppSettings["ConnectionString"];
            return constr;
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        protected  void open()
        {
            string constr;
            constr = getConnectionString();
            this._ConnectionString = new SqlConnection(constr);
            try
            {
                this._ConnectionString.Open();
            }
            catch (Exception ex)
            {
                
            }
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        protected  void close()
        {
            this._ConnectionString.Dispose();
            this._ConnectionString.Close();
        }
        #endregion

        #region ExecuteNonQuery
        public  int ExecuteNonQuery(SqlCommand scmd)
        {
            this.open();
            int result = scmd.ExecuteNonQuery();
            this.close();
            return result;
        }
        public  int ExecuteNonQuery(string cmdText)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
            int result = cmd.ExecuteNonQuery();
            this.close();
            return result;
        }
        public  int ExecuteNonQuery(SqlConnection connection, string cmdText)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            int result = cmd.ExecuteNonQuery();
            this.close();
            return result;
        }
        public  int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            this.open();
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(commandParameters);
                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                result = -1;
            }
            this.close();
            return result;
        }
        public  int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);
            int result = cmd.ExecuteNonQuery();
            this.close();
            return result;

        }
        #endregion

        #region ExecuteReader
        public  SqlDataReader ExecuteReader(string cmdText)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public  SqlDataReader ExecuteReader(SqlConnection connection, string cmdText)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public  SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;

        }
        public  SqlDataReader ExecuteReader(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            this.open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;

        }
        #endregion

        #region ExecuteToDataSet
        public  DataSet ExecuteToDataSet(SqlCommand scmd)
        {
            SqlDataAdapter rs = new SqlDataAdapter(scmd);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            return ds;
        }
        
        public  DataSet ExecuteToDataSet(string cmdText)
        {
            open();
            SqlDataAdapter rs = new SqlDataAdapter(cmdText, this._ConnectionString);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            close();
            return ds;
        }
        
        public  DataSet ExecuteToDataSet(SqlConnection connection, string cmdText)
        {
            SqlDataAdapter rs = new SqlDataAdapter(cmdText, connection);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            connection.Dispose();
            connection.Close();
            return ds;
        }
        
        public  DataSet ExecuteToDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(commandParameters);
                SqlDataAdapter rs = new SqlDataAdapter(cmd);

                rs.Fill(ds);
            }
            catch (Exception ex)
            {
            }
            close();
            return ds;
        }
        #endregion

        #region ExecuteScalar
        public  int ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, this._ConnectionString);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(commandParameters);
                //result = cmd.ExecuteNonQuery();
                result = (int)cmd.ExecuteScalar();
            }
            catch
            {
                result = -1;
            }
            close();
            return result;
        }
        #endregion
    }
}
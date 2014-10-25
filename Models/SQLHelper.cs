using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace fengmiapp.Models
{
    public  class SQLHelper
    {
        protected SQLHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        protected static SqlConnection ConnectionString;   //连接字符串
        protected static string getConnectionString()    //获取连接字符串
        {
            string constr;
            constr = ConfigurationManager.AppSettings["ConnectionString"];
            return constr;
        }

        protected static void open()      //打开数据库
        {
            string constr;
            constr = getConnectionString();
            ConnectionString = new SqlConnection(constr);
            try
            {
                ConnectionString.Open();
            }
            catch (Exception ex)
            {
                
            }
        }
        protected static void close()      //关闭数据库
        {
            ConnectionString.Dispose();
            ConnectionString.Close();
        }

        public static int ExecuteNonQuery(SqlCommand scmd)
        {
            int result = scmd.ExecuteNonQuery();
            return result;
        }
        public static int ExecuteNonQuery(string cmdText)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
            int result = cmd.ExecuteNonQuery();
            close();
            return result;
        }
        public static int ExecuteNonQuery(SqlConnection connection, string cmdText)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            int result = cmd.ExecuteNonQuery();
            close();
            return result;
        }
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(commandParameters);
                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                result = -1;
            }
            close();
            return result;
        }
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);
            int result = cmd.ExecuteNonQuery();
            close();
            return result;

        }

        public static SqlDataReader ExecuteReader(string cmdText)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public static SqlDataReader ExecuteReader(SqlConnection connection, string cmdText)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;

        }
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.CommandType = cmdType;
            cmd.Parameters.AddRange(commandParameters);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;

        }

        public static DataSet ExecuteToDataSet(SqlCommand scmd)
        {
            SqlDataAdapter rs = new SqlDataAdapter(scmd);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            return ds;
        }
        public static DataSet ExecuteToDataSet(string cmdText)
        {
            open();
            SqlDataAdapter rs = new SqlDataAdapter(cmdText, ConnectionString);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            close();
            return ds;
        }
        public static DataSet ExecuteToDataSet(SqlConnection connection, string cmdText)
        {
            SqlDataAdapter rs = new SqlDataAdapter(cmdText, connection);
            DataSet ds = new DataSet();
            rs.Fill(ds);
            connection.Dispose();
            connection.Close();
            return ds;
        }
        public static DataSet ExecuteToDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
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
        //public static DataSet ExecuteToDataSet(string cmdText, int startRecord, int maxRecord, string TableName);
       
        
        public static int ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            open();
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, ConnectionString);
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

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class Log:CommonModel
    {
        #region 参数

        private int _id = 0;
        private string _describe = string.Empty;
        private string _emergeURL = string.Empty;
        private int _userid = 0;
        private int _logType = 0;
        private string _logTime = string.Empty;
        private string _ip = String.Empty;

        #endregion

        #region 属性
      
        ///<summary>
        /// Id
        ///</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        ///<summary>
        /// Describe
        ///</summary>
        public string Describe
        {
            get { return _describe; }
            set { _describe = value; }
        }
        ///<summary>
        /// EmergeURL
        ///</summary>
        public string EmergeURL
        {
            get { return _emergeURL; }
            set { _emergeURL = value; }
        }

        ///<summary>
        /// Userid
        ///</summary>
        public int Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        ///<summary>
        /// LogType
        ///</summary>
        public int LogType
        {
            get { return _logType; }
            set { _logType = value; }
        }

        ///<summary>
        /// IP
        ///</summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        ///<summary>
        /// LogTime
        ///</summary>
        public string LogTime
        {
            get { return _logTime; }
            set { _logTime = value; }
        }

        #endregion

        public Log()
        {
            init();
        }
        public Log(int id)
        {
            init();

            this._id = id;
            string strSql = "SELECT * FROM " + this._table + " where id = @Id ";
            DataTable dt = null;

            SqlParameter[] para = new SqlParameter[]
			{
				new SqlParameter("Id", _id),
			};

            dt = base.GetDataList(strSql, para);

            if (dt.Rows.Count > 0)
            {
                this._id = int.Parse(dt.Rows[0]["id"].ToString());

                this._describe = dt.Rows[0]["describe"].ToString();
                this._emergeURL = dt.Rows[0]["emergeURL"].ToString();

                this._userid = int.Parse(dt.Rows[0]["userid"].ToString());

                this._logType = int.Parse(dt.Rows[0]["logType "].ToString());

                this._logTime = dt.Rows[0]["logTime"].ToString();

                this._ip = dt.Rows[0]["ip"].ToString();
            }
        }
     
        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[log]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            _logTime = DateTime.Now.ToString();
            //string sql = "INSERT INTO " + this._table + "(logType,describe,logTime,[ip],emergeURL,userid) " +
            //"VALUES (@logType,@describe,@logTime,@ip,@emergeURL,@userid)";
            string sql = "INSERT INTO " + this._table + "(logType,describe,logTime) " +
           "VALUES (@logType,@describe,@logTime)";

            string value = "logType,describe,logTime";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@describe", _describe),
                //new SqlParameter("@emergeURL", _emergeURL),
                //new SqlParameter("@userid", _userid),
                new SqlParameter("@logType", _logType),
                new SqlParameter("@logTime", _logTime),
                //new SqlParameter("@ip", _ip),
             
            };

            return base.Add(value, para);

        }
              
        /*
        public DataTable GetDataList()
        {
            string strSql = " Select log.id,log.logType,log.describe,log.ip,log.emergeURL,log.logTime,u.adminName  from " + this._table + "as log left join [admin] as u on u.Id=log.userid where 1=1  ";
            if (_id != 0)
            {
                strSql += " and log.id = @id  ";
            }
            if (_logType != 0)
            {
                strSql += " and log.logType = @logType  ";
            }

            if (_describe != String.Empty)
            {
                strSql += " and log.describe like '%'+ @describe+'%'  ";
            }
            if (_ip != String.Empty)
            {
                strSql += " and log.ip like '%'+ @ip+'%'  ";
            }

            if (_emergeURL != String.Empty)
            {
                strSql += " and log.emergeURL like '%'+  @emergeURL+'%' ";
            }
            string logNextTime = String.Empty;

            if (_logTime != String.Empty)
            {
                try
                {
                    logNextTime = DateTime.Parse(_logTime).AddDays(1).ToString();
                }
                catch { }
                strSql += " and ( log.logTime >= @logTime and  log.logTime < @logNextTime )";
            }

            if (_userid  != 0)
            {
                strSql += " and log.[userid]= @userid ";
            }
            strSql += " order by log.id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
					new SqlParameter("@id", _id),
                    new SqlParameter("@describe", _describe),
                    new SqlParameter("@emergeURL", _emergeURL),
                    new SqlParameter("@userid", _userid),
                    new SqlParameter("@logType", _logType),
                    new SqlParameter("@logTime", _logTime ),
                    new SqlParameter("@logNextTime", logNextTime ),
                    new SqlParameter("@ip", _ip),
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
        */
    }
}
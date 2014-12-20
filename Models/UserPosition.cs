using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace fengmiapp.Models
{
    public class UserPosition:CommonModel
    {
        #region 参数

        private int _id = 0;

        private int _uId = 0;
        private double _longitude = 0;
        private double _latitude = 0;
        private string _placeName = string.Empty;
        
        private string _offlineUserIds = string.Empty;
        private int _isHiding = 0;

        private DateTime _modifyTime = DateTime.Now;
        private DateTime _uploadTime = DateTime.Now;


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
        /// UId
        ///</summary>
        public int UId
        {
            get { return _uId; }
            set { _uId = value; }
        }
        ///<summary>
        /// Longitude
        ///</summary>
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        ///<summary>
        /// Latitude
        ///</summary>
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        ///<summary>
        /// PlaceName
        ///</summary>
        public string PlaceName
        {
            get { return _placeName; }
            set { _placeName = value; }
        }
       
        ///<summary>
        /// OfflineUserIds
        ///</summary>
        public string OfflineUserIds
        {
            get { return _offlineUserIds; }
            set { _offlineUserIds = value; }
        }

        ///<summary>
        /// IsHiding
        ///</summary>
        public int IsHiding
        {
            get { return _isHiding; }
            set { _isHiding = value; }
        }


        /// <summary>
        /// ModifyTime
        /// </summary>
        public DateTime ModifyTime
        {
            get { return this._modifyTime; }
            set { this._modifyTime = value; }
        }

        /// <summary>
        /// UploadTime
        /// </summary>
        public DateTime UploadTime
        {
            get { return this._uploadTime; }
            set { this._uploadTime = value; }
        }

        #endregion

        public UserPosition(): base()
        {
            init();
        }

        public UserPosition(int id)
        {
            init();
            if (id > 0)
            {
                DataTable dt = new DataTable();
                dt = base.GetDataById(id);

                if (dt.Rows.Count > 0)
                {
                    this.SetField(dt);
                }
            }
        }

        protected void SetField(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._id = int.Parse(dt.Rows[0]["Id"].ToString());

                    this._uId = int.Parse(dt.Rows[0]["uId"].ToString());
                    this._placeName = dt.Rows[0]["placeName"].ToString(); ;
                    this._longitude = double.Parse(dt.Rows[0]["longitude"].ToString());
                    this._latitude = double.Parse(dt.Rows[0]["latitude"].ToString());

                    string modifyTime = dt.Rows[0]["modifyTime"].ToString();
                    this._modifyTime = DateTime.Parse(modifyTime);

                    string uploadTime = dt.Rows[0]["uploadTime"].ToString();
                    this._uploadTime = DateTime.Parse(uploadTime);
                   
                    this._isHiding = int.Parse(dt.Rows[0]["isHiding"].ToString());
                    this._offlineUserIds = dt.Rows[0]["offlineUserIds"].ToString(); ;

                }
            }
            catch { }

        }

        public void GetNewestPositionOne()
        {
            DataTable dt = new DataTable();
            dt = this.GetPositionList(1);
            this.SetField(dt);
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init()
        {
            string table = "[userPosition]";
            this._table = table;
            this.Table = table;
        }

        public int Add()
        {
            string value = "uId,longitude,latitude,placeName,modifyTime,uploadTime,isHiding,offlineUserIds";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@uId", _uId),
                new SqlParameter("@longitude", _longitude),
                new SqlParameter("@latitude", _latitude),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),
                new SqlParameter("@isHiding", _isHiding),
                new SqlParameter("@offlineUserIds", _offlineUserIds),

			};
            return base.Add(value, para);
        }

        public int Modify_Time()
        {
            string set = "modifyTime=@modifyTime,uploadTime=@uploadTime";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),
			};
            return base.Modify(set, para);
        }

        public int Modify_Time_IsHiding()
        {
            string set = "modifyTime=@modifyTime,uploadTime=@uploadTime,isHiding=@isHiding,offlineUserIds=@offlineUserIds";
            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),
                new SqlParameter("@isHiding", _isHiding),
                new SqlParameter("@offlineUserIds", _offlineUserIds),
			};
            return base.Modify(set, para);
        }

        public DataTable GetPositionList(int number)
        {
            if (number < 1)
            {
                number = 10;
            }

            string strSql = " Select top " + number + " * from " + this._table + " as t where 1=1 ";


            if (_id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_placeName != String.Empty)
            {
                strSql += " and t.placeName like '%'+ @placeName+'%'  ";
            }

            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }
        
        public DataTable GetPositionList(string fuId,int number)
        {
            if (number < 1)
            {
                number = 10;
            }
            string where = string.Empty;

            if (!string.IsNullOrEmpty(fuId))
            {

                where = " and offlineUserIds not like '%" + fuId + ",%' ";
            }
            string strSql = " Select top " + number + " * from " + this._table + " as t where 1=1 " + where;


            if (_id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_placeName != String.Empty)
            {
                strSql += " and t.placeName like '%'+ @placeName+'%'  ";
            }

            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }
        
        public DataTable GetPositionList(int page, int perPage)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (perPage < 1)
            {
                page = 10;
            }

            int top1 = page * perPage;
            int top2 = (page - 1) * perPage;

            string strSql = " Select top " + top1 + " * from " + this._table + " as t where 1=1 ";


            if (_id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_placeName != String.Empty)
            {
                strSql += " and t.placeName like '%'+ @placeName+'%'  ";
            }

            strSql += " and t.Id not in ( Select top " + top2 + " t2.Id  from " + this._table + " as t2 where 1=1 order by t2.Id desc ) ";
            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }

        public DataTable GetPositionList(string fuId, int page, int perPage)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (perPage < 1)
            {
                page = 10;
            }

            int top1 = page * perPage;
            int top2 = (page - 1) * perPage;

            string strSql = " Select top " + top1 + " * from " + this._table + " as t where 1=1 ";
            string where = string.Empty;

            if (!string.IsNullOrEmpty(fuId))
            {

                where = " and offlineUserIds not like '%" + fuId + ",%' ";
            }


            if (_id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_placeName != String.Empty)
            {
                strSql += " and t.placeName like '%'+ @placeName+'%'  ";
            }

            strSql += " and t.Id not in ( Select top " + top2 + " t2.Id  from " + this._table + " as t2 where 1=1 order by t2.Id desc ) ";
            strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};

            return base.GetDataList(strSql, para);

        }


        public int GetPositionListCount()
        {
            string strSql = " Select count(*) as amount from " + this._table + " as t where 1=1 ";


            if (_id != 0)
            {
                strSql += " and t.Id != @Id  ";
            }
            if (_uId != 0)
            {
                strSql += " and t.uId = @uId  ";
            }

            if (_placeName != String.Empty)
            {
                strSql += " and t.placeName like '%'+ @placeName+'%'  ";
            }

            //strSql += " order by t.Id desc ";

            SqlParameter[] para = new SqlParameter[]
			{
                new SqlParameter("@Id", _id),
                new SqlParameter("@uId", _uId),
                new SqlParameter("@placeName", _placeName),
                new SqlParameter("@modifyTime", _modifyTime),
                new SqlParameter("@uploadTime", _uploadTime),

			};
            int total = 0;

            DataTable dt = new DataTable();

            int count = 0;
            try
            {
                dt = base.GetDataList(strSql, para);
                count = dt.Rows.Count;
            }
            catch { }

            if (count > 0)
            {
                total = int.Parse(dt.Rows[0]["amount"].ToString());
            }
            return total;
        }

        //public DataTable GetNewestFriendPosition()
        //{

        //    string table2 = "[userFriend]";

        //    string strSql = " Select  t.* from " + this._table + " as t where 1=1 ";
        //    strSql += " and t.uId in ( ";
        //    strSql += " Select t2.fuId from " + table2 + " as t2 where t2.uId=@uId  ";
        //    strSql += " order by t2.Id desc ) group by t.uId ";

        //    //@uId  


        //    SqlParameter[] para = new SqlParameter[]
        //    {
        //        new SqlParameter("@Id", _Id),
        //        new SqlParameter("@uId", _uId),
        //        new SqlParameter("@placeName", _placeName),
        //        new SqlParameter("@modifyTime", _modifyTime),
        //        new SqlParameter("@uploadTime", _uploadTime),

        //    };

        //    return base.GetDataList(strSql, para);
        //}

    }
}
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fashion.Code.DAL
{
    public class SpecialConsult_dal
    {

        /// <summary>
        /// Creator:Simple
        /// 通过用户特定咨询后保存的个人照路径查询该条特定咨询数据的id，因为个人照路径geRenZhaoUrl是唯一的
        /// </summary>
        /// <param name="geRenZhaoUrl"></param>
        /// <returns></returns>
        public object GetSpecialConsultId(string geRenZhaoUrl)
        {
            string sqlStr = "select SpecialConsult_Id from tb_SpecialConsult where SpecialConsult_UserPhotoUrl=@geRenZhaoUrl";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@geRenZhaoUrl",geRenZhaoUrl)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        /// <summary>
        /// 通过专家解答的特定咨询staticConsultAnswerHtml查询该条数据的id
        /// </summary>
        /// <param name="staticConsultAnswerHtml"></param>
        /// <returns></returns>
        public object GetConsultAnswerId(string staticConsultAnswerHtml)
        {
            string sqlStr = "select SpecialConsultAnswer_Id from tb_SpecialConsultAnswer where SpecialConsultAnswer_HtmlUrl=@staticConsultAnswerHtml";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@staticConsultAnswerHtml",staticConsultAnswerHtml)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
            
        }

        /// <summary>
        /// 通过特定咨询的帖子编号specialConsultId查询数据库的tb_SpecialConsult，
        /// 获取用户特定咨询时填写的特定咨询数据
        /// </summary>
        /// <param name="specialConsultId">特定咨询的帖子编号</param>
        /// <returns></returns>
        public SpecialConsult_model GetOneConsultData(int specialConsultId)
        {
            string sqlStr = @"select consult.SpecialConsult_Id id,consult.SpecialConsult_Caption caption,consult.SpecialConsult_Occasion occasion,
                                             	   consult.SpecialConsult_UserPhotoUrl geRenZhao,consult.SpecialConsult_LikeStyleUrl likeStyleUrl,
                                             	   consult.SpecialConsult_DislikeStyleUrl dislikeStyleUrl,consult.SpecialConsult_Detail detail,
                                             	   theUser.User_Id userId,theUser.User_Name userName,theUser.User_Height height,theUser.User_Weight weight,
                                             	   theUser.User_YaoWei yaoWei,theUser.User_LegLength tuiChang,theUser.User_ThighGirth daTuiWei,
                                             	   theUser.User_SkinColor skinColor,theUser.User_TunWei tunWei,theUser.User_ArmGirth biWei,
                                             	   theUser.User_XiongWei xiongWei,theUser.User_CalfGirth xiaoTuiWei
                                            from  tb_SpecialConsult consult 
                                                           left join tb_User theUser on consult.SpecialConsult_UserId=theUser.User_Id
                                            where consult.SpecialConsult_Id=@specialConsultId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@specialConsultId",specialConsultId)
            };
            DataTable dataTable=SqlHelper.ExecuteDataTable(sqlStr, parameters);
            SpecialConsult_model specialConsult_model = ToModel(dataTable.Rows[0]);
            return specialConsult_model;
        }

        /// <summary>
        /// 根据specialConsultId获取特定咨询的SpecialConsult_Caption，SpecialConsult_Occasion，SpecialConsult_Detail，SpecialConsult_Date
        /// </summary>
        /// <param name="specialConsultId"></param>
        /// <returns></returns>
        public SpecialConsult_model GetOneShortConsultData(int specialConsultId)
        {
            string sqlStr = @"select SpecialConsult_Caption caption,SpecialConsult_Occasion occasion,
                                                   SpecialConsult_Detail detail,SpecialConsult_Date [date]
                                              from tb_SpecialConsult
                                              where SpecialConsult_Id=@specialConsultId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@specialConsultId",specialConsultId)
            };
            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            SpecialConsult_model specialConsult_model = new SpecialConsult_model();
            if (dt.Rows.Count == 1)
            {
                specialConsult_model.caption = dt.Rows[0]["caption"].ToString();
                specialConsult_model.datetime = (DateTime)dt.Rows[0]["date"];
                specialConsult_model.occasion = dt.Rows[0]["occasion"].ToString();
                specialConsult_model.detail = dt.Rows[0]["detail"].ToString();
            }
            return specialConsult_model;
        }


        /// <summary>
        /// 通过用户名，查询该用户特定咨询过的帖子，
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 特定咨询帖子id  标题 用户个人照 详细描述 日期
        /// </summary>
        /// <param name="userName">用户名id</param>
        /// <returns></returns>
        public List<SpecialConsult_model>  GetShortConsultData(int userId)
        {
            string sqlStr = @"select SpecialConsult_Id as id,SpecialConsult_Caption as caption,
                                                   SpecialConsult_UserPhotoUrl as geRenZhao,
                                                   SpecialConsult_Detail as detail,SpecialConsult_Date  as date
	                                     from tb_SpecialConsult 
                                         where SpecialConsult_UserId=@userId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userId",userId),
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr,parameters);
            List<SpecialConsult_model> specialConsult_modelList = new List<SpecialConsult_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                specialConsult_modelList.Add(ToShortModel(row));
            }
            return specialConsult_modelList;
        }

        /// <summary>
        /// 通过专家名，查询特定咨询该专家的帖子，
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 特定咨询帖子id  标题 用户个人照 详细描述 日期
        /// </summary>
        /// <param name="expertId">专家id</param>
        /// <returns></returns>
        public List<SpecialConsult_model> GetShortConsultExpertData(int expertId)
        {
            string sqlStr = @" select specialConsult.SpecialConsult_Id as id,
                                                    specialConsult.SpecialConsult_Caption as caption,
                                                    specialConsult.SpecialConsult_UserPhotoUrl as geRenZhao,
                                                    specialConsult.SpecialConsult_Detail as detail,
                                                	specialConsult.SpecialConsult_Date  as [date]
                                           from tb_SpecialConsultSelectExperts expert
                                           inner join tb_SpecialConsult specialConsult
                                       	  on expert.SpecialConsultSelectExpert_SpecialConsultId=specialConsult.SpecialConsult_Id
                                          where expert.SpecialConsultSelectExpert_ExpertId=@expertId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@expertId",expertId),
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            List<SpecialConsult_model> specialConsult_modelList = new List<SpecialConsult_model>();
            foreach (DataRow row in dataTable.Rows)
            {
                specialConsult_modelList.Add(ToShortModel(row));
            }
            return specialConsult_modelList;
        }


        /// <summary>
        /// 将一条数据转化为SpecialConsult_model数据
        /// 特定咨询帖子id  标题 用户个人照 详细描述 日期
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public SpecialConsult_model ToShortModel(DataRow row)
        {
            SpecialConsult_model specialConsult_model = new SpecialConsult_model();
            specialConsult_model.id = (int)row["id"];
            specialConsult_model.caption = row["caption"].ToString();
            specialConsult_model.userPhotoUrl = row["geRenZhao"].ToString();
            specialConsult_model.detail = row["detail"].ToString();
            specialConsult_model.datetime = (DateTime)row["date"];
            return specialConsult_model;
        }

        /// <summary>
        /// 将一条数据转化为SpecialConsult_model数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public SpecialConsult_model ToModel(DataRow row)
        {
            ///这里还缺少一个处理，那就是，当数据库里的某个字段为空时，要初始化什么值，或者做什么处理
            SpecialConsult_model specialConsult_model = new SpecialConsult_model();
            specialConsult_model.id = (int)row["id"];
            specialConsult_model.caption = row["caption"].ToString();
            specialConsult_model.userPhotoUrl = row["geRenZhao"].ToString();
            specialConsult_model.occasion = row["occasion"].ToString();
            specialConsult_model.likeStyleUrl = row["likeStyleUrl"].ToString();
            specialConsult_model.dislikeStyleUrl = row["dislikeStyleUrl"].ToString();
            specialConsult_model.detail = row["detail"].ToString();
            //咨询者数据
            specialConsult_model.user.userId = (int)row["userId"];
            specialConsult_model.user.userName = row["userName"].ToString();
            specialConsult_model.user.height = Convert.ToInt32(row["height"]);
            specialConsult_model.user.weight = Convert.ToInt32(row["weight"]);
            specialConsult_model.user.yaoWei = Convert.ToInt32(row["yaoWei"]);
            specialConsult_model.user.tuiChang = Convert.ToInt32(row["tuiChang"]);
            specialConsult_model.user.daTuiWei = Convert.ToInt32(row["daTuiWei"]);
            specialConsult_model.user.xiaoTunWei = Convert.ToInt32(row["xiaoTuiWei"]);
            specialConsult_model.user.tunWei = Convert.ToInt32(row["tunWei"]);
            specialConsult_model.user.biWei = Convert.ToInt32(row["biWei"]);
            specialConsult_model.user.xiongWei = Convert.ToInt32(row["xiongWei"]);
            specialConsult_model.user.skinColor = row["skinColor"].ToString();
            return specialConsult_model;
        }

        /// <summary>
        /// 用户特定咨询，保存用户特定咨询填写的数据
        /// </summary>
        /// <param name="userId">咨询者的id</param>
        /// <param name="expertId">专家id</param>
        /// <param name="occasion">场合</param>
        /// <param name="details">咨询详情</param>
        /// <param name="geRenZhaoUrl">个人照片url</param>
        /// <param name="likeStyleImageUrl">喜欢风格的照片url</param>
        /// <param name="dislikeStyleImageUrl">不喜欢风格的照片url</param>
        /// <returns></returns>
        public int InsertConsultData(int userId, string occasion, string details, string geRenZhaoUrl, string likeStyleImageUrl, string dislikeStyleImageUrl,DateTime datetime)
        {
            string sqlStr = @"insert into tb_SpecialConsult 
                                                             ( SpecialConsult_UserId, SpecialConsult_UserPhotoUrl,
                                                               SpecialConsult_Occasion, SpecialConsult_LikeStyleUrl, 
                                                               SpecialConsult_DislikeStyleUrl, SpecialConsult_Detail, 
                                                               SpecialConsult_Date)
                                                   values(@userId,@geRenZhaoUrl,
                                                               @occasion,@likeStyleImageUrl,
   	                                                           @dislikeStyleImageUrl,@details,@datetime)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userId",userId),
                new SqlParameter("@occasion",occasion),
                new SqlParameter("@details",details),
                new SqlParameter("@geRenZhaoUrl",geRenZhaoUrl),
                new SqlParameter("@likeStyleImageUrl",likeStyleImageUrl),
                new SqlParameter("@dislikeStyleImageUrl",dislikeStyleImageUrl),
                new SqlParameter("@datetime",datetime),
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);

        }

       /// <summary>
        /// 将专家对特定咨询的回答存入数据库tb_SpecialConsultAnswer
       /// </summary>
       /// <param name="specialConsultId">用户特定咨询数据表里的id</param>
       /// <param name="htmlUrl">专家回答的静态htmlurl</param>
       /// <returns></returns>
        public int InsertAnswerData(int specialConsultId, int expertId,string htmlUrl,DateTime datetime)
        {
            string sqlStr = @"insert tb_SpecialConsultAnswer
                                                      ( SpecialConsultAnswer_SpecialConsultId, SpecialConsult_ExpertId,SpecialConsultAnswer_HtmlUrl,SpecialConsultAnswer_Date)  
                                            values(@specialConsultId,@expertId,@htmlUrl,@datetime)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@specialConsultId",specialConsultId),
                new SqlParameter("@expertId",expertId),
                new SqlParameter("@htmlUrl",htmlUrl),
                new SqlParameter("@datetime",datetime),
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
            
        }

    }
}









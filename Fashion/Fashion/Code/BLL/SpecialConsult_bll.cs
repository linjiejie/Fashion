using Fashion.Code.DAL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Code.BLL
{
    public class SpecialConsult_bll
    {
        /// <summary>
        /// 用户特定咨询，保存用户特定咨询填写的数据
        /// </summary>
        /// <param name="userId">咨询者的id</param>
        /// <param name="expertId">专家id</param>
        /// <param name="occasion">场合</param>
        /// <param name="details">咨询详情</param>
        /// <param name="geRenZhaoFileName">个人照片名</param>
        /// <param name="likeStyleImageFileName">喜欢风格的照片名</param>
        /// <param name="dislikeStyleImageFileName">不喜欢风格的照片名</param>
        /// <returns></returns>

        public int InsertConsultData(int userId, string occasion, string details, string geRenZhaoFileName, string likeStyleImageFileName, string dislikeStyleImageFileName, DateTime datetime)
        {
            SpecialConsult_dal specialConsul_dal = new SpecialConsult_dal();
            string geRenZhaoUrl="/Images/ConsultImages/GeRenZhao/"+ geRenZhaoFileName;
            string likeStyleImageUrl="/Images/ConsultImages/LikeStyleImage/"+ likeStyleImageFileName;
            string dislikeStyleImageUrl = "/Images/ConsultImages/DisLikeStyleImage/" + dislikeStyleImageFileName;
            return specialConsul_dal.InsertConsultData(userId, occasion, details, geRenZhaoUrl, likeStyleImageUrl, dislikeStyleImageUrl,datetime);
        }


        /// <summary>
        /// 通过特定咨询的帖子编号specialConsultId查询数据库的tb_SpecialConsult，获取特定咨询的
        /// 用户咨询数据
        /// </summary>
        /// <param name="specialConsultId">特定咨询的帖子编号</param>
        /// </summary>
        /// <param name="specialConsultId"></param>
        /// <returns></returns>
        public SpecialConsult_model GetOneSpecialConsult(int specialConsultId)
        {
            SpecialConsult_dal specialConsult_dal = new SpecialConsult_dal();
            return specialConsult_dal.GetOneConsultData(specialConsultId);
        }
        /// <summary>
        /// 根据specialConsultId获取特定咨询的SpecialConsult_Caption，SpecialConsult_Occasion，SpecialConsult_Detail，SpecialConsult_Date
        /// </summary>
        /// <param name="specialConsultId"></param>
        /// <returns></returns>
        public SpecialConsult_model GetOneShortConsultData(int specialConsultId)
        {
            SpecialConsult_dal specialConsult_dal = new SpecialConsult_dal();
            return specialConsult_dal.GetOneShortConsultData(specialConsultId);
        }
        /// <summary>
        /// 通过用户名，查询该用户特定咨询过的帖子，
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 特定咨询帖子id  标题 用户个人照 详细描述 日期
        /// </summary>
        /// <param name="userName">用户名id</param>
        /// <returns></returns>
        public List<SpecialConsult_model> GetMyConsultData(int userId)
        {
            SpecialConsult_dal specialConsult_dal = new DAL.SpecialConsult_dal();
            List<SpecialConsult_model> specialConsult_modelList = specialConsult_dal.GetShortConsultData(userId);
            return specialConsult_modelList;
        }

        /// <summary>
        /// 通过专家名，查询特定咨询该专家的帖子，
        /// 这次不用查询出全部的数据，只需查询一部分数据，因为不是用于详情内容，而是用于遍历
        /// 特定咨询帖子id  标题 用户个人照 详细描述 日期
        /// </summary>
        /// <param name="userName">用户名id</param>
        /// <returns></returns>
        public List<SpecialConsult_model> GetShortConsultExpertData(int expertId)
        {
            SpecialConsult_dal specialConsult_dal = new DAL.SpecialConsult_dal();
            List<SpecialConsult_model> specialConsult_modelList = specialConsult_dal.GetShortConsultExpertData(expertId);
            return specialConsult_modelList;
        }










        /// <summary>
        /// 专家对特定咨询的解答
        /// 将专家对特定咨询的回答存入数据库tb_SpecialConsultAnswer，1条数据
        /// 成功返回1，失败返回0
        /// </summary>
        /// <param name="specialConsultId">用户特定咨询数据表里的id</param>
        /// <param name="htmlUrl">专家回答的静态htmlurl</param>
        /// <returns></returns>
        public int InsertAnswerData(int specialConsultId, int expertId, string htmlUrl, DateTime datetime)
        {
            SpecialConsult_dal specialConsult_dal = new SpecialConsult_dal();

            if (specialConsult_dal.InsertAnswerData(specialConsultId, expertId, htmlUrl, datetime) != 1)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Creator:Simple
        /// 通过用户特定咨询后保存的个人照路径查询该条特定咨询数据的id，因为个人照路径geRenZhaoUrl是唯一的
        /// </summary>
        /// <param name="geRenZhaoUrl"></param>
        /// <returns></returns>
        public int GetSpecialConsultId(string geRenZhaoFileName)
        {
            string geRenZhaoUrl = "/Images/ConsultImages/GeRenZhao/" + geRenZhaoFileName;
            SpecialConsult_dal specialConsult_dal = new SpecialConsult_dal();
            object objSpecialConsult_Id = specialConsult_dal.GetSpecialConsultId(geRenZhaoUrl);
            if (objSpecialConsult_Id == null || objSpecialConsult_Id == System.DBNull.Value)
            {
            }
            return Convert.ToInt32(objSpecialConsult_Id);
        }

        /// <summary>
        /// 通过专家解答的特定咨询staticConsultAnswerHtml查询该条数据的id
        /// </summary>
        /// <param name="staticConsultAnswerHtml"></param>
        /// <returns></returns>
        public int GetConsultAnswerId(string staticConsultAnswerHtml)
        {
            SpecialConsult_dal specialConsult_dal = new SpecialConsult_dal();
            object objSpecialConsultAnswer_Id = specialConsult_dal.GetConsultAnswerId(staticConsultAnswerHtml);
            if (objSpecialConsultAnswer_Id == null || objSpecialConsultAnswer_Id == System.DBNull.Value)
            { 

            }
            return Convert.ToInt32(objSpecialConsultAnswer_Id);
        }

    }
}
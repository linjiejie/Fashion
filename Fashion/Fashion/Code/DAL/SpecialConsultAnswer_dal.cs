using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fashion.Code.DAL
{
    public class SpecialConsultAnswer_dal
    {
        /// <summary>
        /// 通过特定咨询的帖子编号specialConsultId查询数据库的tb_SpecialConsultAnswer，
        /// 获取专家解答的数据
        /// 无论查询到多少条数据，返回一条SpecialConsultAnswer_model对象的实例的数据
        /// 失败： 查询不到数据，抛出异常0
        /// </summary>
        /// <param name="specialConsultId">特定咨询的帖子编号</param>
        /// <returns></returns>
        public SpecialConsultAnswer_model GetOneSpecialAnswerData(int specialConsultId, int expertId)
        {
            string sqlStr = @"select SpecialConsultAnswer_Id as specialAnswerId,
                                                   SpecialConsultAnswer_SpecialConsultId as specialConsultId,
                                            	   SpecialConsultAnswer_HtmlUrl as answerHtmlUrl,
                                            	   SpecialConsultAnswer_Date [datetime]
                                            from tb_SpecialConsultAnswer where SpecialConsultAnswer_SpecialConsultId=@specialConsultId and SpecialConsult_ExpertId=@expertId";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@specialConsultId",specialConsultId),
                new SqlParameter("@expertId",expertId)
            };
            DataTable dataTable = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            SpecialConsultAnswer_model specialConsultAnswer_model = new SpecialConsultAnswer_model();
            if (dataTable.Rows.Count == 0)
            {
                throw new Exception("0");
            }
            specialConsultAnswer_model = ToModel(dataTable.Rows[0]);
            return specialConsultAnswer_model;
        }

        /// <summary>
        /// 通过特定咨询的id查询tb_SpecialConsultSelectExperts表，过去所有选择的专家的解答的部分数据
        /// </summary>
        /// <param name="specialConsultId"></param>
        /// <returns></returns>
        public List<SpecialConsultAnswer_model> GetAllSelectExpertShortAnswer(int specialConsultId)
        {
            string sqlStr = @"select * from (
                                                             select tb_User.User_Name expertUserName,tb_User.User_TouXiangUrl touXiangUrl,
                                                             	   answer.SpecialConsultAnswer_Id specialAnswerId,
                                                                    cloth.SpecialConsultAnswerClothes_ClotheType clothType,
                                                             	   cloth.SpecialConsultAnswerClothes_ClotheUrl clothUrl
                                                             from (tb_SpecialConsultSelectExperts selectExpert 
                                                                     left join tb_User on selectExpert.SpecialConsultSelectExpert_ExpertId=tb_User.User_Id)
                                                             			inner join 
                                                             			(tb_SpecialConsultAnswer answer left join tb_SpecialConsultAnswerClothes cloth on answer.SpecialConsultAnswer_Id=cloth.SpecialConsultAnswerClothes_AnswerId)
                                                                      on selectExpert.SpecialConsultSelectExpert_ExpertId=answer.SpecialConsult_ExpertId
                                                             		    and selectExpert.SpecialConsultSelectExpert_SpecialConsultId=answer.SpecialConsultAnswer_SpecialConsultId
                                                              where selectExpert.SpecialConsultSelectExpert_SpecialConsultId=@specialConsultId
                                                         ) a order by a.specialAnswerId asc";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@specialConsultId",specialConsultId)
            };
            DataTable answerDt = SqlHelper.ExecuteDataTable(sqlStr, parameters);
            List<SpecialConsultAnswer_model> specialConsultAnswer_modelList = new List<SpecialConsultAnswer_model>();
            int answerId=-1;
            SpecialConsultAnswer_model specialConsultAnswer_model = new SpecialConsultAnswer_model();
            //说明，dataTable是用户特定咨询时选择的专家解答数据，该数据包含专家搭配的衣服，而专家搭配的衣服有很多件，
            //每一件对应一条数据，所以会存在多条specialAnswerId一致的数据，因此要做一些处理，让链表里保存的数据里specialAnswerId只存在一条
            //以下实现将dataTable的数据转化为List<SpecialConsultAnswer_model>链表数据，以下简称为链表
            for (int i = 0; i < answerDt.Rows.Count; i++)
            {
                //answerId存放当前数据的id，根据specialAnswerId的值与answerId对比，来判断是否为同一条专家解答数据
                DataRow currentRow = answerDt.Rows[i];//当前行数据
                if ((int)currentRow["specialAnswerId"] == answerId)//为同一个专家解答数据，只需添加衣服到specialConsultAnswer_model.clothList_model里
                {
                    SpecialConsultAnswerClothes_model specialConsultAnswerClothes_model1 = new SpecialConsultAnswerClothes_model();
                    specialConsultAnswerClothes_model1.clothType = currentRow["clothType"].ToString();
                    specialConsultAnswerClothes_model1.clothUrl = currentRow["clothUrl"].ToString();
                    specialConsultAnswer_model.clothList_model.Add(specialConsultAnswerClothes_model1);
                }
                else
                {//不是同一个专家解答数据，新建一条specialConsultAnswer_model数据
                    //清空specialConsultAnswer_model的属性值
                    specialConsultAnswer_model = new SpecialConsultAnswer_model();
                    //添加数据
                    specialConsultAnswer_model.expert_model.userName = currentRow["expertUserName"].ToString();
                    specialConsultAnswer_model.expert_model.touXiangUrl = currentRow["touXiangUrl"].ToString();
                    specialConsultAnswer_model.specialAnswerId = (int)currentRow["specialAnswerId"];
                    SpecialConsultAnswerClothes_model specialConsultAnswerClothes_model2 = new SpecialConsultAnswerClothes_model();
                    specialConsultAnswerClothes_model2.clothType = currentRow["clothType"].ToString();
                    specialConsultAnswerClothes_model2.clothUrl = currentRow["clothUrl"].ToString();
                    specialConsultAnswer_model.clothList_model.Add(specialConsultAnswerClothes_model2);
                    //设置当前的数据id
                    answerId = (int)currentRow["specialAnswerId"];
                }
                if (i == answerDt.Rows.Count - 1)//为最后一条数据
                {
                    specialConsultAnswer_modelList.Add(specialConsultAnswer_model);//添加当前数据到链表里
                    break;//结束
                }
                int j = i+1;
                if ((int)answerDt.Rows[j]["specialAnswerId"] != answerId)//与下一条数据不是同一个专家解答数据
                {
                    specialConsultAnswer_modelList.Add(specialConsultAnswer_model);//添加当前数据到链表里
                }

            }
            return specialConsultAnswer_modelList;
            }


        /// <summary>
        /// 将一条数据转化为SpecialConsultAnswer_model数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public SpecialConsultAnswer_model ToModel(DataRow row)
        {
            SpecialConsultAnswer_model specialConsultAnswer_model = new SpecialConsultAnswer_model();
            specialConsultAnswer_model.specialAnswerId = (int)row["specialAnswerId"];
            specialConsultAnswer_model.specialConsult_model.id = (int)row["specialConsultId"];
            specialConsultAnswer_model.answerHtmlUrl = row["answerHtmlUrl"].ToString();
            specialConsultAnswer_model.date = (DateTime)row["datetime"];
            return specialConsultAnswer_model;
        }
    }
}
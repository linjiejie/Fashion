using Fashion.Code.DAL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Code.BLL
{
    public class SpecialConsultAnswer_bll
    {
        /// <summary>
        /// 通过特定咨询的帖子编号specialConsultId查询数据库的tb_SpecialConsultAnswer，
        /// 获取专家解答的数据
        /// </summary>
        /// <param name="specialConsultId">特定咨询的帖子编号</param>
        /// <returns></returns>
        public SpecialConsultAnswer_model GetOneSpecialAnswerData(int specialConsultId, int expertId)
        {
            SpecialConsultAnswer_dal specialConsultAnswer_dal = new SpecialConsultAnswer_dal();
            SpecialConsultAnswer_model specialConsultAnswer_model = new SpecialConsultAnswer_model();
            try {
                specialConsultAnswer_model = specialConsultAnswer_dal.GetOneSpecialAnswerData(specialConsultId, expertId);
            }
            catch (Exception e)
            {
                if (e.ToString() == "0")
                {//当查询到的数据为空时，初始化specialConsultAnswer_model的值
                    specialConsultAnswer_model.specialAnswerId = 0;
                    specialConsultAnswer_model.answerHtmlUrl = "";
                }
            }
                
            return specialConsultAnswer_model;
        }


        public List<SpecialConsultAnswer_model> GetAllSelectExpertShortAnswer(int specialConsultId)
        {
            SpecialConsultAnswer_dal specialConsultAnswer_dal = new SpecialConsultAnswer_dal();
            return specialConsultAnswer_dal.GetAllSelectExpertShortAnswer(specialConsultId);
        }

    }
}
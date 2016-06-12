using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Code.DAL
{
    public class SpecialConsultSelectExperts_dal
    {
        /// <summary>
        /// 插入特定咨询选择的专家
        /// </summary>
        /// <param name="specialConsultId"></param>
        /// <param name="expertIdList"></param>
        /// <returns></returns>
        public int InsertSpecialConsultSelectExperts(int specialConsultId,List<string> expertIdList)
        {
            string sqlStr="";
            foreach (string expertId in expertIdList)
            {
                sqlStr = sqlStr + @"insert into tb_SpecialConsultSelectExperts (SpecialConsultSelectExpert_SpecialConsultId,SpecialConsultSelectExpert_ExpertId) " +
                                   "values (" + specialConsultId + "," + expertId + ")";
            }
            return SqlHelper.ExecuteNonquery(sqlStr);
        }

        
    }
}
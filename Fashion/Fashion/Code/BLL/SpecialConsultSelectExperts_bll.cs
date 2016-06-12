using Fashion.Code.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Code.BLL
{
    public class SpecialConsultSelectExperts_bll
    {
        public int InsertSpecialConsultSelectExperts(int specialConsultId, List<string> expertIdList)
        {
            SpecialConsultSelectExperts_dal specialConsultSelectExperts_dal = new SpecialConsultSelectExperts_dal();
            return specialConsultSelectExperts_dal.InsertSpecialConsultSelectExperts(specialConsultId, expertIdList);
        }
    }
}
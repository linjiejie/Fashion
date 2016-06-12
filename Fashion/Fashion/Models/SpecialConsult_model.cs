using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fashion.Models
{
    public class SpecialConsult_model
    {
        public int id;
        public string caption;
        public string userPhotoUrl;
        public string occasion;
        public string likeStyleUrl;
        public string dislikeStyleUrl;
        public string detail;
        public DateTime datetime;
        public User_model user;//咨询者，即普通用户
        public SpecialConsult_model(){
            user = new User_model();
    }
    }
}
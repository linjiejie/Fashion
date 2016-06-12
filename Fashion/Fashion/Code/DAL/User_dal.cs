using Fashion.Code.BLL;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fashion.Code.DAL
{
    public class User_dal
    {

              /// <summary>
              /// 查询关注记录是否存在 返回object
              /// 存在返回查询字段concernNameId，无记录返回null
              /// </summary>
              /// <param name="concernNameId"></param>
              /// <param name="beConcernNameId"></param>
              /// <returns></returns>
              public object select_ConIdAndBeConId(int concernNameId,int beConcernNameId)
              {
                  string sqlStr = "select Attention_Id from tb_Attention where Attention_ConcernsId=@concernNameId and Attention_BeConcernedId=@beConcernNameId";
	                  SqlParameter[] parameters = new SqlParameter[] {  
                     new SqlParameter("@concernNameId",concernNameId),
                     new SqlParameter("@beConcernNameId",beConcernNameId),
                   
                           };
                      return SqlHelper.ExecuteScalar(sqlStr, parameters);

              }
               
              /// <summary>
              /// 插入关注记录到tb_Attention，返回int
              /// 成功返回1，失败返回0
              /// </summary>
              /// <param name="concernNameId"></param>
              /// <param name="beConcernNameId"></param>
              /// <returns></returns>
              public int insert_IdTotb_Attention(int concernNameId, int beConcernNameId)
              {
                  string sqlStr = "insert into tb_Attention(Attention_ConcernsId,Attention_BeConcernedId)values(@concernNameId,@beConcernNameId)";
                  SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@concernNameId",concernNameId),
                    new SqlParameter("@beConcernNameId",beConcernNameId),
                   
                   };
                  return SqlHelper.ExecuteNonquery(sqlStr, parameters);

              }
              
                  /// <summary>
                  /// 取消关注操作,删除记录，返回int,成功返回1，失败返回0
                  /// </summary>
                  /// <param name="concernNameId"></param>
                  /// <param name="beConcernNameId"></param>
                  /// <returns></returns>
                  public int delete_IdFromtb_Attention(int concernNameId,int beConcernNameId)
                  {
	                    string sqlStr="delete from tb_Attention where Attention_ConcernsId=@concernNameId and Attention_BeConcernedId=@beConcernNameId";
	                       SqlParameter[] parameters = new SqlParameter[] { 
                                new SqlParameter("@concernNameId",concernNameId),
                                new SqlParameter("@beConcernNameId",beConcernNameId),
                   
                               };        
                            return SqlHelper.ExecuteNonquery(sqlStr, parameters);


                  }







        /// <summary>
        /// 更新User_StarCount(感谢)成功返回1
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object UpdateStarCount(string userName,string Num)
        {   
            
            string sqlStr1 = "update [tb_User] set User_StarCount=User_StarCount+1 where User_Name=@userName";
            string sqlStr2 = "update [tb_User] set User_StarCount=User_StarCount-1 where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[]{
                  new SqlParameter("@userName",userName)
                   };
            if (Num == "1")
                return SqlHelper.ExecuteNonquery(sqlStr1, parameters);
            else if (Num == "0")
                return SqlHelper.ExecuteNonquery(sqlStr2, parameters);
            else return 2;

        }


        /// <summary>
        /// 通过用户名查询数据库里该用户的条数
        /// </summary>
        /// <param name="userName">用户名</param>       
        /// <returns></returns>
        public object GetAccountCount(string userName)
        {
            string sqlStr = "select count(*) from [tb_User] where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }
        

       /// <summary>
       /// 获取用户的密码
       /// </summary>
        /// <param name="userName">用户的账号</param>
       /// <returns></returns>
        public object GetPassword(string userName)
        {
            string sqlStr = "select [User_Password] from [tb_User] where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }



        /// <summary>
        /// 通过用户名查找用户的真实名字
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetRealName (string userName)
        {
            string sqlStr = "select User_RealName from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        /// <summary>
        /// 通过用户名查找用户的个性签名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetSignature(string userName)
        {
            string sqlStr = "select User_Signature from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

       /// <summary>
        /// 通过通过用户名查找用户的出生年月日
       /// </summary>
       /// <param name="userName"></param>
       /// <returns></returns>
        public object GetBirthDate(string userName)
        {
            string sqlStr = "select User_BirthDate from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }



        /// <summary>
        /// 通过用户名查找用户的职业
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetProfession(string userName)
        {
            string sqlStr = "select User_Profession from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }
        /// <summary>
        /// 通过用户名查找用户的手机
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetPhoneNumber(string userName)
        {
            string sqlStr = "select  User_PhoneNumber from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        /// <summary>
        /// 通过用户名查找用户的学历
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetEducationalBackground(string userName)
        {
            string sqlStr = "select  User_EducationalBackground from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        /// <summary>
        /// 通过用户名查找用户的兴趣
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetInterest(string userName)
        {
            string sqlStr = "select User_Interest from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        

        /// <summary>
        /// 获取用户的个人基本信息（真实姓名，生日，职业，手机，学历，爱好）
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetPersonalInformation(string userName)
        {
            string sqlStr = "select User_RealName,User_BirthDate, User_Profession,User_PhoneNumber,User_EducationalBackground,User_Interest from [tb_User] where User_Name =  @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
           return SqlHelper.ExecuteDataTable(sqlStr, parameters);
        }
        /// <summary>
        /// 获取用户身体信息 身高 腰围 体重 臀围 胸围 腿长 大腿围 小腿围 臂围 肤色 返回整个表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetBodyData(string userName)
        {
            string sqlStr = "select User_SkinColor,User_Weight, User_XiongWei,User_YaoWei,User_TunWei,User_Height,User_LegLength,User_ThighGirth,User_CalfGirth,User_ArmGirth,User_QuanShenZhaoUrl from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteDataTable(sqlStr, parameters);
        }



        /// <summary>
        /// 根据用户的userId 和等级名rankName  获取用户的 特定咨询数（或特定解答数） 提问数 回答数 收藏 关注数 粉丝数 获赞数
        /// rankName为普通用户时，查询特定咨询数
        ///                  为专家时，查询特定解答数
        /// 成功返回CountUser_model对象的实例   
        /// Creator:Simple
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="rankName"></param>
        /// <returns></returns>
        public CountUser_model GetCountUser(int userId,string rankName)
        {
            string sqlStr = @"select SpecialConsult.specialConsultCount,
                                                   Post.zhuTieCount,
                                            	   ReplyPost.replyCount,
                                            	   Collect.collectCount,
                                            	   Concerns.concernsCount,
                                            	   Fans.fansCount,
                                            	   tb_User.User_StarCount as supportCount
                                                   from  tb_User left join
                                                         (select Post_SenderId senderId,count(*) zhuTieCount from tb_Post 
                                                                 group by Post_SenderId) as Post 
                                            					 on tb_User.User_Id=Post.senderId 
                                            					 left join
                                            		     (select ReplyPost_ReplyerId replyId,COUNT(*) replyCount from tb_ReplyPost
                                                                 group by ReplyPost_ReplyerId) as ReplyPost 
                                            					 on tb_User.User_Id=ReplyPost.replyId
                                            					 left join
                                            					 (select  Collect_CollectorId collectorId,COUNT(*) collectCount from tb_Collect
                                            					  group by Collect_CollectorId) as Collect
                                            					  on tb_User.User_Id=Collect.collectorId
                                            					  left join 
                                            		     (select Attention_ConcernsId as concernsId,count(*) concernsCount from tb_Attention
                                                                  group by Attention_ConcernsId)as Concerns
                                            					  on tb_User.User_Id=Concerns.concernsId
                                            					  left join 
                                            			 (select Attention_BeConcernedId as beConcernedId,count(*) fansCount  from tb_Attention 
                                                                  group by Attention_BeConcernedId)as Fans
                                            					  on tb_User.User_Id=Fans.beConcernedId  ";
            if (rankName == "普通用户")// rankName为普通用户时，查询特定咨询数
            {
                sqlStr = sqlStr + @"left join
		                                     (select SpecialConsult_UserId userId, count(*) specialConsultCount 
			                                   from tb_SpecialConsult
                                               group by SpecialConsult_UserId)  as SpecialConsult on tb_User.User_Id=SpecialConsult.userId					  
                                               where tb_User.User_Id=@userId";
            }
            else
                if (rankName == "专家")//rankName为专家时，查询特定解答数
                {
                    sqlStr = sqlStr + @"  left join
		                                     (select SpecialConsultSelectExpert_ExpertId userId, count(*) specialConsultCount 
                                              from tb_SpecialConsultSelectExperts
                                              group by SpecialConsultSelectExpert_ExpertId)  as SpecialConsult on tb_User.User_Id=SpecialConsult.userId					  
                                               where tb_User.User_Id=@userId";
                }
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userId",userId)
            };
            DataTable dt=SqlHelper.ExecuteDataTable(sqlStr,parameters);
            if(dt.Rows.Count>1)
            {
                throw new Exception("数据库出错，查询到的数据条数超过1条");//数据库存在多条数据
            }
            if (dt.Rows.Count == 0)
            {//查询到的数据条数为0
                return new CountUser_model();
            }
            return ToModel_CountUser(dt.Rows[0]);
        }


        /// <summary>
        /// Creator:Simple
        /// 获取一定数量的专家的数据：id、用户名、头像url          
        /// </summary>
        /// <returns></returns>
        public List<ExpertUserConsult_model> GetExpertConsult()
        {
            string sqlStr = "select User_Id,User_Name,User_TouXiangUrl from tb_User where User_RankId='3'";
            DataTable dataTableExpertConsult = SqlHelper.ExecuteDataTable(sqlStr);
            List<ExpertUserConsult_model> expertUserConsult_modelList = new List<ExpertUserConsult_model>();
            foreach(DataRow row in dataTableExpertConsult.Rows)
            {
                expertUserConsult_modelList.Add(ToModelExpertUserConsult(row));
            }
            return expertUserConsult_modelList;
        }


        /// <summary>
        /// 通过用户名获取用的个人资料《特定咨询》
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User_model GetUserDataConsult(string userName)
        {
            User_model user_model = new User_model();
            string sqlStr = "select User_BirthDate,User_Height,User_SkinColor,User_Weight,User_XiongWei,User_YaoWei,User_TunWei  from tb_User  where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userName",userName)
            };
            DataTable userData = SqlHelper.ExecuteDataTable(sqlStr,parameters);
            if (userData.Rows.Count == 0)
            {//不存在该用户时
                throw new Exception("不存在该用户，查询到的数据为空");
            }
            if (userData.Rows[0]["User_BirthDate"] == System.DBNull.Value)
            {
            
            }
            else
            {
                user_model.birthDate = (DateTime)userData.Rows[0]["User_BirthDate"];
            }

            user_model.height = userData.Rows[0]["User_Height"] == System.DBNull.Value ? 0 : Convert.ToSingle(userData.Rows[0]["User_Height"]);
            user_model.tunWei = userData.Rows[0]["User_TunWei"] == System.DBNull.Value ? 0 : Convert.ToSingle(userData.Rows[0]["User_TunWei"]);
            user_model.yaoWei = userData.Rows[0]["User_YaoWei"] == System.DBNull.Value ? 0 : Convert.ToSingle(userData.Rows[0]["User_YaoWei"]);
            user_model.xiongWei = userData.Rows[0]["User_XiongWei"] == System.DBNull.Value ? 0 : Convert.ToSingle(userData.Rows[0]["User_XiongWei"]);
            user_model.weight = userData.Rows[0]["User_Weight"] == System.DBNull.Value ? 0 : Convert.ToSingle(userData.Rows[0]["User_Weight"]);
            user_model.skinColor = userData.Rows[0]["User_SkinColor"] == System.DBNull.Value ? "请选择" : userData.Rows[0]["User_SkinColor"].ToString();
            return user_model;
        }

        

        /// <summary>
        /// 更新个人信息的身高 腰围 体重 臀围 胸围 腿长 大腿围 小腿围 臂围 肤色 成功返回1
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="SkinColor"></param>
        /// <param name="Weight"></param>
        /// <param name="XiongWei"></param>
        /// <param name="YaoWei"></param>
        /// <param name="TunWei"></param>
        /// <param name="Height"></param>
        /// <param name="LegLength"></param>
        /// <param name="ThighGirth"></param>
        /// <param name="ArmGirth"></param>
        /// <param name="CalfGirth"></param>
        /// <returns></returns>
        public int UpdateBodyData(string UserName, string SkinColor, float Weight, float XiongWei, float YaoWei, float TunWei, float Height, float LegLength, float ThighGirth, float ArmGirth, float CalfGirth)
        {
            string sqlStr = @"update [tb_User]  set  User_Weight =@Weight,User_XiongWei=@XiongWei,User_YaoWei=@YaoWei,User_SkinColor=@SkinColor
						,User_TunWei=@TunWei,User_Height=@Height,User_LegLength=@LegLength,
						User_ThighGirth=@ThighGirth,User_ArmGirth=@ArmGirth,User_CalfGirth=@CalfGirth
                             where User_Name=@UserName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("UserName",UserName),
                new SqlParameter("SkinColor",SkinColor),
                new SqlParameter("Weight",Weight),
                new SqlParameter("XiongWei",XiongWei),
                new SqlParameter("YaoWei",YaoWei),
                new SqlParameter("TunWei",TunWei),
                new SqlParameter("Height",Height),
                new SqlParameter("LegLength",LegLength),
                new SqlParameter("ThighGirth",ThighGirth),
                new SqlParameter("ArmGirth",ArmGirth), 
                new SqlParameter("CalfGirth",CalfGirth),
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }


        

        /// <summary>
        /// 更新个人信息的数据 真实姓名，生日，职业，手机号，学历，兴趣
        /// 插入成功返回1
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="BirthDate">生日</param>
        /// <param name="Profession">职业</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="EducationalBackground">学历</param>
        /// <param name="Interest">兴趣</param>
        /// <returns></returns>
        public int UpdateInformation(string UserName, string RealName, string BirthDate, string Profession, string PhoneNumber, string EducationalBackground, string Interest)
        {
            string sqlStr = @"update [tb_User] set User_RealName = @RealName,User_BirthDate=@BirthDate,User_Profession=@Profession,
                              User_PhoneNumber= @PhoneNumber,User_EducationalBackground=@EducationalBackground,User_Interest=@Interest
                             where User_Name=@UserName";

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("UserName",UserName),
                new SqlParameter("RealName",RealName),
                new SqlParameter("BirthDate",BirthDate),
                new SqlParameter("Profession",Profession),
                new SqlParameter("PhoneNumber",PhoneNumber),
                new SqlParameter("EducationalBackground",EducationalBackground),
                new SqlParameter("Interest",Interest),
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }

        

       


        /// <summary>
        /// 通过用户名查询该用户的盐值，返回类型为object
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public object GetSalt(string userName)
        {
            string sqlStr = "select User_Salt from [tb_User] where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

        /// <summary>
        /// 通过用户名获取该用户的密码和盐值
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //public DataTable GetPwdAndSalt(string userName)
        //{
        //    string sqlStr = "select [password],[salt] from [user] where userName=@userName";
        //    SqlParameter[] parameters = new SqlParameter[] { 
        //        new SqlParameter("@userName",userName)
        //    };
        //    return SqlHelper.ExecuteDataTable(sqlStr,parameters);
        //}
        public User_model GetPwdAndSaltModel(string userName)
        {
            string sqlStr = "select [User_Password],[User_Salt] from [tb_User] where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userName",userName)
            };
            DataTable dt=SqlHelper.ExecuteDataTable(sqlStr,parameters);
            if (dt.Rows.Count > 1)
            {
                //数据库出错处理，数据库里存在大于两条用户名一样的数据,抛出异常
                throw new Exception("more than 1 row was found");
            }
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];
            //把取回来的dt_User表的一行数据转化为model
            User_model model = new User_model();
            model.password = (string)row["User_Password"];
            model.salt = (string)row["User_Salt"];
            return model;
        }

       
        /// <summary>
        /// 通过用户名获取该用户的头像的url，返回结果为string型
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public object GetImgUrlTouXiang(string userName)
        {
            string sqlStr = "select User_TouXiangUrl from tb_User where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr,parameters);
        }

       
        /// <summary>
        /// 实现将用户的头像的url插入到数据库的功能，url为相对路径如：/Images/TouXiang/userName.png
        /// 函数返回受影响的函数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="urlTouXiang">图片相对路径</param>
        /// <returns></returns>
        public int InsertUrlTouXiang(string userName,string urlTouXiang)
        {
            string sqlStr = "update tb_User set User_TouXiangUrl=@urlTouXiang where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@urlTouXiang",urlTouXiang),
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }


        /// <summary>
        /// 实现将用户的全身照的url插入到数据库的功能，url为相对路径如：/Images/QuanShenZhao/userName.png
        /// 函数返回受影响的函数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="urlQuanShenZhao">图片相对路径</param>
        /// <returns></returns>
        public int InsertUrlQuanShenZhao(string userName, string urlQuanShenZhao)
        {
            string sqlStr = "update tb_User set User_QuanShenZhaoUrl=@urlQuanShenZhao where User_Name=@userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName),
                new SqlParameter("@urlQuanShenZhao",urlQuanShenZhao)
            };
            return SqlHelper.ExecuteNonquery(sqlStr,parameters);
        }


        //未编辑
        /// <summary>
        /// 封装sqlparameters的功能
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        //public SqlParameter[] pp(params Dictionary<string,string>)
        //{ 
        //    SqlParameter[] ppp=new SqlParameter[]{
        //    };
        //    return ppp;
        //}
      
        /// <summary>
        /// 查询tb_User表，获取指定的一行数据
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public User_model Get(string user_id)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select  * from tb_User where User_Id=@user_id", new SqlParameter("@user_id", user_id));
            if(dt.Rows.Count>1)
            {
                throw new Exception("more than 1 row was found");
            }
            if(dt.Rows.Count<=0)
            {
                return null;
            }
           /////////////////////////////////////////////////////////
            //还没写完，因为还没用到，所以以后再写
            DataRow row=dt.Rows[0];
            User_model model = ToModel(row);
            /////////////////////////////////////////////////////////
            return model;     
        }


        /// <summary>
        /// 将一条数据转化为CountUser_model 用户的：点赞数 关注数 粉丝数 收藏数 提问数 回帖数 特定咨询数 等
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public CountUser_model ToModel_CountUser(DataRow row)
        {
            CountUser_model countUser_model = new CountUser_model();
            countUser_model.specialConsultCount = row["specialConsultCount"] == System.DBNull.Value ? 0 :(int) row["specialConsultCount"];
            countUser_model.zhuTieCount = row["zhuTieCount"] == System.DBNull.Value ? 0 : (int)row["zhuTieCount"];
            countUser_model.replyCount = row["replyCount"] == System.DBNull.Value ? 0 : (int)row["replyCount"];
            countUser_model.collectCount = row["collectCount"] == System.DBNull.Value ? 0 : (int)row["collectCount"];
            countUser_model.concernsCount = row["concernsCount"] == System.DBNull.Value ? 0 : (int)row["concernsCount"];
            countUser_model.fansCount = row["fansCount"] == System.DBNull.Value ? 0 : (int)row["fansCount"];
            countUser_model.supportCount = row["supportCount"] == System.DBNull.Value ? 0 : (int)row["supportCount"];
            return countUser_model;
        }

        /// <summary>
        /// 将从数据库里取回的一行数据转化为User_model数据
        /// </summary>
        /// <param name="row">一行数据</param>
        /// <returns></returns>
        private static User_model ToModel(DataRow row)
        {
            User_model model=new User_model();
            /////////////////////////////////////////////////////////
            //还没写完，因为还没用到，所以以后再写
            model.userId=(int)row["User_Id"];
            model.userName = (string)row["User_Name"];
            /////////////////////////////////////////////////////////
            return model;
        }

        /// <summary>
        /// 将row数据转化为ExpertUserConsult_model数据（专家的数据）
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ExpertUserConsult_model ToModelExpertUserConsult(DataRow row)
        {
            ExpertUserConsult_model expertUserConsult_model = new ExpertUserConsult_model();
            expertUserConsult_model.expertId = (int)row["User_Id"];
            expertUserConsult_model.userName = row["User_Name"].ToString();
            expertUserConsult_model.touXiangUrl = row["User_TouXiangUrl"].ToString();
            return expertUserConsult_model;
        }








        /// <summary>
        /// 专家注册，注册成功返回1
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="rankId"></param>
        /// <param name="Email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="profession"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        public int InsertExrertRegisterstring(string userName, string realName, string password, string salt, string rankId, string Email, string phoneNumber, string profession, string introduction,string quanShenZhaoUrl,string touXiangUrl)
        {
            string sqlStr = "insert into [tb_User](User_Name,User_RealName,User_QuanShenZhaoUrl,User_TouXiangUrl,[User_Password],User_Salt,User_RankId,User_Email,User_PhoneNumber,User_Profession,User_Introduction)values(@userName,@realName,@quanShenZhaoUrl,@touXiangUrl,@password,@salt,@rankId,@email,@phoneNumber,@profession,@introduction)";
            SqlParameter[] parameters = new SqlParameter[]{
            new SqlParameter("@userName", userName),
            new SqlParameter("@realName",realName),
            new SqlParameter("@quanShenZhaoUrl",quanShenZhaoUrl),
            new SqlParameter("@touXiangUrl",touXiangUrl),
            new SqlParameter("@password",password),
            new SqlParameter("@salt", salt),
            new SqlParameter("@rankId", rankId),
            new SqlParameter("@Email",Email),
            new SqlParameter("@phoneNumber", phoneNumber),
            new SqlParameter("@profession", profession),
            new SqlParameter("@introduction", introduction),
          };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);

        }



        /// <summary>
        /// 用户注册，注册成功返回1
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="salt">盐值</param>
        /// <param name="password">密码</param>
        /// <param name="rankId">等级编号</param>
        /// <returns></returns>
        public int InsertRegister(string userName,string salt,string password,string rankId)
        {

            string sqlStr = "insert into [tb_User] (User_Name,User_Salt,[User_Password],User_RankId ) values (@userName,@salt,@password,@rankId)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("userName",userName),
                new SqlParameter("salt",salt),
                new SqlParameter("password",password),
                new SqlParameter("rankId",rankId)
            };
            return SqlHelper.ExecuteNonquery(sqlStr,parameters);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 用户使用手机号注册，注册成功返回1
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="salt">盐值</param>
        /// <param name="password">密码</param>
        /// <param name="rankId">等级编号</param>
        /// <returns></returns>
        public int InsertPhoneNumberRegister(string userName, string salt, string password, string rankId, string phoneNumber, string quanShenZhaoUrl, string touXiangUrl)
        {

            string sqlStr = "insert into [tb_User] (User_Name,User_Salt,User_QuanShenZhaoUrl,User_TouXiangUrl,[User_Password],User_RankId,User_PhoneNumber) values (@userName,@salt,@quanShenZhaoUrl,@touXiangUrl,@password,@rankId,@phoneNumber)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("userName",userName),
                new SqlParameter("@salt",salt),
                new SqlParameter("@quanShenZhaoUrl",quanShenZhaoUrl),
                new SqlParameter("touXiangUrl",touXiangUrl),
                new SqlParameter("password",password),
                new SqlParameter("rankId",rankId),
                new SqlParameter("phoneNumber",phoneNumber)
            };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        //1.函数要首字母大写
        /// <summary>
        /// 用户使用邮箱注册，注册成功返回1
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="salt"></param>
        /// <param name="password"></param>
        /// <param name="rankId"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public int InsertEmailRegister(string userName, string salt, string password, string rankId, string email, string quanShenZhaoUrl, string touXiangUrl)
        {
            string sqlStr = "insert into [tb_User] (User_Name,User_Salt,User_QuanShenZhaoUrl,User_TouXiangUrl,[User_Password],User_RankId,User_Email) values (@userName,@salt,@quanShenZhaoUrl,@touXiangUrl,@password,@rankId,@email)";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("userName",userName),
                new SqlParameter("@salt",salt),
                 new SqlParameter("@quanShenZhaoUrl",quanShenZhaoUrl),
                new SqlParameter("touXiangUrl",touXiangUrl),
                new SqlParameter("password",password),
                new SqlParameter("rankId",rankId),
                new SqlParameter("email",email)
                 };
            return SqlHelper.ExecuteNonquery(sqlStr, parameters);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////

      
        //public DataTable Get(string[] columnNames,)
        //{ 
        //    if(columnNames.Length<=0)
        //    { }
        //    string sqlStr = "select ";
        //    sqlStr=sqlStr+columnNames[0];
        //    for(int i=1;i<columnNames.Length;i++)
        //    {
        //        sqlStr = sqlStr + "," + columnNames[i];
        //    }
        //    sqlStr = sqlStr + " from User where userId=@userId";
        //    SqlParameter[] parameters = new SqlParameter[]{
        //        new SqlParameter("@userId",userId)
        //    };

                
            
        //}
        /// <summary>
        /// 通过用户名查询该用户的ID，返回类型为object
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public object GetUserId(string userName)
        {
            string sqlStr = "select USER_ID from [tb_User] where User_Name = @userName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            return SqlHelper.ExecuteScalar(sqlStr, parameters);
        }

    }
}
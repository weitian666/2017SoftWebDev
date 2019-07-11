using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ELearning.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleAuditController : Controller
    {
        private DataTable dt = new DataTable();
        private SqlConnection conn;

        public IActionResult Index()
        {
            string sql = "select  * from Articles  where news_state=0";
            dt = GetdataSet(sql);

            return View(dt);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int aa(string name)
        {
            try
            {
                string sql = "UPDATE Articles SET  news_state = 1   WHERE Name = '" + name + "'"; 
                int a = ExecuteSql(sql);
                return a;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SqlConnection openConnection()
        {                                                   
            string connstr = "Server=localhost;Initial Catalog=ELearningDB; uid=sa;pwd=123456;MultipleActiveResultSets=True"; 
            {
                conn = new SqlConnection(connstr);
                conn.Open();
            }
            return conn;
        }
        /// <summary>
        /// 
        /// </summary>
        private void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        /// <summary>
        /// 执行数据库查询的封装类
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetdataSet(string sql)
        {
            try
            {
                SqlCommand comm = new SqlCommand(sql, openConnection());
                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                closeConnection();
                return ds.Tables[0];//返回的是查到的整个数据表
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 执行数据库的修改表封装类
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            try//
            {
                openConnection();
                SqlCommand comm = new SqlCommand(sql, conn);
                int i = comm.ExecuteNonQuery();
                closeConnection();
                return i;//返回的是执行那个修改结果  如果是0那就是没成功   1就是执行成功
            }
            catch (Exception e)
            {
                Console.Write(e);//输出错误结果
                return 0;
            }
        }
    }
}
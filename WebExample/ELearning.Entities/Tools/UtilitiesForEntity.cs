using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace ELearning.Entities.Tools
{
    public class UtilitiesForEntity
    {
        /// <summary>
        /// 提取根据系统时间生成 SortCode 所需要的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string SortCodeByDefaultDateTime<T>()
        {
            var result = "Default";
            var timeStampString = "";

            var nowTime = DateTime.Now;
            timeStampString = nowTime.ToString("yyyy-MM-dd-hh-mm-ss-ffff", DateTimeFormatInfo.InvariantInfo);

            var entityName = typeof(T).Name;
            result = entityName + "_" + timeStampString;
            return result;
        }


        public static string GetRandomCode(int num)
        {
            string[] source = { "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string code = "";
            int p = int.Parse(DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            Thread.Sleep(1);
            Random rd = new Random(p);
            for (int i = 0; i < num; i++)
            {
                code += source[rd.Next(0, source.Length)];
            }
            return code;
        }

        public static string GetValidateCode(int number)
        {
            //number :验证码的位数
            //amount :生成的验证码个数
            int amount = 200;
            int a;
            List<ValidateCode> valiCodes = new List<ValidateCode>();
            string[] codeArray = new string[amount];
            Random rd = new Random();
            var i = 0;
            do
            {
                string[] source = { "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string code = "";

                for (var j = 0; j < number; j++)
                {
                    code += source[rd.Next(0, source.Length)];
                }

                a = 0;
                foreach (var valicode in valiCodes)
                {
                    if (valicode.Name == code)
                    {
                        a = 1;
                        break;
                    }
                }
                if (a != 1)
                {
                    var valicode = new ValidateCode()
                    {
                        Name = code
                    };
                    codeArray[i] = code;
                    valiCodes.Add(valicode);
                    i++;
                }
            } while (i != amount);
            System.Threading.Thread.Sleep(10);
            string singleCode = codeArray[rd.Next(0, codeArray.Length)];
            return singleCode;
        }

    }
}

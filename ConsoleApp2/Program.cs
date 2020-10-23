using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"D:\zjc\new 25.txt";
            string data = JsonStr(fileName);
            JObject Json = JObject.Parse(data);
            //获取主要字段
            string result = GetInformation(Json);
            //控制台打印
            Console.WriteLine(result);
        }

        private static string GetInformation(JObject Json)
        {
            //输出字符串
            string consoleString = "";

            //标题
            string title = Json["data"]["formInfo"]["widgetMap"]["_S_TITLE"]["value"].ToString();
            consoleString += title + "\n";

            //市场区域
            string marketTitle = Json["data"]["formInfo"]["widgetMap"]["Ra_0"]["title"].ToString();//标题
            consoleString += marketTitle + ": ";
            List<MarketArea> markets = GetMarket(Json["data"]["formInfo"]["widgetMap"]["Ra_0"]["options"].ToString());
            string marketKey = Json["data"]["formInfo"]["widgetMap"]["Ra_0"]["value"].ToString();
            string marketArea= "";//内容
            foreach (var item in markets)
            {
                if (item.Key.Equals(marketKey))
                    marketArea = item.Value;
            }
            consoleString += marketArea + "\n";

            //执行人
            string personsTitle = Json["data"]["formInfo"]["widgetMap"]["Ps_0"]["title"].ToString();//标题
            consoleString += personsTitle + ": ";
            List<PersonInfo> persons = GetPerson(Json["data"]["formInfo"]["widgetMap"]["Ps_0"]["personInfo"].ToString());
            string executor = "";//内容
            persons.ForEach(p =>
            {
                executor += p.name.ToString() + ", ";
            });
            consoleString += executor + "\n";

            //申请内容
            string contentTitle = Json["data"]["formInfo"]["widgetMap"]["Ta_0"]["title"].ToString();//标题
            consoleString += contentTitle + ": ";
            string content = Json["data"]["formInfo"]["widgetMap"]["Ta_0"]["value"].ToString();//内容
            consoleString += content + "\n";

            //费用类型
            string feeKeyTitle = Json["data"]["formInfo"]["widgetMap"]["Ra_1"]["title"].ToString();//标题
            consoleString += feeKeyTitle + ": ";
            List<FeeType> feeTypes = GetFeeType(Json["data"]["formInfo"]["widgetMap"]["Ra_1"]["options"].ToString());
            string feeKey = Json["data"]["formInfo"]["widgetMap"]["Ra_1"]["value"].ToString();//内容
            string feeType = "";
            foreach (var item in feeTypes)
            {
                if (item.Key.Equals(feeKey))
                    feeType = item.Value;
            }
            consoleString += feeType + "\n";

            //申请金额
            string applyAmountTitle = Json["data"]["formInfo"]["widgetMap"]["Mo_1"]["title"].ToString();
            consoleString += applyAmountTitle + ": ";
            string applyAmount = Json["data"]["formInfo"]["widgetMap"]["Mo_1"]["value"].ToString();
            consoleString += applyAmount + "\n";


            //明细
            string detailTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["title"].ToString();
            consoleString += detailTitle + "\n";
            string widgetTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetVos"]["Te_0"]["title"].ToString();//用酒名称
            consoleString += widgetTitle + "\t";
            string couuntTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetVos"]["Nu_1"]["title"].ToString();//用酒数量
            consoleString += couuntTitle + "\t";
            string feeTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetVos"]["Mo_0"]["title"].ToString();//费用金额(元)
            consoleString += feeTitle + "\n";

            List<WidgetValue> widgetValues = GetWidgetValue(Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetValue"].ToString());
            int totalCount = 0;
            decimal totalFee = 0m;
            widgetValues.ForEach(w =>
            {
                consoleString += w.Te_0 + "\t" + w.Nu_1 + "\t" + w.Mo_0 + "\n";
                totalCount += int.Parse(w.Nu_1);
                totalFee += decimal.Parse(w.Mo_0);

            });
            string totalFeeTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetVos"]["Mo_0"]["detailCountName"].ToString();
            string totalCouuntTitle = Json["data"]["formInfo"]["detailMap"]["Dd_0"]["widgetVos"]["Nu_1"]["detailCountName"].ToString();

            consoleString += totalFeeTitle + ": " + totalFee +"\t" + totalCouuntTitle + ": " + totalCount;
            return consoleString;
        }
        private static string JsonStr(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            String data = sr.ReadToEnd();
            data = data.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);//去除空格
            return data;
        }
        /// <summary>
        /// 获取市场区域
        /// </summary>
        /// <param name="marketStr"></param>
        /// <returns></returns>
        private static List<MarketArea> GetMarket(string marketStr)
        {
            List<MarketArea> markets = new List<MarketArea>();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            markets = Serializer.Deserialize<List<MarketArea>>(marketStr);
            return markets;
        }
        /// <summary>
        /// 获取执行人
        /// </summary>
        /// <param name="personStr"></param>
        /// <returns></returns>
        private static List<PersonInfo> GetPerson(string personStr)
        {
            List<PersonInfo> persons = new List<PersonInfo>();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            persons = Serializer.Deserialize<List<PersonInfo>>(personStr);
            return persons;
        }
        /// <summary>
        /// 获取费用类型
        /// </summary>
        /// <param name="feeTypeStr"></param>
        /// <returns></returns>
        private static List<FeeType> GetFeeType(string feeTypeStr)
        {
            List<FeeType> feeTypes = new List<FeeType>();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            feeTypes = Serializer.Deserialize<List<FeeType>>(feeTypeStr);
            return feeTypes;
        }
        /// <summary>
        /// 获取用酒
        /// </summary>
        /// <param name="widgetValueStr"></param>
        /// <returns></returns>
        private static List<WidgetValue> GetWidgetValue(string widgetValueStr)
        {
            List<WidgetValue> widgetValues = new List<WidgetValue>();
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            widgetValues = Serializer.Deserialize<List<WidgetValue>>(widgetValueStr);
            return widgetValues;
        }

    }
}

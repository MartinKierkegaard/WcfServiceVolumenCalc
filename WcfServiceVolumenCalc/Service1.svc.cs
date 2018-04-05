using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceVolumenCalc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        //Husk at ændre i string så den peger på den rigtige database server, husk a indsætte user+password
        private const string ConnectionString =
          "Server=tcp:easj2016100.database.windows.net,1433;Initial Catalog=HotelDB;Persist Security Info=False;User ID='';Password='';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";



        public double GetVolumeDB(double length, double width, double height)
        {

            double volumeResult = length * width * height;

            try
            {
                //kalder metoden som indsætter data i Azure databasen
                this.InsertData("GetVolume", volumeResult, length, width, height);
            }
            catch (Exception e)
            {
                //burde skrives til en log fx. event loggen
                Console.WriteLine("Der skete en fejl i indsættelse af data, fejl: " + e.Message );
            }

            return volumResulte;
        }




        /// <summary>
        /// metode til at indsætte data i database på Azure 
        /// </summary>
        /// <param name="request">hvilken request er der tale om</param>
        /// <param name="volume">den udregnede volumen</param>
        /// <param name="length">længden </param>
        /// <param name="width">bredden</param>
        /// <param name="height">højden</param>
        /// <returns>antal rækker der bliver berørt af sql'en (rowsaffected)</returns>
        public int InsertData(string request, double volume, double length, double width, double height)
        {
            const string sqlstring = "insert into volumen1 (request, volume, length, width, height) " +
                "values (@request, @volume, @length, @width, @height) ";

            //Husk at have connectionstring rigtig
            using (var DBconnection = new SqlConnection(ConnectionString))
            {
                DBconnection.Open();
                using (var sqlcommand = new SqlCommand(sqlstring, DBconnection))
                {
                    sqlcommand.Parameters.AddWithValue("@request", request);
                    sqlcommand.Parameters.AddWithValue("@volume", volume);
                    sqlcommand.Parameters.AddWithValue("@length", length);
                    sqlcommand.Parameters.AddWithValue("@width", width);
                    sqlcommand.Parameters.AddWithValue("@height", height);
                    var rowsaffected = sqlcommand.ExecuteNonQuery();
                    return rowsaffected;
                }
            }
        }

    }
}

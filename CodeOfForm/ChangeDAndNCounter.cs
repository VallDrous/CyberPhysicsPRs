using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace PR2CP
{
    public class ChangeDAndNCounter
    {
        MySqlConnection con = new MySqlConnection("SERVER=LOCALHOST ;DATABASE=energymeter ;UID=root ;PASSWORD = 1234554321Sss");
        public void ChangeDAndNCounterFalseDN(string nameCount, double dKwh, double nKwh)
        {
            con.Open();
            MySqlCommand addC = new MySqlCommand($"Insert into electricity_counters_new (counter_name, consumption_day_kwh, consumption_night_kwh, record_date)  values('{nameCount.ToLower()}', {dKwh + 100}, {nKwh + 80}, '{DateTime.Now.ToString("yyyy-MM-dd")}') ", con);
            addC.ExecuteNonQuery();
            con.Close();
        }
        public void ChangeDAndNCounterFalseD(string nameCount, double dKwh, double nKwh)
        {
            con.Open();
            MySqlCommand addC = new MySqlCommand($"Insert into electricity_counters_new (counter_name, consumption_day_kwh, consumption_night_kwh, record_date)  values('{nameCount.ToLower()}', {dKwh + 100}, {nKwh}, '{DateTime.Now.ToString("yyyy-MM-dd")}') ", con);
            addC.ExecuteNonQuery();
            con.Close();
        }
        public void ChangeDAndNCounterFalseN(string nameCount, double dKwh, double nKwh)
        {
            con.Open();
            MySqlCommand addC = new MySqlCommand($"Insert into electricity_counters_new (counter_name, consumption_day_kwh, consumption_night_kwh, record_date)  values('{nameCount.ToLower()}', {dKwh}, {nKwh + 80}, '{DateTime.Now.ToString("yyyy-MM-dd")}') ", con);
            addC.ExecuteNonQuery();
            con.Close();
        }
        public void ChangeDAndNCounterTrueDN(string nameCount, double dKwh, double nKwh)
        {
            con.Open();
            MySqlCommand addC = new MySqlCommand($"Insert into electricity_counters_new (counter_name, consumption_day_kwh, consumption_night_kwh, record_date)  values('{nameCount.ToLower()}', {dKwh}, {nKwh}, '{DateTime.Now.ToString("yyyy-MM-dd")}') ", con);
            addC.ExecuteNonQuery();
            con.Close();
        }
        public void SaveNewCounterToOld(string nameCount)
        {
            con.Open();
            MySqlCommand ToHistoryC = new MySqlCommand($"Insert into electricity_counters_old(counter_name, consumption_day_kwh, consumption_night_kwh, record_date)" +
                $" select counter_name, consumption_day_kwh, consumption_night_kwh, record_date" +
                $" From electricity_counters_new where counter_name = '{nameCount}'", con);
            ToHistoryC.ExecuteNonQuery();
            MySqlCommand delC = new MySqlCommand($"Delete from electricity_counters_new where counter_name = '{nameCount}'", con);
            delC.ExecuteNonQuery();
            con.Close();
        }
        public void SaveOldCounterToHistory(string nameCount)
        {
            con.Open();
            MySqlCommand ToHistoryC = new MySqlCommand($"Insert into electricity_counters_history(counter_name, consumption_day_kwh, consumption_night_kwh, record_date)" +
                $" select counter_name, consumption_day_kwh, consumption_night_kwh, record_date" +
                $" From electricity_counters_old where counter_name = '{nameCount}'", con);
            ToHistoryC.ExecuteNonQuery();
            MySqlCommand delC = new MySqlCommand($"Delete from electricity_counters_old where counter_name = '{nameCount}'", con);
            delC.ExecuteNonQuery();
            con.Close();
        }
    }
}

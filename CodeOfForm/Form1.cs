using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace PR2CP
{
    public partial class Form1 : Form
    {
        const double DAYCOST = 4.32;
        const double NIGHTCOST = 1.73;
        MySqlConnection con = new MySqlConnection("SERVER=LOCALHOST ;DATABASE=energymeter ;UID=root ;PASSWORD = 1234554321Sss");
        ChangeDAndNCounter counterC = new ChangeDAndNCounter();
        public Form1()
        {
            InitializeComponent();
        }

        private void addInform_Click(object sender, EventArgs e)
        {
            double dKwh = CheckOnDouble(tBDayKwh.Text);
            double nKwh = CheckOnDouble(tBNightKwh.Text);
            double[] dnKwhOld = getNumbersFromBD(tBName.Text.ToLower());
            if ((dKwh != -1 && nKwh != -1) && (dKwh >= 0 && nKwh >= 0))
            {
                try
                {
                    if (dnKwhOld != null)
                    {
                        if (dnKwhOld[0] > dKwh && dnKwhOld[1] > nKwh)
                        {
                            if (MessageBox.Show("Два показника занижені ви впевнені, що хочете продовжити?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                counterC.SaveOldCounterToHistory(tBName.Text.ToLower());
                                counterC.SaveNewCounterToOld(tBName.Text.ToLower());
                                counterC.ChangeDAndNCounterFalseDN(tBName.Text.ToLower(), dKwh, nKwh);
                                MessageBox.Show("Інформація успішно додана");
                            }
                        }
                        else if(dnKwhOld[0] > dKwh)
                        {
                            if (MessageBox.Show("Дані про день занижені ви впевнені, що хочете продовжити?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                counterC.SaveOldCounterToHistory(tBName.Text.ToLower());
                                counterC.SaveNewCounterToOld(tBName.Text.ToLower());
                                counterC.ChangeDAndNCounterFalseD(tBName.Text.ToLower(), dKwh, nKwh);
                                MessageBox.Show("Інформація успішно додана");
                            }
                        }
                        else if(dnKwhOld[1] > nKwh)
                        {
                            if (MessageBox.Show("Дані про ніч занижені ви впевнені, що хочете продовжити?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                counterC.SaveOldCounterToHistory(tBName.Text.ToLower());
                                counterC.SaveNewCounterToOld(tBName.Text.ToLower());
                                counterC.ChangeDAndNCounterFalseN(tBName.Text.ToLower(), dKwh, nKwh);
                                MessageBox.Show("Інформація успішно додана");
                            }
                        }
                        else
                        {
                            counterC.SaveOldCounterToHistory(tBName.Text.ToLower());
                            counterC.SaveNewCounterToOld(tBName.Text.ToLower());
                            counterC.ChangeDAndNCounterTrueDN(tBName.Text.ToLower(), dKwh, nKwh);
                            MessageBox.Show("Інформація успішно додана");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Щось пішло не так");
                }
            }
        }

        private double CheckOnDouble(string s)
        {
            try
            {
                double d = Double.Parse(s);
                return d;
            }
            catch
            {
                return -1;
            }
        }

        public double[] getNumbersFromBD(string name)
        {
            string value = string.Empty;
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter($"select consumption_day_kwh, consumption_night_kwh from electricity_counters_new" +
                                                           $" where counter_name = '{name}'", con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                string a = ds.Tables[0].Rows.ToString();
                dataGridView1.DataSource = ds.Tables[0];
                value = dataGridView1.Rows[0].Cells[0].Value?.ToString() ?? "0";
                double dKwh = double.Parse(value);
                value = dataGridView1.Rows[0].Cells[1].Value?.ToString() ?? "0";
                double nKwh = double.Parse(value);
                dataGridView1.DataSource = null;
                return new[] { dKwh, nKwh };
            }
            catch
            {
                return null;
            }
        }

        private void bPrice_Click(object sender, EventArgs e)
        {
            MySqlDataAdapter da = new MySqlDataAdapter($"select new.counter_name as 'назва лічильника',{DAYCOST}*(new.consumption_day_kwh - COALESCE(old.consumption_day_kwh,0)) as 'ціна за день', {NIGHTCOST}*(new.consumption_night_kwh - COALESCE(old.consumption_night_kwh,0)) as 'ціна за ніч' from electricity_counters_new as new left join electricity_counters_old as old ON new.counter_name = old.counter_name", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];  
        }

        private void getDataQueue_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            List<string> names = getNamesOfCounter();
            Queue<dataForCounter> queueOfInf = new Queue<dataForCounter>();
            int count = rnd.Next(3,6);
            for(int i = 0; i < count; i++)
            {
                int nameI = rnd.Next(names.Count());
                double dayC = Math.Round(50 + rnd.NextDouble() * 100, 2);
                double nightC = Math.Round(50 + rnd.NextDouble() * 100,2);
                queueOfInf.Enqueue(new dataForCounter(names[nameI],dayC,nightC));
            }
            foreach(var inf in queueOfInf)
            {
                double dKwh = inf.dCount;
                double nKwh = inf.nCount;
                double[] dnKwhOld = getNumbersFromBD(inf.name.ToLower());
                try
                {
                    if (dnKwhOld != null)
                    {
                        counterC.SaveOldCounterToHistory(inf.name.ToLower());
                        counterC.SaveNewCounterToOld(inf.name.ToLower());
                        if (dnKwhOld[0] > dKwh && dnKwhOld[1] > nKwh)
                        {
                            counterC.ChangeDAndNCounterFalseDN(inf.name.ToLower(), dKwh, nKwh);
                        }
                        else if (dnKwhOld[0] > dKwh)
                        {
                            counterC.ChangeDAndNCounterFalseD(inf.name.ToLower(), dKwh, nKwh);
                        }
                        else if (dnKwhOld[1] > nKwh)
                        {
                            counterC.ChangeDAndNCounterFalseN(inf.name.ToLower(), dKwh, nKwh);
                        }
                        else
                        {
                            counterC.ChangeDAndNCounterTrueDN(inf.name.ToLower(), dKwh, nKwh);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Щось пішло не так");
                }
            }
        }

        private List<string> getNamesOfCounter()
        {
            List<string> infName = new List<string>();
            MySqlDataAdapter da = new MySqlDataAdapter("select counter_name from electricity_counters_new",con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            foreach (DataRow r in dt.Rows)
            {
                var value = r[0];
                infName.Add((string)value);
            }
            return infName;
        }
    }
}

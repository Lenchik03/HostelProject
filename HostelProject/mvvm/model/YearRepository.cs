using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostelProject.mvvm.model
{
    public class YearRepository
    {
        static YearRepository instance;
        public static YearRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new YearRepository();
                return instance;
            }
        }

        internal IEnumerable<Year> GetAllYears()
        {
            var result = new List<Year>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT * FROM years";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("year_id");
                    var year = new Year();

                    year.ID = id;
                    year.Title = reader.GetString("year");
                    

                    result.Add(year);
                }
            }
            return result;
        }


        internal void AddYear(Year year)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("years");

                string sql = "INSERT INTO years VALUES (0, @year)";
                using (var mc = new MySqlCommand(sql, connect)) 
                {
                    mc.Parameters.Add(new MySqlParameter("year", year.Title));
                   
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace КП_БД
{
    internal class Database
    {
        private SqlDataAdapter adapter;
        private SqlCommand cmd;
        public Database()
        {

        }
        private string connectionString = @"Data Source = HOME-PC\SQLEXPRESS; Initial Catalog = SOKOLOVA;Integrated Security = True;";
        public void DisplayPeopleData(DataGridView dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select * from ЧЕЛОВЕК", connectionString);
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DisplayPeopleDataAndPhoneData(DataGridView dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ОТЧЕСТВО, НОМЕР_ТЕЛЕФОНА.НОМЕР_ТЕЛЕФОНА, ТЕЛЕФОН.ТИП_ТЕЛЕФОНА FROM ЧЕЛОВЕК INNER JOIN НОМЕР_ТЕЛЕФОНА ON ЧЕЛОВЕК.НОМЕР_id = НОМЕР_ТЕЛЕФОНА.id_i INNER JOIN ТЕЛЕФОН ON ТЕЛЕФОН.id_t = НОМЕР_ТЕЛЕФОНА.id_i  ", connectionString); /// дописать джоин он по адресу и еще 1 по 
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DisplayPeopleDataAndAdressData(DataGridView dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ОТЧЕСТВО, АДРЕС.ГОРОД, АДРЕС.ДОМ, АДРЕС.ИНДЕКС, АДРЕС.КВАРТИРА, АДРЕС.УЛИЦА   " +
                    "FROM ЧЕЛОВЕК INNER JOIN АДРЕС ON ЧЕЛОВЕК.АДРЕС_ID = АДРЕС.id_h", connectionString); /// дописать джоин он по адресу и еще 1 п
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DisplayPeopleInformAndWorkData(DataGridView dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ОТЧЕСТВО, ЧЕЛОВЕК.ДАТА_РОЖДЕНИЯ, ДОЛЖНОСТЬ.ДОЛЖНОСТЬ, " +
                    "МЕСТО_РАБОТЫ.МЕСТО_РАБОТЫ, СТАТУС.СТАТУС FROM ЧЕЛОВЕК INNER JOIN ДОЛЖНОСТЬ ON ЧЕЛОВЕК.ДОЛЖНОСТЬ_id = ДОЛЖНОСТЬ.id_d " +
                    "INNER JOIN  МЕСТО_РАБОТЫ ON  ЧЕЛОВЕК.МЕСТО_РАБОТЫ_id = МЕСТО_РАБОТЫ.id_m " +
                    "INNER JOIN СТАТУС ON  ЧЕЛОВЕК.СТАТУС_id = СТАТУС.id_c " +
                    "Order by ЧЕЛОВЕК.ФАМИЛИЯ ", connectionString); /// дописать джоин он по адресу и еще 1 по 
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DisplayPeopleInformAndWorkDataWithConditionHaving(DataGridView dataGrid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                DateTime date = new DateTime();
                date = DateTime.Parse("2002.01.01");
                adapter = new SqlDataAdapter("SELECT ФАМИЛИЯ, COUNT(ФАМИЛИЯ) AS КОЛИЧЕСТВО FROM ЧЕЛОВЕК GROUP BY ФАМИЛИЯ HAVING ФАМИЛИЯ = 'Астахов' ", connectionString);
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public List<string> getTypesOfWorkPlaces()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int n = 5;
                List<string> s = new List< string>();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select * from МЕСТО_РАБОТЫ", connectionString);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    s.Add(Convert.ToString(dr["МЕСТО_РАБОТЫ"]) );
                }
                connection.Close();
                return s;
            }
        }
        public List<string> getTypesOfJobTitle()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int n = 5;
                List<string> s = new List<string>();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select * from ДОЛЖНОСТЬ", connectionString);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    s.Add(Convert.ToString(dr["ДОЛЖНОСТЬ"]));
                }
                connection.Close();
                return s;
            }
        }
        public List<string> getTypesOfStatus()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int n = 3;
                List<string> s = new List<string>();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select * from СТАТУС", connectionString);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    s.Add(Convert.ToString(dr["СТАТУС"]));
                }
                connection.Close();
                return s;
            }
        }
        public List<string> getTypesOfPhone()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int n = 5;
                List<string> s = new List<string>();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select * from ТЕЛЕФОН", connectionString);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    s.Add(Convert.ToString(dr["ТИП_ТЕЛЕФОНА"]));
                }
                connection.Close();
                return s;
            }
        }
        public void AddHuman(string sername, string first_name,  string patronymic, string sex, int mrID, int dolgID, int numTelID, int statusID, int adressID, DateTime date, DataGridView dataGrid)
        {
            if (first_name != "" && sername != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(
                        "insert into ЧЕЛОВЕК (ФАМИЛИЯ, ИМЯ, ОТЧЕСТВО, ПОЛ, ДАТА_РОЖДЕНИЯ, МЕСТО_РАБОТЫ_id, ДОЛЖНОСТЬ_ID, НОМЕР_ID, СТАТУС_ID, АДРЕС_ID) values(@ФАМИЛИЯ, @ИМЯ, @ОТЧЕСТВО, @ПОЛ,  @ДАТА_РОЖДЕНИЯ, @МЕСТО_РАБОТЫ_id, @ДОЛЖНОСТЬ_ID, @НОМЕР_ID, @СТАТУС_ID, @АДРЕС_ID)", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@ФАМИЛИЯ", sername);
                    cmd.Parameters.AddWithValue("@ИМЯ", first_name);
                    cmd.Parameters.AddWithValue("@ОТЧЕСТВО", patronymic);
                    cmd.Parameters.AddWithValue("@ПОЛ", sex);
                    cmd.Parameters.AddWithValue("@ДАТА_РОЖДЕНИЯ", date);
                    cmd.Parameters.AddWithValue("@МЕСТО_РАБОТЫ_id", mrID);
                    cmd.Parameters.AddWithValue("@ДОЛЖНОСТЬ_ID", dolgID);
                    cmd.Parameters.AddWithValue("@НОМЕР_ID", numTelID);
                    cmd.Parameters.AddWithValue("@СТАТУС_ID", statusID);
                    cmd.Parameters.AddWithValue("@АДРЕС_ID", adressID);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Добавлено");
                    DisplayPeopleData(dataGrid);

                }
            }
            else
            {
                MessageBox.Show("Введите данные");
            }
        } 
        public void UpdateHuman(string sername, string first_name,  string patronymic, string sex, int mrID, int dolgID, int numTelID, int statusID, int adressID, DateTime date, int idHuman)
        {
            if (first_name != "" && sername != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(
                        "update ЧЕЛОВЕК SET ФАМИЛИЯ = @ФАМИЛИЯ, ИМЯ = @ИМЯ, ОТЧЕСТВО = @ОТЧЕСТВО, ПОЛ = @ПОЛ, ДАТА_РОЖДЕНИЯ =@ДАТА_РОЖДЕНИЯ, МЕСТО_РАБОТЫ_id = @МЕСТО_РАБОТЫ_id, ДОЛЖНОСТЬ_ID = @ДОЛЖНОСТЬ_ID, НОМЕР_ID = @НОМЕР_ID, СТАТУС_ID = @СТАТУС_ID, АДРЕС_ID = @АДРЕС_ID where id_p = @id", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@ФАМИЛИЯ", sername);
                    cmd.Parameters.AddWithValue("@ИМЯ", first_name);
                    cmd.Parameters.AddWithValue("@ОТЧЕСТВО", patronymic);
                    cmd.Parameters.AddWithValue("@ПОЛ", sex);
                    cmd.Parameters.AddWithValue("@ДАТА_РОЖДЕНИЯ", date);
                    cmd.Parameters.AddWithValue("@МЕСТО_РАБОТЫ_id", mrID);
                    cmd.Parameters.AddWithValue("@ДОЛЖНОСТЬ_ID", dolgID);
                    cmd.Parameters.AddWithValue("@НОМЕР_ID", numTelID);
                    cmd.Parameters.AddWithValue("@СТАТУС_ID", statusID);
                    cmd.Parameters.AddWithValue("@АДРЕС_ID", adressID);
                    cmd.Parameters.AddWithValue("@id", idHuman);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Изменено");
                    //DisplayPeopleData(dataGrid);

                }
            }
            else
            {
                MessageBox.Show("Введите данные");
            }
        }
        public List<string> GetHuman(int id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> list = new List<string>();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("select ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ОТЧЕСТВО, ЧЕЛОВЕК.ДАТА_РОЖДЕНИЯ, ЧЕЛОВЕК.ПОЛ, ДОЛЖНОСТЬ.ДОЛЖНОСТЬ, НОМЕР_ТЕЛЕФОНА.НОМЕР_ТЕЛЕФОНА, " +
                "МЕСТО_РАБОТЫ.МЕСТО_РАБОТЫ, АДРЕС.ГОРОД, АДРЕС.ИНДЕКС, АДРЕС.УЛИЦА, АДРЕС.ДОМ, АДРЕС.КВАРТИРА, СТАТУС.СТАТУС, ТЕЛЕФОН.ТИП_ТЕЛЕФОНА " +
                "from ЧЕЛОВЕК " +
                "INNER JOIN ДОЛЖНОСТЬ ON ЧЕЛОВЕК.ДОЛЖНОСТЬ_id = ДОЛЖНОСТЬ.id_d " +
                "INNER JOIN  МЕСТО_РАБОТЫ ON ЧЕЛОВЕК.МЕСТО_РАБОТЫ_id = МЕСТО_РАБОТЫ.id_m " +
                "INNER JOIN АДРЕС ON ЧЕЛОВЕК.АДРЕС_id = АДРЕС.id_h " +
                "INNER JOIN СТАТУС ON ЧЕЛОВЕК.СТАТУС_id = СТАТУС.id_c " +
                "INNER JOIN НОМЕР_ТЕЛЕФОНА ON ЧЕЛОВЕК.НОМЕР_id = НОМЕР_ТЕЛЕФОНА.id_i " +
                "INNER JOIN ТЕЛЕФОН ON НОМЕР_ТЕЛЕФОНА.id_i = ТЕЛЕФОН.id_t " +
                "AND ЧЕЛОВЕК.id_p = " + id, connection);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(Convert.ToString(dr["ФАМИЛИЯ"]));
                    list.Add(Convert.ToString(dr["ИМЯ"]));
                    list.Add(Convert.ToString(dr["ОТЧЕСТВО"]));
                    list.Add(Convert.ToString(dr["ПОЛ"]));
                    list.Add(Convert.ToString(dr["ДАТА_РОЖДЕНИЯ"]));
                    list.Add(Convert.ToString(dr["ДОЛЖНОСТЬ"]));
                    list.Add(Convert.ToString(dr["МЕСТО_РАБОТЫ"]));

                    list.Add(Convert.ToString(dr["ГОРОД"]));
                    list.Add(Convert.ToString(dr["ДОМ"]));
                    list.Add(Convert.ToString(dr["УЛИЦА"]));
                    list.Add(Convert.ToString(dr["ИНДЕКС"]));
                    list.Add(Convert.ToString(dr["КВАРТИРА"]));
                    list.Add(Convert.ToString(dr["СТАТУС"]));
                    list.Add(Convert.ToString(dr["НОМЕР_ТЕЛЕФОНА"]));
                    list.Add(Convert.ToString(dr["ТИП_ТЕЛЕФОНА"]));
                }
                connection.Close();
                return list;

            }
        }
        public List<string> GetHumanByName(string name)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<string> list = new List<string>();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select * from ЧЕЛОВЕК where ИМЯ = '"+name+" '", connection);
                cmd.Parameters.AddWithValue("@id", name);
                cmd.ExecuteNonQuery();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(Convert.ToString(dr["ФАМИЛИЯ"]));
                    list.Add(Convert.ToString(dr["ИМЯ"]));
                    list.Add(Convert.ToString(dr["ОТЧЕСТВО"]));
                    list.Add(Convert.ToString(dr["ПОЛ"]));
                    list.Add(Convert.ToString(dr["ДАТА_РОЖДЕНИЯ"]));
                }
                connection.Close();
                return list;

            }
        }
        public int getAdressId(string sity, int home, string street, int index, int flat)
        {
            if (sity != "" && street != "" )
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(
                       "select id_h from АДРЕС where ГОРОД = '"+ sity + "' and ИНДЕКС = " + index + " and УЛИЦА = '" + street + "' and ДОМ = " + home + " and КВАРТИРА = " + flat, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        connection.Close();
                        return (Convert.ToInt32(dr["id_h"]));
                    }
                    return -1;
                 
                }
            }
            else
            {
                MessageBox.Show("Введите данные");
                return -1;
            }
        }
        public int getIdByNumber(string number)
        {
            if (number != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(
                       "select id_i from НОМЕР_ТЕЛЕФОНА where НОМЕР_ТЕЛЕФОНА = '" + number + "'", connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        connection.Close();
                        return (Convert.ToInt32(dr["id_i"]));
                    }
                    return -1;
                 
                }
            }
            else
            {
                MessageBox.Show("Введите данные");
                return -1;
            }
        }
        public void AddAdress(string sity, int home, string street, int index, int flat)
        {
            if (sity != "" && street != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(
                        "insert into АДРЕС (ГОРОД,ИНДЕКС,УЛИЦА,ДОМ,КВАРТИРА) values(@ГОРОД,@ИНДЕКС,@УЛИЦА,@ДОМ,@КВАРТИРА)", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@ГОРОД", sity);
                    cmd.Parameters.AddWithValue("@ИНДЕКС", index);
                    cmd.Parameters.AddWithValue("@УЛИЦА", street);
                    cmd.Parameters.AddWithValue("@ДОМ", home);
                    cmd.Parameters.AddWithValue("@КВАРТИРА", flat);
                    cmd.ExecuteNonQuery();
            
                    connection.Close();

                }
            }
            else
            {
                MessageBox.Show("Введите данные");
            }
        }
        public void AddNumber(string number)
        {
            if (number != "")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    cmd = new SqlCommand(
                        "insert into НОМЕР_ТЕЛЕФОНА (НОМЕР_ТЕЛЕФОНА) values(@НОМЕР_ТЕЛЕФОНА)", connection);
                    connection.Open();
                    cmd.Parameters.AddWithValue("@НОМЕР_ТЕЛЕФОНА", number);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                }
            }
            else
            {
                MessageBox.Show("Введите данные");
            }
        }
        public void DeleteAdressById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd = new SqlCommand(
                    "delete from АДРЕС where id_h = @id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
        }
        public void DeletePhoneNumberById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd = new SqlCommand(
                    "delete from НОМЕР_ТЕЛЕФОНА where id_i = @id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
        }
        public int getAdressIdByHumanId(int humanId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(
                   "select * from ЧЕЛОВЕК where id_p = "+ humanId, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    connection.Close();
                    return (Convert.ToInt32(dr["АДРЕС_ID"]));
                }
                return -1;

            }
        }
        public int getPhoneNumberIdByHumanId(int humanId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(
                   "select * from ЧЕЛОВЕК where id_p = "+ humanId, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    connection.Close();
                    return (Convert.ToInt32(dr["НОМЕР_ID"]));
                }
                return -1;

            }
        }
        public void DisplayPeopleInformAndWorkDataWithName(DataGridView dataGrid, string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ОТЧЕСТВО, ЧЕЛОВЕК.ДАТА_РОЖДЕНИЯ, ДОЛЖНОСТЬ.ДОЛЖНОСТЬ, " +
                    "МЕСТО_РАБОТЫ.МЕСТО_РАБОТЫ, СТАТУС.СТАТУС FROM ЧЕЛОВЕК INNER JOIN ДОЛЖНОСТЬ ON ЧЕЛОВЕК.ДОЛЖНОСТЬ_id = ДОЛЖНОСТЬ.id_d " +
                    "INNER JOIN  МЕСТО_РАБОТЫ ON  ЧЕЛОВЕК.МЕСТО_РАБОТЫ_id = МЕСТО_РАБОТЫ.id_m " +
                    "INNER JOIN СТАТУС ON  ЧЕЛОВЕК.СТАТУС_id = СТАТУС.id_c AND ЧЕЛОВЕК.ИМЯ = '" +name+"'", connectionString); /// дописать джоин он по адресу и еще 1 по 
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DisplayPeopleInformAndWorkDataWithSername(DataGridView dataGrid, string sername)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT ЧЕЛОВЕК.ИМЯ, ЧЕЛОВЕК.ФАМИЛИЯ, ЧЕЛОВЕК.ОТЧЕСТВО, ЧЕЛОВЕК.ДАТА_РОЖДЕНИЯ, ДОЛЖНОСТЬ.ДОЛЖНОСТЬ, " +
                    "МЕСТО_РАБОТЫ.МЕСТО_РАБОТЫ, СТАТУС.СТАТУС FROM ЧЕЛОВЕК INNER JOIN ДОЛЖНОСТЬ ON ЧЕЛОВЕК.ДОЛЖНОСТЬ_id = ДОЛЖНОСТЬ.id_d " +
                    "INNER JOIN  МЕСТО_РАБОТЫ ON  ЧЕЛОВЕК.МЕСТО_РАБОТЫ_id = МЕСТО_РАБОТЫ.id_m " +
                    "INNER JOIN СТАТУС ON  ЧЕЛОВЕК.СТАТУС_id = СТАТУС.id_c AND ЧЕЛОВЕК.ФАМИЛИЯ = '" + sername+"'", connectionString); /// дописать джоин он по адресу и еще 1 по 
                adapter.Fill(dt);
                dataGrid.DataSource = dt;
                connection.Close();
            }
        }
        public void DeleteHumanByID(int id, DataGridView dataGrid)
        {
            int adressId= getAdressIdByHumanId(id);
            int phoneNumberId= getPhoneNumberIdByHumanId(id);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd = new SqlCommand(
                    "delete from ЧЕЛОВЕК where id_p = @id", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            DeleteAdressById(adressId);
            DeletePhoneNumberById(phoneNumberId);
            DisplayPeopleData(dataGrid);
        }
        public void ExportExcel(DataGridView dataGrid)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            // создаем новый WorkBook
            Microsoft.Office.Interop.Excel._Workbook workbook =
            app.Workbooks.Add(Type.Missing);
            // новый Excelsheet в workbook
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // задаем имя для worksheet
            worksheet.Name = "Exported from gridview";
            for (int i = 1; i < dataGrid.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGrid.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] =
                    dataGrid.Rows[i].Cells[j].Value.ToString();
                }
            }
        }
        public void ExportWord(DataGridView DGV)
        {
            if (DGV.Rows.Count != 0)
            {
                int RowCount = DGV.Rows.Count;
                int ColumnCount = DGV.Columns.Count;
                Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];
                //добавим поля и колонки
                int r = 0;
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    for (r = 0; r <= RowCount - 1; r++)
                    {
                        DataArray[r, c] = DGV.Rows[r].Cells[c].Value;
                    }
                }
                Microsoft.Office.Interop.Word.Document oDoc = new
                Microsoft.Office.Interop.Word.Document();
                oDoc.Application.Visible = true;
                //страницы
                oDoc.PageSetup.Orientation =
                Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
                dynamic oRange = oDoc.Content.Application.Selection.Range;
                string oTemp = "";
                for (r = 0; r <= RowCount - 1; r++)
                {
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        oTemp = oTemp + DataArray[r, c] + "\t";
                    }
                }
                //формат таблиц
                oRange.Text = oTemp;
                object Separator =
                Microsoft.Office.Interop.Word.WdTableFieldSeparator.wdSeparateByTabs;
                object ApplyBorders = true;
                object AutoFit = true;
                object AutoFitBehavior =
                Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitContent;
                oRange.ConvertToTable(ref Separator, ref RowCount, ref ColumnCount,
                Type.Missing, Type.Missing, ref ApplyBorders,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, ref AutoFit, ref AutoFitBehavior,
                oRange.Select());
            }
        }
    }
}


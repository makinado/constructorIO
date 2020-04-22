﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2.DatabaseAbstractionLayer
{
    public class PostgreSQLPersistence
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;

        private int ultimoCod;

        public PostgreSQLPersistence()
        {
            string strConn="Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=postgres123;";
            
            conn = new NpgsqlConnection(strConn);
            cmd = conn.CreateCommand();
        }

        public void abrir()
        {
            if(conn.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch(Exception)
                {
                    MessageBox.Show("Banco de dados Offline.");
                }
            }
        }


        public void fechar()
        {
            conn.Close();
            cmd.Parameters.Clear();
        }

        public DataTable executeSelect(string select, Dictionary<string, object> param = null)
        {
            abrir();
            cmd.CommandText = select;
            DataTable dt = new DataTable();

            if(param != null)
            {
                foreach(var p in param)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
            }

            try
            {
                dt.Load(cmd.ExecuteReader());
            }
            catch(Exception)
            {

            }

            fechar();

            return dt;
        }

        public bool executeNonQuery(string sql, Dictionary<string, object> param = null)
        {
            bool sucesso = false;
            //sql += " RETURNING COD;";

            abrir();

            cmd.CommandText = sql;

            if(param != null)
            {
                foreach(var p in param)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value == null ? DBNull.Value : p.Value);
                }
            }
            try
            {
                //qntdLinhas = cmd.ExecuteNonQuery();
            
                //get ultimo cod (serial) inserido;
                ultimoCod = (int) cmd.ExecuteScalar();
                sucesso = true;
            }
            catch(Exception)
            {
                ultimoCod = 0;
            }

            fechar();

            return sucesso;
        }

        public Dictionary<string, object> getParams()
        {
            return new Dictionary<string, object>();
        }

        public int getUltimoCod()
        {
            return this.ultimoCod;
        }
    }
}

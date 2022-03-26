using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyrros
{
    internal class Connect
    {
        private string connectionString;
        private SqlCommand cmd;
        private SqlConnection con;
        public string errmsg;

        public Connect(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string ConnectionString { get { return connectionString; } }

        public SqlConnection Connection
        {

            get
            {
                try
                {
                    con = new SqlConnection(connectionString);
                    con.Open();
                    return con;
                }catch (Exception ex)
                {
                    errmsg = ex.Message;
                    throw new Exception(ex.Message);
                }
                
            }
        }

        public void SetProcedure(string procedure)
        {
            try
            {
                using (Connection)
                {
                    cmd = new SqlCommand(procedure, Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                }
            }catch (Exception ex)
            {
                errmsg = ex.Message;
                throw new Exception(ex.Message);
            }
            
        }

        public void AddParam(string description, object param)
        {
            try
            {
                using (cmd)
                {
                    SqlParameter sqlparam = new SqlParameter(description, param);
                    cmd.Parameters.Add(sqlparam);
                }
            }catch(Exception ex)
            {
                errmsg = ex.Message;
                throw new Exception(ex.Message);
            }
            
        }

        public int Execute()
        {
            try
            {
                using (cmd)
                {
                    return cmd.ExecuteNonQuery();
                }
            }catch(Exception ex)
            {
                errmsg = ex.Message;
                throw new Exception(ex.Message);
            }
            
        }

        public DataSet Select()
        {
            try
            {
                using (cmd)
                {
                    DataSet ds = new DataSet();
                    DbDataAdapter adapter = DbProviderFactories.GetFactory(Connection).CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    return ds;
                    //return cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                throw new Exception(ex.Message);
            }

        }


    }
}

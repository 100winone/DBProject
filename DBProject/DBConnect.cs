using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProject
{
    class DBConnect
    {
        String connectStr = "Data Source = (DESCRIPTION = " + "(ADDRESS_LIST =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = 211.189.132.199)(PORT = 1521))" +
    ")" + "(CONNECT_DATA =" + "(SERVICE_NAME =" + Form1.dbsource + ")" + ")" + ");User Id = " + Form1.idstr + ";password=" + Form1.pwstr + ";";
        OracleConnection conn;
        OracleCommand cmd;
        OracleDataAdapter adapter;

        public DataTable GetTable(String sql)
        {
            conn = new OracleConnection(connectStr);
            conn.Open(); cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            adapter = new OracleDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet rs = new DataSet();
            adapter.Fill(rs);
            conn.Close(); // 첫번째 테이블 반환하고 끝난다. 
            return rs.Tables[0];

        }
    }
}

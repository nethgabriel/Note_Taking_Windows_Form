using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_Taking_Windows_Form
{
    public partial class Global_Connection
    {
        public static ADODB.Connection con = new ADODB.Connection();
        public static ADODB.Command cmd = new ADODB.Command();
        public static ADODB.Recordset rs = new ADODB.Recordset();

        public static string constr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={Application.StartupPath}\\Notes-DB.mdb";
    }
}

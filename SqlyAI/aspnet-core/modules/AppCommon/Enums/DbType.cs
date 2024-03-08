using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Db
{
    public enum DbType
    {
        SQLServer = 1,
        MySql = 2,
        PostgreSQL = 3,
        MariaDB = 4,
        Oracle = 5,
        SQLite = 6,
        CSV = 7,
        GoogleSheet = 8,



        Unknown = 100,

    }
}

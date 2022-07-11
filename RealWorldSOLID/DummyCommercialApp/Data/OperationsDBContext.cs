using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace DummyCommercialApp.Data
{
    public class OperationsDBContext : DbContext
    {
        internal static OperationsDBContext Factory(SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}

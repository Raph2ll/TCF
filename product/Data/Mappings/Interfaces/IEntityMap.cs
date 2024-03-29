using System;
using MySql.Data.MySqlClient;

namespace product.Data.Mappings.Interfaces
{
    public interface IEntityMap
    {
        void Configure(MySqlConnection connection);
    }
}
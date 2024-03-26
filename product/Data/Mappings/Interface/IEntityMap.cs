using System;
using MySql.Data.MySqlClient;

namespace api.Data.Mappings.Interfaces
{
    public interface IEntityMap
    {
        void Configure(MySqlConnection connection);
    }
}
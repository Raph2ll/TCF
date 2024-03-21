using System;
using MySql.Data.MySqlClient;

namespace api.Mappings.Interfaces
{
    public interface IEntityMap
    {
        void Configure(MySqlConnection connection);
    }
}

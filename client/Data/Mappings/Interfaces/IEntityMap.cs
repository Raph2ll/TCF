using System;
using MySql.Data.MySqlClient;

namespace client.Data.Mappings.Interfaces
{
    public interface IEntityMap
    {
        void Configure(MySqlConnection connection);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
                get
            {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
               }
            
        }

        public List<Walks> GetWalksByWalker(int walkerId)
        {

            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT *, Dog.Id DogId, Dog.Name DogName, Dog.OwnerId, Owner.Name OwnerName FROM Walks JOIN Dog on Dog.Id = Walks.DogId JOIN Owner on Dog.OwnerId = Owner.Id WHERE WalkerId = @id ORDER BY Owner.Name";

                        cmd.Parameters.AddWithValue("@id", walkerId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Walks> walks = new List<Walks>();

                            while (reader.Read())
                            {
                                Walks walk = new Walks
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                    WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                    DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                    Dog = new Dog
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                                        Name = reader.GetString(reader.GetOrdinal("DogName")),
                                        Breed = reader.GetString(reader.GetOrdinal("Breed"))
                                    },
                                    Owner = new Owner
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                        Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                                    }
                                };

                                walks.Add(walk);
                            }

                            return walks;
                        }
                    }
                }
            }
        }

    }
}

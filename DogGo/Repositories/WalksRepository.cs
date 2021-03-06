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

        public List<Walks> GetWalks()
        {

            {

                {
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();

                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT *, Dog.Id DogId, Dog.Name DogName, Dog.OwnerId, Owner.Name OwnerName FROM Walks JOIN Dog on Dog.Id = Walks.DogId JOIN Owner on Dog.OwnerId = Owner.Id ORDER BY Walks.Date";

                            

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

        public void AddWalk(Walks walk, int dogId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

               using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Walks (Date, Duration, WalkerId, DogId) OUTPUT INSERTED.ID VALUES(@date, @duration, @walkerId, @dogId)";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWalks(List<int> walks)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    List<string> paramNames = walks.Select((m, i) => $"@p{i}").ToList();
                    //This acheives the same result as what is commented below

                        /*new List<string>();*/

                    //for(int i = 0; i < walks.Count; i++)
                    //{
                    //    paramNames.Add($"@p{i}");
                    //}

                    cmd.CommandText = $"DELETE FROM Walks WHERE Id in ({String.Join(", ", paramNames)})";

                    for(int i = 0; i < walks.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(paramNames[i], walks[i]);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCTPaging.Data
{
    public class InitializeDB : IInitializeDB
    {
        private readonly ApplicationDBContext _db;
        public InitializeDB(ApplicationDBContext db)
        {
            _db = db;

        }
        public void InitializeDbWithData()
        {
            _db.Database.EnsureCreated();
            if (!_db.Employee.Any())
            {
                for (int i = 1; i <= 100; i++)
                {
                    _db.Employee.Add(new Model.Employee() { FirstName = $"FirstName {i}", LastName = $"LastName {i}", Email = $"FirstName {i}@gmail.com" });
                }

            }
            _db.SaveChanges();
        }
    }
}

using ShoeAccounting.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoeAccounting.Controllers
{
    public class UserController
    {
        static public User? LoginUser(string login, string password)
        {
            using (ShoesDbContext context = new ShoesDbContext())
            {
                User? user = context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);

                return user == null ? null : user;
            }
        }
    }
}

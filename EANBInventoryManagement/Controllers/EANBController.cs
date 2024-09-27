using EANBInventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;


namespace EANBInventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EANBController : Controller
    {
        public static string salt= "JK3i6kig";


        EanbinventoryManagementContext context = new EanbinventoryManagementContext();
        public class tempUser
        {
            public string username { get; set; }
            public string password { get; set; }
        }


        

        // Method to hash the password with the salt
        public static string HashPassword(string password, string salt)
        {
            // Append salt to the password
            string saltedPassword = password + salt;

            // Convert salted password to bytes
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);

            // Hash the bytes using SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);

                // Encode the hash bytes to Base64
                string hashBase64 = Convert.ToBase64String(hashBytes);

                // Combine salt and hash for storage in database
                return salt + hashBase64;
            }
        }

        [HttpGet("Events")]
        public IActionResult GetEvents()
        {
            try
            {
                var events = context.Events.ToList();
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(499, ex.Message); // Indicate failure due to an exception
            }

        }

        // GET: EANBController
        [HttpPost("Login")]
        public IActionResult Login(tempUser tempUser)
        {
            try
            {
                var users = tempUser;
                var username = tempUser.username;
                var password = tempUser.password;
                var user = context.Users.Where(x => x.Username == username).FirstOrDefault();
                if (user != null)
                {
                    var hashedPassword = HashPassword(password, salt);
                    if (string.Compare(hashedPassword,(user.PasswordHash)) == 0)
                    {
                        return Ok(user);

                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return NotFound();
            }

        }
    }
}

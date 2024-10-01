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
        public static string salt = "JK3i6kig";


        EanbinventoryManagementContext context = new EanbinventoryManagementContext();
        public class tempUser
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class acceptOffer
        {
            public int offerId { get; set; }
            public int requestedItemId { get; set; }
            public int userId { get; set; }

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

        [HttpGet("Events/{userId}")]
        public IActionResult GetEvents(int userId)
        {
            try
            {
                var events = context.Events
                    .Select(x => new
                    {
                        x.EventId,
                        x.UserId,
                        x.Name,
                        x.Location,
                        x.Start,
                        x.End,
                        Time = x.End - x.Start,
                        x.RequestedItems,
                       
                    })
                    .Where(x=>x.UserId == userId) // && x.End > DateTime.Now
                    .OrderBy(x => x.Start).ToList();
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(499, ex.Message); // Indicate failure due to an exception
            }

        }

        [HttpGet("Offers/{filter?}")]
        public IActionResult GetOffersForRequest(string filter = "")
        {
            try
            {
                var offers = context.Offers
                    .Where(o =>  (string.IsNullOrEmpty(filter) || o.Name.Contains(filter))) //o.RequestUserId == null
                    .ToList();
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Indicate failure due to an exception
            }
        }


        [HttpGet("Offers/Request/{requestedItemId}")]
        public IActionResult GetOffersForRequest(int requestedItemId)
        {
            try
            {
                var offers = context.Offers
                    .Where(o => o.RequestedItemId == requestedItemId) //o.RequestUserId == null
                    .Select(selector: o => new
                    {
                        o.OfferId,
                        o.Name,
                        o.Amount,
                        o.StartDate,
                        o.EndDate,
                        o.RequestUserId,
                        o.RequestedItemId,
                        o.State,
                        o.OfferUser.Username,
                    })
                    .FirstOrDefault();
                return Ok(offers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Indicate failure due to an exception
            }
        }


        [HttpPost("Offers/Reserve")]
        public IActionResult ReserveOffer(acceptOffer acceptOffer)
        {
            try
            {
                var requestItem = context.RequestedItems.FirstOrDefault(r => r.RequestedItemId == acceptOffer.requestedItemId);
                var offer = context.Offers.FirstOrDefault(o => o.OfferId == acceptOffer.offerId);

                //if(int.Parse(offer.Amount) > requestItem.Amount &&(offer.StartDate < requestItem.StartDate))
                //{

                //}
                //TODO : Check for date
                offer.RequestUserId = acceptOffer.userId;
                offer.RequestedItemId = acceptOffer.requestedItemId;
                offer.State = "reserved";
                requestItem.IsFulfilled = true;
                context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("Unable to accept the offer");
            }

        }


        
        [HttpGet("RequestedItems/{userId}")]
        public IActionResult GetRequestedItems(int userId)
        {
            var eventsIdlist = context.Events.Where(x=>x.UserId !=  userId).Select(x=>x.EventId).ToList();
            var items = context.RequestedItems
                .Where(x=>!eventsIdlist.Contains(x.EventId)).ToList();
            return Ok(items);
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
                    if (string.Compare(hashedPassword, (user.PasswordHash)) == 0)
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
using System.ComponentModel.DataAnnotations;

namespace Core.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }



        // this will make EF core make a 1 -> 1 relationship easy and make foreign key

        [Required] //!nullable 
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
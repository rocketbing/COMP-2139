using Booking.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace UnitTesting.Model
{
    public class HotelTesting
    {
        [Fact]
        public void Id_Should_Have_Key_Attribute()
        {
            // Arrange
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Sample Hotel",
                Location = "Sample Location",
                NumberOfRooms = 10,
                PricePerNight = 100
            };

            // Act
            var idProperty = hotel.GetType().GetProperty(nameof(Hotel.Id));
            var keyAttribute = idProperty.GetCustomAttributes(typeof(KeyAttribute), true);

            // Assert
            Assert.True(keyAttribute.Length > 0, "Id property should have Key attribute");
        }
    }
}

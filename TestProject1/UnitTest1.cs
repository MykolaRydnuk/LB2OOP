using NUnit.Framework;
using System;
using Main;

namespace Test
{

    [TestFixture]
    public class ReservationManagerTests
    {
        [Test]
        public void AddRestaurant_ValidInput_ShouldAddRestaurant()
        {
            ReservationManager manager = new ReservationManager();

            manager.AddRestaurant("C", 8);

            Assert.AreEqual(1, manager.Restaurants.Count);
            Assert.AreEqual("C", manager.Restaurants[0].Name);
            Assert.AreEqual(8, manager.Restaurants[0].Tables.Length);
        }

        [Test]
        public void BookTable_ValidBooking_ShouldReturnTrue()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("D", 3);

            bool result = manager.BookTable("D", DateTime.Now, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void BookTable_InvalidRestaurant_ShouldReturnFalse()
        {
            ReservationManager manager = new ReservationManager();

            bool result = manager.BookTable("Nonexistent", DateTime.Now, 1);

            Assert.IsFalse(result);
        }

        [Test]
        public void Sort_ValidInput_ShouldSortRestaurantsByFreeTables()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("F", 3);
            manager.AddRestaurant("G", 5);

            manager.BookTable("F", DateTime.Now, 0);
            manager.BookTable("G", DateTime.Now, 1);

            manager.Sort(DateTime.Now);

            Assert.AreEqual("G", manager.Restaurants[0].Name);
            Assert.AreEqual("F", manager.Restaurants[1].Name);
        }
    }
}
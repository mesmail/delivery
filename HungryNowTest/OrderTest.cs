using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HungryNow;
using HungryNow.Entities;
using HungryNow.Payments;
using HungryNow.Utilites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HungryNowTest
{
	[TestClass]
	public class OrderTest
	{
		Customer customer = null;
		Restaurant restaurant = null;
		Rider rider = null;
		List<Food> foods = new List<Food>();

		IPayable customerAccount;
		IPayable restaurantAccount;
		IPayable riderAccount;

		#region Prepare data
		[TestInitialize()]
		public void Startup()
		{
			customerAccount = new Wallet(1000);
			restaurantAccount = new Wallet(1000);
			riderAccount = new Wallet(1000);

			customer = new Customer(customerAccount)
			{
				Name = "Piyumal",
				Address = new Address()
				{
					Location = new Location(100, 100)
				},
				PhoneNumber = "0790123456"
			};

			restaurant = new Restaurant(restaurantAccount)
			{
				Name = "Tasty Foods",
				Address = new Address()
				{
					Location = new Location(101.5, 102)
				}
			};

			rider = new Rider(riderAccount)
			{
				Name = "Dilan",
				Address = new Address()
				{
					Location = new Location(104.5, 106)
				},
			//	waitingTime = 10
				
				
			};

			Food food1 = new Food()
			{
				Name = "Fried Rice",
				SpicyLevel = SpicyLevels.TooSpicy,
				Price = 350,
				PreperationTime = 2
			};

			Food food2 = new Food()
			{
				Name = "Kottu",
				SpicyLevel = SpicyLevels.TooSpicy,
				Price = 300,
				PreperationTime = 5
			};

			foods.Add(food1);
			foods.Add(food2);
		}
		#endregion

		#region Task 1
		[TestMethod]
		public void TestEstimatedTime() // Task 1
		{
			Order order = new Order(customer, restaurant, rider, foods);

			double estimatedTime = order.EstimateTimeToPrepare() + order.EstimateTimeToDeliver();
			Assert.IsTrue(estimatedTime == 22); //(2+5)+(5+2.5)*2
		}
		#endregion

		#region Task 2
		[TestMethod]
		public void TestCancelingOrderWhenFoodIsNotPreparedWithinEstimatedTime() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);
			bool cancellationResult = false;

			if (order.ConfirmFromRestaurant() && order.ConfirmFromRider())
			{
				if (order.HandlePaymentsOfCustomerAndRestaurant() && order.HandlePaymentOfRider())
				{
					order.StartPrepareOrder();
					Thread.Sleep((int) order.EstimateTimeToPrepare() * 1000 + 1000);

					// Customer cancels the order as it take too long time for preparation
					cancellationResult = order.CancelOrder();
				}
			}

			Assert.IsTrue(cancellationResult == true);
			Assert.IsTrue(customerAccount.Amount == 1000);
			Assert.IsTrue(restaurantAccount.Amount == 1000);
			Assert.IsTrue(riderAccount.Amount == 1000);
		}

		[TestMethod]
		public void TestCancelingOrderWhenFoodIsNotDeliveredWithinEstimatedTime() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);
			bool cancellationResult = false;

			if (order.HandlePaymentsOfCustomerAndRestaurant() && order.HandlePaymentOfRider())
			{
				order.StartPrepareOrder();
				Thread.Sleep((int) order.EstimateTimeToPrepare() * 1000 - 1000);
				order.FinishPrepareOrder();

				order.StartDeliverFood();
				Thread.Sleep((int) order.EstimateTimeToDeliver() * 1000 + 1000);

				// Customer cancels the order as it take too long time to deliver
				cancellationResult = order.CancelOrder();
			}

			Assert.IsTrue(cancellationResult == true);
			Assert.IsTrue(customerAccount.Amount == 1000);
			Assert.IsTrue(restaurantAccount.Amount == 1650);
			Assert.IsTrue(riderAccount.Amount == 350);
		}

		[TestMethod]
		public void TestCancelingOrderWhenFoodIsPreparedWithinEstimatedTime() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);
			bool cancellationResult = true;

			if (order.HandlePaymentsOfCustomerAndRestaurant() && order.HandlePaymentOfRider())
			{
				order.StartPrepareOrder();
				Thread.Sleep((int) order.EstimateTimeToPrepare() * 1000 - 1000);
				order.FinishPrepareOrder();

				cancellationResult = order.CancelOrder();
			}

			Assert.IsTrue(cancellationResult == false);
		}

		[TestMethod]
		public void TestCancelingOrderWhenFoodIsDeliveredWithinEstimatedTime() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);
			bool cancellationResult = true;

			if (order.HandlePaymentsOfCustomerAndRestaurant() && order.HandlePaymentOfRider())
			{
				order.StartPrepareOrder();
				Thread.Sleep((int) order.EstimateTimeToPrepare() * 1000 - 1000);
				order.FinishPrepareOrder();

				order.StartDeliverFood();
				Thread.Sleep((int) order.EstimateTimeToDeliver() * 1000 + 1000);
				order.FinishDeliverFood();

				cancellationResult = order.CancelOrder();
			}

			Assert.IsTrue(cancellationResult == false);
		}

		[TestMethod]
		public void TestRevisePayments() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);

			order.RevisePayments();
			Assert.IsTrue(customer.Payment.Amount == 1755); //1000+650+30+75
			Assert.IsTrue(restaurant.Payment.Amount == 350); //1000-650
			Assert.IsTrue(rider.Payment.Amount == 925); // 1000-75
		}

		[TestMethod]
		public void TestRevisePaymentsDeductingFromRider() // Task 2
		{
			Order order = new Order(customer, restaurant, rider, foods);

			order.RevisePayments(true);
			Assert.IsTrue(customer.Payment.Amount == 1755); //1000+650+30+75
			Assert.IsTrue(restaurant.Payment.Amount == 1000); //1000
			Assert.IsTrue(rider.Payment.Amount == 275); //1000-650-75
		}
		#endregion

		#region Task 3
		[TestMethod]
		public void TestSpicyLevelUpdate() //Task 3
		{
			Food food = new Food()
			{
				Name = "Fried Rice",
				SpicyLevel = SpicyLevels.TooSpicy,
				Price = 350,
				PreperationTime = 10
			};

			food.IncreaseSpicyLevel();
			Assert.IsTrue(food.SpicyLevel == SpicyLevels.BurningSpicy);
			food.IncreaseSpicyLevel();
			Assert.IsTrue(food.SpicyLevel == SpicyLevels.BurningSpicy);

			food.SpicyLevel = SpicyLevels.LessSpicy;
			food.DecreaseSpicyLevel();
			Assert.IsTrue(food.SpicyLevel == SpicyLevels.NoSpicy);
			food.DecreaseSpicyLevel();
			Assert.IsTrue(food.SpicyLevel == SpicyLevels.NoSpicy);
		}
		#endregion

		#region Task 4
		[TestMethod]
		public void TestHandlingPayments() //Task 4
		{
			Order order = new Order(customer, restaurant, foods);
			order.HandlePaymentsOfCustomerAndRestaurant();
			order.SetRider(rider);
			order.HandlePaymentOfRider();

			Assert.IsTrue(customerAccount.Amount == 245);
			Assert.IsTrue(restaurantAccount.Amount == 1650);
			Assert.IsTrue(riderAccount.Amount == 1075);
		}
		#endregion

		#region Task 5
		[TestMethod]
		public void TestNotifyCustomer() // Task 5
		{
			Order order = new Order(customer, restaurant, foods);
			string displayedText = order.TestNotification("Test info");
			Assert.IsTrue(displayedText.Contains("Customer Info - "));
		}
		#endregion
	}
}

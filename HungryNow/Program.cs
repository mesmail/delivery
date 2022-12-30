using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Entities;
using HungryNow.Payments;
using HungryNow.Utilites;

namespace HungryNow
{
	class Program
	{
		static Customer customer = null;
		static Restaurant restaurant = null;
		static Rider rider = null;
		static List<Food> foods = new List<Food>();

		static IPayable customerAccount;
		static IPayable restaurantAccount;
		static IPayable riderAccount;

		static void Main(string[] args)
		{
			PrepareData();

			// Current flow
			OrderFlowWhenRiderIsAssignedLater();

			// Flow on Task 1
			//OrderFlowWhenRiderIsAssignedAtTheBeginning();

			Console.Read();
		}

		static void OrderFlowWhenRiderIsAssignedLater()
		{
			Order order = new Order(customer, restaurant, foods);

			if (!order.ConfirmFromCustomer())
			{
				return;
			}

			if (order.ConfirmFromRestaurant())
			{
				if (order.HandlePaymentsOfCustomerAndRestaurant())
				{
					order.StartPrepareOrder();
					order.FinishPrepareOrder();
				}

				order.SetRider(rider);

				if (order.ConfirmFromRider())
				{
					if (order.HandlePaymentOfRider())
					{
						order.StartDeliverFood();
						order.FinishDeliverFood();
					}
				}
			}
		}

		static void OrderFlowWhenRiderIsAssignedAtTheBeginning()
		{
			Order order = new Order(customer, restaurant, rider, foods);

			if (!order.ConfirmFromCustomer())
			{
				return;
			}

			if (order.ConfirmFromRestaurant() && order.ConfirmFromRider())
			{
				if (order.HandlePaymentsOfCustomerAndRestaurant() && order.HandlePaymentOfRider())
				{
					order.StartPrepareOrder();
					order.FinishPrepareOrder();

					order.StartDeliverFood();
					order.FinishDeliverFood();
				}
			}
		}

		static void PrepareData()
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
				}
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
	}
}

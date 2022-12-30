using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HungryNow.Entities;
using HungryNow.Payments;
using HungryNow.Utilites;

namespace HungryNow
{
	public class Order
	{
		private readonly Entity customer;
		private readonly Entity restaurant;
		private readonly List<Food> foods;
		private Rider rider;
		
		public DateTime? PreperationStartTime { get; set; }
		private DateTime? PreperationEndTime { get; set; }
		private DateTime? DeliveryStartTime { get; set; }
		private DateTime? DeliveryEndTime { get; set; }

		public static IPayable HungryNowPayment { get; set; }

		public Order(Customer customer, Restaurant restaurant, List<Food> foods)
		{
			this.customer = customer;
			this.restaurant = restaurant;
			this.foods = foods;

			if (HungryNowPayment == null)
			{
				HungryNowPayment = new Wallet(0);
			}
		}

		public Order(Customer customer, Restaurant restaurant, Rider rider, List<Food> foods)
		{
			this.customer = customer;
			this.restaurant = restaurant;
			this.foods = foods;

			customer.Notify("Trying to assign a rider");
			this.rider = rider;

			if (HungryNowPayment == null)
			{
				HungryNowPayment = new Wallet(0);
			}
		}

		public void SetRider(Rider rider)
		{
			customer.Notify("Trying to assign a rider");
			this.rider = rider;
		}

		public bool ConfirmFromRestaurant()
		{
			customer.Notify("Confirming the order with restaurant by providing the list of food");
			return ((Restaurant)restaurant).Confirm(foods);
		}

		public bool ConfirmFromRider()
		{
			double estimatedTime = EstimateTimeToPrepare() + EstimateTimeToDeliver();
			customer.Notify("Confirming the order with rider by providing the time " + estimatedTime + " as he needs to fulfill the delivery");
			return ((Rider) rider).Confirm(restaurant.Address, customer.Address, estimatedTime);
		}

		public bool ConfirmFromCustomer()
		{
			double estimatedTime = EstimateTimeToPrepare() + EstimateTimeToDeliver();
			customer.Notify("Confirming the order with customer by providing the estimated time as " + estimatedTime);
			return ((Customer)customer).Confirm(estimatedTime);
		}

		public bool HandlePaymentsOfCustomerAndRestaurant()
		{
			double amount = foods.Sum(f => f.Price);
			if (customer.Payment.DeductAmount(amount + 30 + CalculateDeliveryCost()))
			{
				restaurant.Payment.AddAmount(amount);
				HungryNowPayment.AddAmount(30);
				return true;
			}

			return false;
		}

        public void RevisePayments(bool deductFromRider = false)
        {
            double amount = foods.Sum(f => f.Price);

            if (!deductFromRider)
            {
                customer.Payment.AddAmount(amount + 30 + CalculateDeliveryCost());
                restaurant.Payment.DeductAmount(amount);
                rider.Payment.DeductAmount(CalculateDeliveryCost());
                HungryNowPayment.DeductAmount(30);
            }
            else
            {
                customer.Payment.AddAmount(amount + 30 + CalculateDeliveryCost());
                rider.Payment.DeductAmount(amount + CalculateDeliveryCost());
            }
        }

        public bool HandlePaymentOfRider()
		{
			rider.Payment.AddAmount(CalculateDeliveryCost());
			return true;
		}

		public void StartPrepareOrder()
		{
			// Coordination with restaurant to prepare food
			customer.Notify("Preparing your order");
			PreperationStartTime = DateTime.Now;
		}
		public void FinishPrepareOrder()
		{
			// Confirm with restaurant when the food is ready
			customer.Notify("Your order is ready");
			PreperationEndTime = DateTime.Now;
		}

		public void StartDeliverFood()
		{
			// Coordination with rider to deliver
			customer.Notify("Rider is arriving to pick the food, and food will soon be on the way");
			DeliveryStartTime = DateTime.Now;
		}
		public void FinishDeliverFood()
		{
			// Confirm with rider when the food is delivered
			customer.Notify("Food is delivered");
			DeliveryEndTime = DateTime.Now;
		}

		public double EstimateTimeToDeliver()
		{
			return MapHelper.CalculateTime(restaurant.Address.Location, customer.Address.Location);// + rider.waitingTime; ;
		}

		public double EstimateTimeToPrepare()
		{
			return foods.Sum(f => f.PreperationTime);
		}

        public bool CancelOrder()
        {
            if (DeliveryStartTime == null && (DateTime.Now - PreperationStartTime)?.TotalSeconds > EstimateTimeToPrepare())
            {
                RevisePayments();
                return true;
            }
            else if (DeliveryEndTime == null && (DateTime.Now - DeliveryStartTime)?.TotalSeconds > EstimateTimeToDeliver())
            {
                RevisePayments(true);
                return true;
            }


            return false;
        }

        private double CalculateDeliveryCost()
		{
			 return MapHelper.CalculateDistance(restaurant.Address.Location, customer.Address.Location)*30 ;
		}

		public string TestNotification(string message)
		{
			return customer.Notify(message);
		}
	}
}

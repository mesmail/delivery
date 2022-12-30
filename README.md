HungryNow is a food ordering app with some basic operations.

Pre-requisites for the project: 

- Install Git : https://git-scm.com/downloads
- Install Visual Studio : https://visualstudio.microsoft.com/vs/community/
- .NET Core 3.0 


Current flow is as mentioned below
- A customer places an order by picking some foods from a particular restaurant
- The customer is informed the rough estimated time (This doesn't include the time takes for the rider to come to the restaurant to pick the order), and customer has to confirm the order
- Then the restaurant has to confirm the order too
- Complete amount (cost for the food, service charge, delivery fee) is deducted  from the customer's account, and if enough funds are not available on customer's account, the order is not placed
- Once the order is ready from the restaurant, a rider is assigned and once the rider confirms he will come to the restaurant to pick the order and the order is delivered to the customer

You can run OrderFlowWhenRiderIsAssignedLater method on Program.cs to simulate this flow.


There are so many complaints from customers that the estimated time is wrong.
- Estimated time = Estimated food preparation time + Estimated delivery time from the restaurant to the customer
- Actual time = Actual food preparation time + Actual delivery time from the restaurant to the customer + Time takes for the rider to come to the restaurant + Waiting time to find a rider
So, even the food is prepared and delivered within the estimated times, there could be an additional time takes to find a rider and for him to come to the restaurant to pick the order.

There are many reported cases that it was not possible to find a rider soon after preparing the food, and customers had to wait for longer times to get the food delivered.
Customers are very frustrated since they are unable to cancel an order in case of an order is delayed too.

Management has proposed 2 changes on the app to resolve these problems due to the fact that complains are getting higher and higher day by day, and below are those proposals.

Proposal 1: Pick a rider at the beginning of the process, so that customers can be given an accurate estimated time, and able to avoid the waiting time to find a rider at latter stage
Proposal 2: Provide the facility to cancel an order to customers, provided that either preparing food or delivering food is getting delayed

You are assigned Task 1 and Task 2 to achieve the goals mentioned in above proposals.

Task 1 (5 marks)
======
Pick a rider at the beginning of the process, so that customers can be informed with an accurate estimated time, and able to avoid waiting times to find a rider at latter stage and for him to come to the restaurant.

To achieve this, overloaded constructor with rider is defined on Order.cs class, and since we have the rider at the beginning of the process, now we can add the time taken for the rider to come to the restaurant into the delivery time.
Your task is to correct this, so that customer can see more accurate estimation time.

You can run OrderFlowWhenRiderIsAssignedAtTheBeginning method on Program.cs to simulate this flow.
==============================================================

Task 2 (30 marks)
======
Current flow of this app doesn't support canceling an order, and you need to implement it to satisfy below requirements.
- Customer is able to cancel an order, only if the estimated time is exceeded (Exceeding order preparation time or exceeding delivery time which are explained below as A and B).
  In other words if the order is prepared within the estimated time, and delivered within the estimated time, customer is unable to cancel an order at any time.

(A) - If food preparation estimated time is exceeded before complete preparing an order, customer can cancel the order before handing over the order for delivery.
  In this scenario, all payments are reversed completely so that full amount would be refunded to the customer, and the restaurant has to keep the half prepared food by themselves.
(B) - Once the order is prepared, and not delivered within the estimated time, customer can cancel the order.
  In this scenario, full amount is refunded to the customer, however the rider has to accept the lost for the food, and take the food for himself.
  Note: Amount transferred to the restaurant's account remains same, and the transfer of service charge is revered.

Eg for (A): If the estimated time for food preparation is informed as 5 minutes to the customer, the customer is able to cancel the order only if it takes more than 5 minutes to prepare the order.
Eg for (B): After order is prepared by the restaurant in 5 minutes, if the estimated time for the delivery is 10 minutes, customer is able to cancel the order only if the order doesn't receive within 10 minutes.

You need to implement the logic in CancelOrder method in Order.cs, where true should be returned where cancellation is possible, and false if not.
You need to use the existing method RevisePayments inside CancelOrder by updating the logic as necessary to handle payments if cancellation is possible. The additional parameter deductFromRider here is used to set true to distinguish scenario (B).
==============================================================


There are some other bugs on the existing flow as well, and Task 3, 4 and 5 are to resolve them.

Task 3 (10 marks)
======
We have 2 methods implemented on Food entity (on Food.cs class) IncreaseSpicyLevel and DecreaseSpicyLevel, and we have a problem of handling 2 scenarios here.
1) Keeping the maximum spicy level as it is if customer wants to make the food even more spicy
   It's incrementing further to invalid numbers and you need to fix this.
2) Keeping the minimum spicy level as it is if customer wants to make the food even less spicy
   It's decrementing further to invalid numbers and you need to fix this.

Here you should not update the logic based on the hard coded values defined on the enum, since there could be new spicy levels added to the enum, and then it would be a hassle to update the logic as well.
So, you are expected to come up with a general solution to handle the problem even with some future updates on the enum.
==============================================================

Task 4 (10 marks)
======
Delivery charge should be calculated based on the distance from the restaurant location to the customer location, and 30 bucks for each distance unit.
However currently it's always calculated as a fix amount of 30 due to a mistake on initial implementation.
Until now, it was not a problem since most of the deliveries were done within short distances, but now it's the time to fix this bug.

Your task is to fix this bug, so that delivery fee is calculated based on the distance. Regarding calculating the distance between different addresses, to get an idea, you may refer the logic we use to calculate the time based on the distance.
==============================================================

Task 5 (15 marks)
======
If you have noticed, the info messages are prompted to the customer in the format of "Info - Date/Time: xxxxxx".
However if you check Customer.cs, we have special logic implemented on how we should show the info messages to the customers in the format of "Customer Info - Date/Time: xxxxxx".

Your task is to fix this bug, so that when we use customer.Notify, it should pick the logic implemented on Customer.cs.
==============================================================

Special Notes
- Program.cs is just for simulation purposes and any changes done there would not be evaluated, even though you are allowed to update it for testing purposes.
- Cash on delivery option is not available on this app
- HungryNow company rakes a service charge of 30 bucks for each order
- Delivery time for a distance unit is considered as 2
- Distance is calculated from the Pythagorean theorem based on straight distances and it may vary from the actual distance calculated from actual roads
- All times set are considered in seconds in calculations for simulation purposes even though they represent very short times which are not enough for preparing foods and delivering them practically
- We assume the rider is back on the same location once the food is prepared same as on how he was when confirming the ride
- Some best practices are not used when implementing this app which is used only for simulation purposes.

DevGrade Technical Support : +94743546446
=========================================

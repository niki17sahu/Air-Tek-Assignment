using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Air_Tek_Coding_Exercise
{
    class Program
    {
        private static string flightScheduleInput = ConfigurationManager.AppSettings["FlightScheduleSourceFile"];
        private static string multiFlightScheduleInput = ConfigurationManager.AppSettings["MultipleFlightScheduleSourceFile"];
        private static string ordersInput = ConfigurationManager.AppSettings["OrdersFile"];
        private static List<Flight> scheduledFlights;
        private static Logger logger = new Logger();

        //This dictionary holds the box count for each flight: key-Flight Object; value: box acount
        private static Dictionary<Flight, int> flightLoad = new Dictionary<Flight, int>();

        //This dictionary holds the order shipment details: key-Order number; value: flight object where this shipment is scheduled
        private static Dictionary<string, Flight> OrderSchedule = new Dictionary<string, Flight>();

        //This dictionary stores the deserialized JSON input of the order list with order number and destination
        private static Dictionary<string, string> OrderList = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            //USE CASE 1
            LoadAndOuputFlightSchedule();

            //USE CASE 2 - Load the Json
            LoadOrderDetails();

            //USE CASE 2 - Display the schedule 
            PrintShipmentDeliverySchdeule();

            logger.LogInformation(DateTime.Now + " - Program Completed!");
        }

        /// <summary>
        /// This method is responsible for loading the Flight schedule
        /// Reads the schedule from a JSON file provided by the user
        /// Sample input file is: multiFlightSchedule.json
        /// </summary>
        private static void LoadAndOuputFlightSchedule()
        {
            try
            {
                string jsonFlightDetailsMulti = System.IO.File.ReadAllText(multiFlightScheduleInput);
                scheduledFlights = JsonConvert.DeserializeObject<List<Flight>>(jsonFlightDetailsMulti);

                // Expected output --- Flight: 1, departure: YUL, arrival: YYZ, day: 1
                foreach (Flight flight in scheduledFlights)
                {
                    Console.WriteLine("Flight: " + flight.FlightNumber + ", departure: " + flight.Source + ", arrival: " + flight.Destination + ", day: " + flight.Day);
                    flightLoad.Add(flight, 0);
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                logger.LogError("Error Occured While Creating the Flight Schedule: Stack Trace - " + ex.StackTrace + "Error Message: " + ex.Message);
            }
        }

        /// <summary>
        /// This method is responsible for loading the Order details
        /// Reads the Order details from a JSON file provided by the user
        /// Sample input file is: coding-assigment-orders.json
        /// </summary>
        private static void LoadOrderDetails()
        {
            try
            {
                string jsonOrderDetailsMulti = System.IO.File.ReadAllText(ordersInput);
                dynamic orderDetials = JObject.Parse(jsonOrderDetailsMulti);

                //Loop through the dynamic object loaded to create the OrderSchedule for each and every order
                foreach (var item in orderDetials)
                {
                    Order order = JsonConvert.DeserializeObject<Order>(item.Value.ToString());
                    OrderList.Add(item.Name, order.destination);
                    OrderSchedule.Add(item.Name, null);

                    var flight = flightLoad.FirstOrDefault(t => t.Key.Destination == order.destination && t.Value < 20);

                    if (flight.Key != null)
                    {
                        flightLoad[flight.Key]++;
                        OrderSchedule[item.Name] = flight.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error Occured While Order Loading: Stack Trace - " + ex.StackTrace + "Error Message: " + ex.Message);
            }
        }

        /// <summary>
        /// This method is responsible for displaying the delivery schdeule for each order.
        /// Prints the details per order 
        /// </summary>
        private static void PrintShipmentDeliverySchdeule()
        {
            try
            {
                //order: order - 001, flightNumber: 1, departure: < departure_city >, arrival: < arrival_city >, day: x
                //order: order-X, flightNumber: not scheduled
                foreach (string order in OrderSchedule.Keys)
                {
                    if (OrderSchedule[order] != null)
                        Console.WriteLine("order - " + order + ", flightNumber: " + OrderSchedule[order].FlightNumber + ", departure: " + OrderSchedule[order].Source + ", arrival: " + OrderSchedule[order].Destination + ", day: " + OrderSchedule[order].Day);
                    else
                        Console.WriteLine("order - " + order + ", flightNumber: not scheduled");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                logger.LogError("Error Occured While Preparing Shipment Schedule: Stack Trace - " + ex.StackTrace + "Error Message: " + ex.Message);
            }
        }

        
    }
}

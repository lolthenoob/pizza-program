//This Project was created by Jonathan Tan on 19/09/17
//It is a program created to allow customers to order pizzas



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace pizza_program
{
    class Program
    {
        // Declaring Constant Variables
        public const int maxPizza = 5;
        public const double deliveryCharge = 3.00;
        public const double baseCharge = 5.00;

        


        //Main Method
        static void Main(string[] args)
        {
            Boolean exitProgram = false;

            while(!exitProgram)
            {
                //Declaring the arrays used
                String[] customerArray = new String [4];
                customerArray[2] = "";
                customerArray[3] = "";


                String[,] menuArray = new String[13, 2];
                int[] orderArray = new int[11];

                Console.WriteLine("===============================");
                Console.WriteLine("=Welcome To Jon's Pizza Galour=");
                Console.WriteLine("===============================\n");

                //Getting the menuArray from getMenuInfo() method
                menuArray = getMenuInfo(menuArray);

                Console.WriteLine("================");
                Console.WriteLine("==Pizza Menu===");
                Console.WriteLine("================");

                //Display all the pizza names and prices from menuArray
                for(int i = 0; i < menuArray.Length/2  ; i ++)
                {
                    if (i <= 8)
                    {
                        Console.WriteLine(i + 1 + ")  " + menuArray[i, 0].PadRight(18) + "---------------- $" + menuArray[i, 1]);
                    }

                    if (i >= 9)
                    {
                        Console.WriteLine(i + 1 + ") " + menuArray[i, 0].PadRight(18) + "---------------- $" + menuArray[i, 1]);
                    }
                }

                Console.WriteLine("");

                //Getting array infromation from respective classes
                customerArray = getCustInfo(customerArray);    
                orderArray = getOrderInfo(orderArray);
                showOrder(customerArray,menuArray,orderArray);

                //Calling store data method
                storeData(customerArray, orderArray, menuArray);

                //Method to check if customer wants to place another order
                string anotherOrder = Readstring("Would You Want To Place Another Order (Y/N)");               
                if(anotherOrder.ToLower()!= "y")
                {
                    Console.WriteLine("Press Any Key to Exit");
                    Console.ReadLine();
                    exitProgram = true;
                }
                        

            }
        }


        
        //Method for getting customer info
        public static string[] getCustInfo(string[] customerArray)
        {

            //initiating the loop for getting customer info
            Boolean success = false;
            while(! success)
            {
                
                //Check if the customer wants a delivery or pickup and saves it to customer array
                //Then store name of cutomer
                customerArray[0] = ReadChar("Is The Order For Pickup or Delivery(P or D): ");
                customerArray[1] = Readstring("\nPlease Enter your name: ");

                //Exits the loop if order is for pickup
                if (customerArray[0].ToLower() == "p")
                {
                    
                    success = true;
                }

                //If customer is for delivery, get phone number and address
                //Then store it to customer array
                else if (customerArray[0].ToLower() == "d")
                {
                    
                    customerArray[2] = ReadAddress("\nPlease Enter Your Address: ");
                    customerArray[3] = Convert.ToString(ReadPhone("\nEnter Your Phone Number: "));
                    success = true;
                }

                else
                    //If customer choose a charcter other than 'p" or "d"
                    //A error message will be displayed and success loop will repeat
                    Console.Write(" Your Entered A Invalid Input, Please Try Again\n");
            }
            return customerArray;
        }


        //Method for getting menu infomation form csv file
        public static string[,] getMenuInfo(string[,] menuArray)
        {
            string whole_file = System.IO.File.ReadAllText("pizzaMenu.csv");

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(',').Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }
            menuArray = values;
            return menuArray;
        }


        //Method for getting order from customer
        public static int[] getOrderInfo(int[] orderArray)
        {
            Console.WriteLine("");
            
            //Saves the number of pizza customer chooses
            orderArray[0] = ReadOrder("How many pizza would you like to order (Max 5): ");

            // displays the number of choices of pizza the customer wants
            for (int choice = 1; choice < orderArray[0]+1; choice++)  
            {
                orderArray[choice] = ReadChoice("\nChoice " + choice + " (Type In The Number To the left Of Your Desired Pizza): ");
            }



            return orderArray;
        }

        //Method for combining info from customerArray, menuArray and OrderArray
        //This to display in the receipt
        public static void showOrder(string[] customerArray,string[,] menuArray,int[] orderArray)
        {
            Console.WriteLine("");
            Console.WriteLine("============================================");
            Console.WriteLine("===============Pizza Selection==============");
            Console.WriteLine("============================================\n");

            // For loop to loop through all the pizza the customer wants and display them
            for (int i = 1; i < orderArray[0]+1; i++) 
            {
                Console.WriteLine(menuArray[orderArray[i], 0].PadRight(18) + "---------------- $" + menuArray[orderArray[i], 1] );
            }

            //If customer is for delivery , add a delivery charge
            if(customerArray[0].ToLower() == "d")
            {
                Console.WriteLine("\nDelivery Charge                    $" + deliveryCharge.ToString("F"));
            }
            Console.WriteLine("\n============================================");

            //Call the bill claculation method
            calcPrice(customerArray,menuArray,orderArray);
            
            //Display order and customer details
            if(customerArray[0].ToLower() == "p")
            {

                Console.WriteLine("\n++++++++++++++++");
                Console.WriteLine("Customer Details");
                Console.WriteLine("++++++++++++++++\n");

                Console.WriteLine("Name: " + customerArray[1] +"\n");

                Console.WriteLine("Your Order Will Be Ready In 20 Minutes");
                Console.WriteLine("Please Pickup Your Pizza At Jon's Pizzeria");
            }

            if(customerArray[0].ToLower() == "d")
            {
                Console.WriteLine("Your Order Is For Delivery\n");

                Console.WriteLine("Customer Details");
                Console.WriteLine("Name: " + customerArray[1]);
                Console.WriteLine("Phone Number: " + customerArray[3]);
                Console.WriteLine("Address: " + customerArray[2] + "\n");

                Console.WriteLine("Your Pizza Will Reach The Specified Address Within 20 minutes");
                Console.WriteLine("There is a Delivery Charge of " + deliveryCharge.ToString("F"));
                
            }
        }
    
        //Method for reading string, and displaying a error if input is invalid
        static string Readstring(String prompt)
        {
        string userInput = "";
            Boolean success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Console.ReadLine();
                    if (userInput.Length <= 20 && userInput.Trim().Length > 0) //check length
                    {
                        success = true;// Assumes input is correct

                        foreach (char character in userInput)
                        {
                           // Look at each character and check if it is a number, symbol or punctuation
                        if (Char.IsNumber(character) || Char.IsSymbol(character) || Char.IsPunctuation(character)) 
                            {
                                success = false;
                            }
                        }
                    }
                    if (success == false)
                    {
                        Console.WriteLine("You Entered A Invalid Input, Please Try Again" + "\n");
                    }
                    else
                    {
                        success = true; // Enables loop to exit
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return userInput;
}

        //Method for reading int, and displays a error if input is invalid
        static int ReadInt(string prompt)
        {
            int userInput = 0;
        Boolean success = false;
        while (!success)
        {
            Console.Write(prompt);
            try
            {
                //Convert string input to int
                userInput = Convert.ToInt32(Console.ReadLine());
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Your Response Is Invalid, Please Try Again");
            }
        }
        return userInput;

        }

        //Method for reading order
        static int ReadChoice(string prompt)
        {
            int userInput = 0;
            Boolean success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());

                    //Check if user input is 1< x < 13, as that is the range of pizzas.
                    if (userInput >= 1  && userInput <= 13 )
                    {
                        userInput--;
                        success = true;
                    }

                    else
                    {
                        success = false;
                        Console.WriteLine("Your Response Is Invalid, Please Try Again");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Your Response Is Invalid, Please Enter The Number Of The Pizza From The Menu");
                }
            }
            return userInput;

        }

        //Method for processsing char
        public static string ReadChar(string prompt)
    {

        string userInput = "";
            Boolean success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Console.ReadLine();
                    if (userInput.ToLower() == "p" || userInput.ToLower() == "d")  //check if the input is euqla to p or q
                    {

                        success = true;// Assumes input is correct
                    }
                    else
                    {
                        Console.WriteLine("\nYou Entered A Invalid Input\nType In 'p' For Pickup or 'd' For Delivery\n");
                        success = false; // Enables loop to exit
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return userInput;
    }

        //Method to read phone number
        public static int ReadPhone(string prompt)         
        {
            int userInput = 0;
            Boolean success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());

                    //Check if user's phone number is valid
                    if (userInput.ToString().Length == 9 || userInput.ToString().Length == 10)
                    {
                        success = true;
                    }

                    else
                    {
                        Console.WriteLine("Your Phone Number Is Invalid, Please Try Again\n");
                        success = false;
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Your Phone Number Is Invalid, Please Try Again\n");
                }
            }
            return userInput;
        }

        //MEthod for pizza choice
        public static int ReadOrder(string prompt)
        {
            int userInput = 0;
            Boolean success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());

                    //Customer can choose only up to 5 pizzas
                    if (userInput > 0 && userInput <= 5 )
                    {
                        success = true;
                    }

                    else
                    {
                        Console.WriteLine("Your Response Is Invalid, Only Up to 10 Pizzas Can Be Ordered At Once\n");
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Your Response Is Invalid, Please Try Again\n");
                }
            }
            return userInput;
        }

        //Method of calculation of price
        public static void calcPrice(string[] customerArray,string[,] menuArray,int[] orderArray)
        {
            double bill = 0;

            //Go through all pizza in orderarray, and calculate their prices
            for(int i = 1; i < orderArray[0]+1; i++)
            {
                bill += Convert.ToDouble(menuArray[orderArray[i], 1]);
            }

            //add delivery charge to bill
            if(customerArray[0].ToLower() == "d")
            {
                bill += deliveryCharge;
            }

            decimal decimalBill = System.Convert.ToDecimal(bill);
            Console.WriteLine("Total Bill" + "                         $" + decimalBill.ToString("F") + "\n\n");

           
        }

  
        //Method for reading address
        public static string ReadAddress(String prompt)
        {
            string userInput = " ";
            bool success = false;
            while (!success)
            {
                Console.Write(prompt);
                try
                {
                    userInput = Console.ReadLine();
                    if (userInput.Length <= 70 && userInput.Length > 10) //checks the length of the user input
                    {
 
                        success = true;     //Assumes input is correct
                        bool letter = false;
                        bool space = false;
                        bool number = false;
                       
                        foreach (char character in userInput)
                        {
                            letter = letter || Char.IsLetter(character);
                            space = letter || Char.IsWhiteSpace(character);  
                            number = letter || Char.IsNumber(character);
                        }
                        success = letter & space & number;
                    }
                    else
                    {
                        Console.WriteLine("Please Enter A Valid Address");
                        success = false; // enables loop to exit
                       
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return userInput;
        }


        //Store customer data
        public static void storeData(string[] customerArray, int[] orderArray, string[,] menuArray)

        {

            string custInfo = "";
            
            custInfo += customerArray[1] + "," + customerArray[2] + "," + customerArray[3] +",";

            for (int i = 1; i < orderArray[0] + 1; i++)
            {
                   custInfo += menuArray[orderArray[i], 0] + ",";           
            }

            custInfo += "\n";
            File.AppendAllText("PizzaCustInfo.csv", custInfo);
        }
    }         
}
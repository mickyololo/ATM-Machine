
using System;

namespace ATM
{
    class AtmMachine
    {
        static void Main(string[] args)
        {
            // Create an instance of ATM with initial balance and PIN
            Atm atm = new Atm(10000, "1234");

            // Run the ATM interface
            atm.Run();
        }
    }
}
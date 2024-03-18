using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    // Define a delegate for PIN validation
    delegate bool PinValidationDelegate(string enteredPin);

    public class Atm
    {
        private Account _account;
        private PinValidationDelegate _pinValidator;

        public Atm(decimal initialBalance, string pin)
        {
            _account = new Account(initialBalance, pin);
            _pinValidator = ValidatePin;
        }

        public void Run()
        {
            string choice = "";

            while (choice != "5")
            {
                Console.WriteLine("Welcome to the ATM. Choose an option:");
                Console.WriteLine("1 - Check Balance");
                Console.WriteLine("2 - Deposit");
                Console.WriteLine("3 - Withdraw");
                Console.WriteLine("4 - Transfer");
                Console.WriteLine("5 - Exit");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Your balance is {_account.Balance:C}");
                        break;
                    case "2":
                        Console.Write("Enter deposit amount: ");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());
                        _account.Deposit(depositAmount);
                        Console.WriteLine($"Your new balance is {_account.Balance:C}");
                        break;
                    case "3":
                        Console.Write("Enter withdrawal amount: ");
                        decimal withdrawalAmount = decimal.Parse(Console.ReadLine());
                        try
                        {
                            _account.Withdraw(withdrawalAmount, _pinValidator);
                            Console.WriteLine($"Your new balance is {_account.Balance:C}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "4":
                        Console.Write("Enter the account number to transfer to: ");
                        string accountNumber = Console.ReadLine();

                        Console.Write("Enter transfer amount: ");
                        decimal transferAmount = decimal.Parse(Console.ReadLine());

                        try
                        {
                            _account.Transfer(accountNumber, transferAmount, _pinValidator);
                            Console.WriteLine($"Your new balance is {_account.Balance:C}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "5":
                        Console.WriteLine("Thank you for using the ATM.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        // Method to validate PIN
        private bool ValidatePin(string enteredPin)
        {
            // You can implement your PIN validation logic here
            return enteredPin == _account.GetPin();
        }
    }

    public class Account
    {
        private decimal _balance;
        private string _pin;
        private bool _isBlocked;
        private int _incorrectPinAttempts;

        public decimal Balance
        {
            get { return _balance; }
        }

        public Account(decimal balance, string pin)
        {
            _balance = balance;
            _pin = pin;
            _isBlocked = false;
            _incorrectPinAttempts = 0;
        }

        public void Deposit(decimal amount)
        {
            _balance += amount;
        }

        public void Withdraw(decimal amount, PinValidationDelegate pinValidator)
        {
            if (_isBlocked)
            {
                throw new Exception("Account is blocked.");
            }

            if (!pinValidator(GetPin()))
            {
                IncorrectPinEntered();
                throw new Exception("Invalid PIN.");
            }

            if (amount > _balance)
            {
                throw new Exception("Insufficient funds.");
            }

            _balance -= amount;
        }

        public void Transfer(string accountNumber, decimal amount, PinValidationDelegate pinValidator)
        {
            if (_isBlocked)
            {
                throw new Exception("Account is blocked.");
            }

            if (!pinValidator(GetPin()))
            {
                IncorrectPinEntered();
                throw new Exception("Invalid PIN.");
            }

            if (amount > _balance)
            {
                throw new Exception("Insufficient funds.");
            }

            // simulate transfer to another account on the same bank
            // for demo purposes, we assume the transfer is successful
            _balance -= amount;
        }

        public string GetPin()
        {
            Console.Write("Enter PIN: ");
            return Console.ReadLine();
        }

        private void IncorrectPinEntered()
        {
            _incorrectPinAttempts++;

            if (_incorrectPinAttempts >= 3)
            {
                BlockAccount();
            }
        }

        private void BlockAccount()
        {
            _isBlocked = true;
            Console.WriteLine("Account is blocked.");
        }
    }
}
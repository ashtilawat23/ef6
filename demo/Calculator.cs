using System;

namespace Demo
{
    /// <summary>
    /// A simple calculator class to demonstrate C# features
    /// </summary>
    public class Calculator
    {
        private double memory;

        public Calculator()
        {
            memory = 0;
        }

        /// <summary>
        /// Adds two numbers
        /// </summary>
        public double Add(double a, double b) => a + b;

        /// <summary>
        /// Subtracts second number from first
        /// </summary>
        public double Subtract(double a, double b) => a - b;

        /// <summary>
        /// Multiplies two numbers
        /// </summary>
        public double Multiply(double a, double b) => a * b;

        /// <summary>
        /// Divides first number by second
        /// </summary>
        /// <exception cref="DivideByZeroException">Thrown when divisor is zero</exception>
        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero");
            return a / b;
        }

        /// <summary>
        /// Stores a number in memory
        /// </summary>
        public void StoreInMemory(double number)
        {
            memory = number;
        }

        /// <summary>
        /// Recalls the number from memory
        /// </summary>
        public double RecallMemory() => memory;

        /// <summary>
        /// Clears the memory
        /// </summary>
        public void ClearMemory()
        {
            memory = 0;
        }
    }
} 
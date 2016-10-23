﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BagOUtils.Guards.Messages;

namespace BagOUtils.Guards
{
    /// <summary>
    /// Defend against invalid states/parameters/arguments when the
    /// state/parameter/argument is a numeric value. Each of the methods
    /// defines a "guard". The guard validates that the
    /// state/parameter/argument does not have a particular type of
    /// potential error. If the guard finds the state/parameter/argument
    /// to be in error, an exception is thrown. If the value is
    /// validated, the value is returned, allowing for a fluent guard API.
    /// </summary>
    public static class NumericGuards
    {
        /// <summary>
        /// Guard that a numeric value is equal to or above the minimum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Minimum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMinimum<T>(this T value, string argumentName, T minimumValue)
            where T : IComparable<T>
        {
            var message = CustomTemplate
                .BelowMinimum
                .UsingItem(argumentName)
                .UsingValue(value)
                .ComparedTo(minimumValue)
                .Prepare();
            return value
                .GuardMinimumWithMessage(argumentName, minimumValue, message);
        }

        /// <summary>
        /// Guard that a numeric value is equal to or above the minimum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Minimum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMinimumWithMessage<T>(this T value, string argumentName, T minimumValue, string message)
            where T : IComparable<T>
        {
            if (value.LessThan(minimumValue))
            {
                throw new ArgumentOutOfRangeException(argumentName, message);
            }

            return value;
        }

        /// <summary>
        /// Guard that a numeric value is equal to or less than the
        /// maximum value.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter/state being tested.
        /// </param>
        /// <param name="minimumValue">
        /// Maximum acceptable value.
        /// </param>
        /// <returns>
        /// The value is returned if it is valid.
        /// </returns>
        public static T GuardMaximum<T>(this T value, string argumentName, T maximumValue) where T : IComparable<T>
        {
            if (value.GreaterThan(maximumValue))
            {
                var exMessage = CustomTemplate
                    .AboveMaximum
                    .UsingItem(argumentName)
                    .UsingValue(value)
                    .ComparedTo(maximumValue)
                    .Prepare();
                throw new ArgumentOutOfRangeException(argumentName, exMessage);
            }

            return value;
        }

        /// <summary>
        /// Validate that the value is within the provided limits, inclusive.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter.
        /// </param>
        /// <param name="lowerLimit">
        /// Lower limit of valid values.
        /// </param>
        /// <param name="upperLimit">
        /// Upper limit of valid values.
        /// </param>
        public static T GuardInRange<T>(this T value, string argumentName, T lowerLimit, T upperLimit) where T : IComparable<T>
        {
            var exMessage = CustomTemplate
                .OutOfRange
                .UsingItem(argumentName)
                .WithMinimum(lowerLimit)
                .WithMaximum(upperLimit)
                .Prepare();
            return value
                .GuardInRangeWithMessage(argumentName, lowerLimit, upperLimit, exMessage);
        }

        /// <summary>
        /// Validate that the value is within the provided limits, inclusive.
        /// </summary>
        /// <param name="value">
        /// Value to test.
        /// </param>
        /// <param name="argumentName">
        /// Name of argument/parameter.
        /// </param>
        /// <param name="lowerLimit">
        /// Lower limit of valid values.
        /// </param>
        /// <param name="upperLimit">
        /// Upper limit of valid values.
        /// </param>
        public static T GuardInRangeWithMessage<T>(this T value, string argumentName, T lowerLimit, T upperLimit, string message)
            where T : IComparable<T>
        {
            if (value.LessThan(lowerLimit) || value.GreaterThan(upperLimit))
            {
                throw new ArgumentOutOfRangeException(argumentName, message);
            }
            return value;
        }
    }
}

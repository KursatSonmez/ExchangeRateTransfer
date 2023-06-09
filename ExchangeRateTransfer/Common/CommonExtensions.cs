﻿using System;

namespace ExchangeRateTransfer.Common
{
    internal static class CommonExtensions
    {
        public static string ToSimpleString(this Exception ex)
        {
            string message =
        "Exception type " + ex.GetType() + Environment.NewLine +
        "Exception message: " + ex.Message + Environment.NewLine +
        "Stack trace: " + ex.StackTrace + Environment.NewLine;
            if (ex.InnerException != null)
            {
                message += "---BEGIN InnerException--- " + Environment.NewLine +
                           "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                           "---END Inner Exception";
            }

            return message;
        }
    }
}

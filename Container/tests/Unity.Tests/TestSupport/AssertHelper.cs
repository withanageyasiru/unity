﻿using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Tests.v5.TestSupport
{
    /// <summary>
    /// Used because Microsoft.VisualStudio.TestTools.UnitTesting does not support Assert.ThrowsException and 
    /// Microsoft.VisualStudio.TestPlatform.UnitTestFramework does not support ExpectedExceptionAttribute
    /// </summary>
    public static class AssertHelper
    {
        public static T ThrowsException<T>(Action action) where T : Exception
        {
            return ThrowsException<T>(action, string.Empty, null);
        }

        public static T ThrowsException<T>(Action action, string message) where T : Exception
        {
            return ThrowsException<T>(action, message, null);
        }

        public static T ThrowsException<T>(Func<object> action) where T : Exception
        {
            return ThrowsException<T>(action, string.Empty, null);
        }

        public static T ThrowsException<T>(Func<object> action, string message) where T : Exception
        {
            return ThrowsException<T>(action, message, null);
        }

        public static T ThrowsException<T>(Func<object> action, string message, params object[] parameters) where T : Exception
        {
            return ThrowsException<T>(delegate
            {
                action.Invoke();
            }, message, parameters);
        }

        public static T ThrowsException<T>(Action action, string message, params object[] parameters)
            where T : Exception
        {
            string message2 = string.Empty;

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (typeof(T) != ex.GetType())
                {
                    message2 = string.Format(CultureInfo.CurrentCulture,
                        @"Threw Exception {2}, but exception {1} was expected.{0}
Exception Message: {3}
Stack Trace : {4}", new object[]
                        {
                            message ?? string.Empty,
                            typeof(T).Name,
                            ex.GetType().Name,
                            ex.Message,
                            ex.StackTrace
                        });
                    Assert.Fail("Assert.ThrowsException", message2, parameters);
                }
                return (T)((object)ex);
            }

            message2 = string.Format(CultureInfo.CurrentCulture, "No exception thrown. {1} was expected. {0}", new object[]
            {
                message ?? string.Empty,
                typeof(T).Name
            });
            Assert.Fail("Assert.ThrowsException", message2, parameters);
            return default(T);
        }
    }
}

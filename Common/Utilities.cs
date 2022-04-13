// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.Management.Samples.Common
{
    public static class Utilities
    {
        public static bool IsRunningMocked { get; set; }
        public static Action<string?> LoggerMethod { get; set; }
        public static Func<string?> PauseMethod { get; set; }

        public static string ProjectPath { get; set; }

        static Utilities()
        {
            LoggerMethod = Console.WriteLine;
            PauseMethod = Console.ReadLine;
            ProjectPath = ".";
        }

        public static void Log(string message)
        {
            LoggerMethod.Invoke(message);
        }

        public static void Log(object obj)
        {
            if (obj != null)
            {
                LoggerMethod.Invoke(obj.ToString());
            }
            else
            {
                LoggerMethod.Invoke("(null)");
            }
        }

        public static void Log()
        {
            Utilities.Log("");
        }

        public static string? ReadLine()
        {
            return PauseMethod.Invoke();
        }
    }
}

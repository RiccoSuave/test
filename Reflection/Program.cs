using System;
using System.IO;
using System.Reflection; 

namespace Reflection
{
    // Here are some different delegate definitions
    internal delegate Object TwoInt32s(Int32 n1, Int32 n2);
    internal delegate Object OneString(String s1);
    public static class DelegateReflection
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2) { 
                String usage =
                @"Usage:" +
                "{0} delType methodName [Arg1] [Arg2]" +
                "{0} where delType must be TowInt32s or OneString" +
                "{0} if delType is TwoInt32s, methodName must be Add or Subtract" +
                "{0} if delType is  Onestring, methodName must be NumChars or Reverse" +
                "{0} " +
                "{0}Examples: "+
                "{0}  TwoInt32s Add 123 321" +
                "{0}  TwoInts32s Subtract 123 321" +
                "{0}  OneString NumChars \"Hello There\""+
                "{0}  OneString Reverse \"Hello There\"";
                Console.WriteLine(usage, Environment.NewLine);
                //Console.ReadLine();
                return;
            }
            // Convert the delType argument to a delegate type
            Type delType = Type.GetType(args[0]);
            if (delType == null) {
                Console.WriteLine("Invalid delType argument: " + args[0]);
                return; 
            }
            Delegate d;
            try {
                // Convert the Arg1 argument toa  method
                MethodInfo mi = typeof(DelegateReflection).GetTypeInfo().GetDeclaredMethod(args[1]);
                // Create a delegate object that wraps the stati method 
                d = mi.CreateDelegate(delType);
            }
            catch (ArgumentException) {
                Console.WriteLine("Invalid methodName argument: " + args[1]);
                return;
            }
            // Create an array that will contain just the arguments to pass to the method via the 
            // delgate object 
            Object[] callbackArgs = new Object[args.Length - 2];
            if (d.GetType() == typeof(TwoInt32s)) {
                try {
                    // Convert the String arguments to Int32 arguments
                    for (Int32 a = 2; a < args.Length; a++)
                    {
                        callbackArgs[a - 2] = Int32.Parse(args[a]);
                    }
                }
                catch (FormatException){
                    Console.WriteLine("Parameters must be integers.");
                    return;
                }                 
            }
            if (d.GetType() == typeof(OneString)){
                // Just copy the string argument 
                Array.Copy(args, 2, callbackArgs, 0, callbackArgs.Length);
            }
            try {
                // Invoke the delegate and show the result 
                Object result = d.DynamicInvoke(callbackArgs);
                Console.WriteLine("Result =" + result);
            }
            catch (TargetParameterCountException) {
                Console.WriteLine("Incorrect number of parameters specified.");
            }
        }
        // This callback method takes 2 Int332 arguments
        private static Object Add(Int32 n1, Int32 n2) {
            return n1 + n2;
        }
        // This callback method takes 2 Int332 arguments
        private static Object Subtract(Int32 n1, Int32 n2) {
            return n1 - n2;
        }
        // This callback method takes 1 string argument
        private static Object NumChars(String s1) {
            return s1.Length;
        }
        // This callback method takes 1 string argument
        private static Object Reverse(String s1) {
            return new String(s1.Reverse().ToArray());
        }
    }
}

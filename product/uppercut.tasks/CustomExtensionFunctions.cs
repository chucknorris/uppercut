using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace uppercut.tasks
{
    [FunctionSet("uppercut", "uppercut")]
    public class CustomExtensionFunctions : FunctionSetBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExtensionFunctions"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="properties">The properties.</param>
        public CustomExtensionFunctions(Project project, PropertyDictionary properties)
            : base(project, properties)
        {
        }

        /// <summary>
        /// Determines whether the operating system is 64 bit.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the operating system is 64 bit; otherwise, <c>false</c>.
        /// </returns>
        [Function("is-os64bit")]
        public static bool IsOperatingSystem64Bit()
        {
            return IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor());
        }        
        
        [Function("is-process64bit")]
        public static bool IsProcess64Bit()
        {
            return IntPtr.Size == 8;
        }

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                {
                    return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(IsWow64ProcessDelegate));
                }
            }

            return null;
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

            if (fnDelegate == null)
            {
                return false;
            }

            bool isWow64;
            bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

            return retVal && isWow64;
        }
    }
}
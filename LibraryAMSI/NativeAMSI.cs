using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace LibraryAMSI.Native
{
    public class NativeAMSI
    {
        internal const string AMSIDllName = "Amsi.dll";

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiInitialize), CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiInitialize(
            string appName, out AMSIHandleContext amsiContext);

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiUninitialize), CallingConvention = CallingConvention.StdCall)]
        internal static extern void AmsiUninitialize(
            IntPtr amsiContext);

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiOpenSession), CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiOpenSession(
            AMSIHandleContext amsiContext, out AMSIHandleSession session);

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiCloseSession), CallingConvention = CallingConvention.StdCall)]
        internal static extern void AmsiCloseSession(
            AMSIHandleContext amsiContext,IntPtr session);

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiScanString), CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiScanString(
            AMSIHandleContext amsiContext, string @string, string contentName,AMSIHandleSession session, out int result);

        [DllImport(AMSIDllName, EntryPoint = nameof(AmsiScanBuffer), CallingConvention = CallingConvention.StdCall)]
        internal static extern int AmsiScanBuffer(
            AMSIHandleContext amsiContext, byte[] buffer, ulong length, string contentName, AMSIHandleSession session , out int result);

        internal static bool IsDllImportPossible()
        {
            try
            {
                Marshal.PrelinkAll(typeof(NativeAMSI));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    internal sealed class AMSIHandleSession : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal AMSIHandleContext Context { get; set; }
        public override bool IsInvalid => base.IsInvalid || Context.IsInvalid;

        internal AMSIHandleSession() : base(true)
        { }

        protected override bool ReleaseHandle()
        {
            NativeAMSI.AmsiCloseSession(Context, handle);
            return true;
        }
    }

    internal sealed class AMSIHandleContext : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal AMSIHandleContext() : base(true)
        { }

        protected override bool ReleaseHandle()
        {
            NativeAMSI.AmsiUninitialize(handle);
            return true;
        }
    }

    internal enum AMSIResult
    {
        AMSI_RESULT_CLEAN = 0,
        AMSI_RESULT_NOT_DETECTED = 1,
        AMSI_RESULT_BLOCKED_BY_ADMIN = 16384, // 16384 -  20479
        AMSI_RESULT_DETECTED = 32768
    }
}

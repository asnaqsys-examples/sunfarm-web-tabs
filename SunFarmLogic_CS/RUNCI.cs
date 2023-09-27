// Translated from Encore to C# on 9/27/2023 at 4:15:07 PM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QCLSRC, member RUNCI

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using System.Collections;
using System.Collections.Specialized;
using ASNA.QSys.Runtime.JobSupport;



namespace ACME.SunFarm
{
    [System.ComponentModel.Description("Run Customer Inquiry")]
    [ProgramEntry("_ENTRY")]
    public partial class RUNCI : CLProgram
    {
        protected dynamic _DynamicCaller;



        //------------------------------------------------------------------------------ 
        //  "*Entry" Mainline Code (Monarch generated)
        //------------------------------------------------------------------------------ 
        void StarEntry(int cparms)
        {
            Indicator _LR = '0';
            _INLR = '1';

            try
            {
                AddLibLEntry("ERCAP");
            }
            catch(CPF2103Exception) /* LIBRARY SO_AND_SO ALREADY IN LIBL */
            {
            }
            _DynamicCaller.CallD("ACME.SunFarm.CUSTINQ", out _LR);
        }

#region Entry and activation methods for *ENTRY

        int _parms;

        void __ENTRY()
        {
            int cparms = 0;
            bool _cleanup = true;
            try
            {
                _parms = cparms;
                StarEntry(cparms);
            }
            catch(Return)
            {
            }
            catch(System.Threading.ThreadAbortException)
            {
                _cleanup = false;
                _INLR = '1';
            }
            finally
            {
                _ = _cleanup;
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR)
        {
            __inLR = RunProgram<RUNCI>(_caller, (RUNCI _instance) => _instance.__ENTRY());
        }
#endregion

        public RUNCI()
        {
            _instanceInit();
        }

        void _instanceInit()
        {
            _DynamicCaller = new DynamicCaller(this);
        }
    }

}

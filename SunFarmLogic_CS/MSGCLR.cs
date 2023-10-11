// Translated from Encore to C# on 10/11/2023 at 10:10:23 AM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QCLSRC, member MSGCLR

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using System.Collections;
using System.Collections.Specialized;
using ASNA.QSys.Runtime.JobSupport;



namespace ACME.SunFarm
{
    [System.ComponentModel.Description("Clear program messages")]
    [ProgramEntry("_ENTRY")]
    public partial class MSGCLR : CLProgram
    {



        //------------------------------------------------------------------------------ 
        //  "*Entry" Mainline Code (Monarch generated)
        //------------------------------------------------------------------------------ 
        void StarEntry(int cparms)
        {
            _INLR = '1';

            RemoveMessage("*ALL");
            return;


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
            __inLR = RunProgram<MSGCLR>(_caller, (MSGCLR _instance) => _instance.__ENTRY());
        }
#endregion
    }

}

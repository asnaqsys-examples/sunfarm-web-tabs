// Translated from Encore to C# on 9/27/2023 at 4:15:07 PM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QCLSRC, member MSGLOD

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using System.Collections;
using System.Collections.Specialized;
using ASNA.QSys.Runtime.JobSupport;



namespace ACME.SunFarm
{
    [System.ComponentModel.Description("Write messages to the msg subfile")]
    [ProgramEntry("_ENTRY")]
    public partial class MSGLOD : CLProgram
    {


        FixedString<_7> _MSGID;
        FixedString<_30> _MSGTXT;

        //------------------------------------------------------------------------------ 
        //  "*Entry" Mainline Code (Monarch generated)
        //------------------------------------------------------------------------------ 
        void StarEntry(int cparms)
        {
            _INLR = '1';


            if (!_MSGID.IsBlanks())
            {
                if (((string)_MSGID).Substring(0, 3) == "CST")
                    SendProgramMessage(_MSGID, "CUSTMSGF", _MSGTXT);
                else
                    SendProgramMessage(_MSGID, "ITEMMSGF", _MSGTXT);
            }


        }

#region Entry and activation methods for *ENTRY

        int _parms;

        void __ENTRY(ref FixedString<_7> __MSGID, ref FixedString<_30> __MSGTXT)
        {
            int cparms = 2;
            bool _cleanup = true;
            _MSGTXT = __MSGTXT;
            _MSGID = __MSGID;
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
                if (_cleanup)
                {
                    __MSGID = _MSGID;
                    __MSGTXT = _MSGTXT;
                }
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR, ref FixedString<_7> _MSGID, ref FixedString<_30> _MSGTXT)
        {
            FixedString<_7> _p_MSGID = _MSGID;
            FixedString<_30> _p_MSGTXT = _MSGTXT;
            __inLR = RunProgram<MSGLOD>(_caller, (MSGLOD _instance) => _instance.__ENTRY(ref _p_MSGID, ref _p_MSGTXT));
            _MSGID = _p_MSGID;
            _MSGTXT = _p_MSGTXT;
        }
#endregion
    }

}

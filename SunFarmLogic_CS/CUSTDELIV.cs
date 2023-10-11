// Translated from Encore to C# on 10/11/2023 at 10:10:23 AM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QRPGSRC, member CUSTDELIV

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using ASNA.QSys.Runtime.JobSupport;


namespace ACME.SunFarm
{
    [ActivationGroup("*DFTACTGRP"), System.ComponentModel.Description("Customer Subfile Delivey Inquiry")]
    [ProgramEntry("_ENTRY")]
    public partial class CUSTDELIV : Program
    {

        //********************************************************************
        // JB   8/31/2004   Created.

        //********************************************************************
        DatabaseFile CUSTOMERL1;
        DatabaseFile CAMASTER;
        WorkstationFile CUSTDELIV_File;
        //********************************************************************
        // Customer DS
        DataStructure CUSTDS;

        //********************************************************************

        // Fields defined in main C-Specs declared now as Global fields (Monarch generated)
        FixedDecimal<_9, _0> pNumber;
        FixedDecimal<_4, _0> savrrn;
        FixedDecimal<_4, _0> sflrrn;

#region Constructor and Dispose 
        public CUSTDELIV()
        {
            _instanceInit();
            CAMASTER.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            CUSTOMERL1.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                CUSTOMERL1.Close();
                CAMASTER.Close();
                CUSTDELIV_File.Close();
            }
            base.Dispose(disposing);
        }


#endregion
        void StarEntry(int cparms)
        {
            int _RrnTmp = 0;
            do
            {
                while (!(bool)_IN[12])
                {
                    _IN[90] = '0';
                    CUSTDELIV_File.Write("KEYS", _IN.Array);
                    CUSTDELIV_File.ExFmt("SFLC", _IN.Array);
                    // exit
                    if ((bool)_IN[12])
                    {
                        _INLR = '1';
                        break;
                    }
                    _IN[30] = '0';
                    savrrn = sflrrn;
                    _RrnTmp = (int)sflrrn;
                    _IN[40] = CUSTDELIV_File.ReadNextChanged("SFL1", ref _RrnTmp, _IN.Array) ? '0' : '1';
                    sflrrn = _RrnTmp;
                    while (!(bool)_IN[40])
                    {
                        if (SFLSEL == "1")
                        {
                            CUSTDELIV_File.ChainByRRN("SFL1", (int)sflrrn, _IN.Array);
                            Something();
                            SFLSEL = "";
                            CUSTDELIV_File.Update("SFL1", _IN.Array);
                        }
                        _RrnTmp = (int)sflrrn;
                        _IN[40] = CUSTDELIV_File.ReadNextChanged("SFL1", ref _RrnTmp, _IN.Array) ? '0' : '1';
                        sflrrn = _RrnTmp;
                    }
                    sflrrn = savrrn;
                }
                //****************************************

                //****************************************
            } while (!(bool)_INLR);
        }
        void Something()
        {
        }
        //**********************
        //  Load Sfl Subroutine
        //**********************
        void LoadSfl()
        {
            _IN[99] = CAMASTER.ReadNextEqual(true, pNumber) ? '0' : '1';
            while (!(bool)_IN[99])
            {
                SFLCUSTh = CACUSTNO;
                SFLCUST = (string)CANAME;
                SFLCITY = CACITY.TrimEnd() + ", " + CASTATE;
                SFLZIP = (string)CAZIP;
                sflrrn = sflrrn + 1;
                CUSTDELIV_File.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
                _IN[99] = CAMASTER.ReadNextEqual(true, pNumber) ? '0' : '1';
            }
            // end of file
            if (sflrrn == 0m)
            {
                sflrrn = sflrrn + 1;
                CMCUSTNO = 0m;
                SFLCUST = "No Address Records Found";
                SFLCITY = "";
                SFLZIP = "";
                CUSTDELIV_File.WriteSubfile("SFL1", (int)sflrrn, _IN.Array);
            }
        }
        //*********************
        // Init Subroutine
        //*********************
        void PROCESS_STAR_INZSR()
        {
            sflrrn = 0m;
            _IN[90] = '1';
            CUSTDELIV_File.Write("SFLC", _IN.Array);
            CAMASTER.Seek(SeekMode.SetLL, pNumber);
            LoadSfl();
        }

#region Entry and activation methods for *ENTRY

        int _parms;

        bool _isInitialized;
        void __ENTRY(ref FixedDecimal<_9, _0> _pNumber)
        {
            int cparms = 1;
            bool _cleanup = true;
            pNumber = _pNumber;
            try
            {
                _parms = cparms;
                if (!_isInitialized)
                {
                    PROCESS_STAR_INZSR();
                    _isInitialized = true;
                }
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
                    _pNumber = pNumber;
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR, ref FixedDecimal<_9, _0> pNumber)
        {
            FixedDecimal<_9, _0> _ppNumber = pNumber;
            __inLR = RunProgram<CUSTDELIV>(_caller, (CUSTDELIV _instance) => _instance.__ENTRY(ref _ppNumber));
            pNumber = _ppNumber;
        }
#endregion

        void _instanceInit()
        {
            CUSTOMERL1 = new DatabaseFile(PopulateBufferCUSTOMERL1, PopulateFieldsCUSTOMERL1, null, "CUSTOMERL1", "*LIBL/CUSTOMERL1", CUSTOMERL1FormatIDs)
            { IsDefaultRFN = true };
            CAMASTER = new DatabaseFile(PopulateBufferCAMASTER, PopulateFieldsCAMASTER, null, "CAMASTER", "*LIBL/CAMASTER", CAMASTERFormatIDs)
            { IsDefaultRFN = true };
            CUSTDELIV_File = new WorkstationFile(PopulateBufferCUSTDELIV_File, PopulateFieldsCUSTDELIV_File, null, "CUSTDELIV_File", "/SunFarmViews/CUSTDELIV");
            CUSTDELIV_File.Open();
            CUSTDS = new (extSizeCUSTDS, CUSTDS_000, CUSTDS_001, CUSTDS_002, CUSTDS_003, CUSTDS_004, CUSTDS_005, CUSTDS_006, CUSTDS_007, 
                CUSTDS_008, CUSTDS_009, CUSTDS_010, CUSTDS_011, CUSTDS_012, CUSTDS_013, CUSTDS_014, CUSTDS_015);
        }
    }

}

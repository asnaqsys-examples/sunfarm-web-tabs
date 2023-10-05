// Translated from Encore to C# on 10/5/2023 at 10:57:08 AM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QRPGSRC, member CUSTPRTS

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using ASNA.QSys.Runtime.JobSupport;


namespace ACME.SunFarm
{
    [ActivationGroup("*DFTACTGRP"), System.ComponentModel.Description("Print a Customer\'s Sales/Credits Details")]
    [ProgramEntry("_ENTRY")]
    public partial class CUSTPRTS : Program
    {

        DatabaseFile CSMASTERL1;
        DatabaseFile CUSTOMERL1;
        // DclPrintFile QPRINT DB(MyJob.MyPrinterDB) File("REPORTS\CUSTPRTS") UsesPageFlds(*Yes) ImpOpen(*No) OverflowInd(*InOF) 
        internal const decimal QPRINT_PrintLineHeight = 50; // Notes: Units are LOMETRIC (one hundredth of a centimeter). The constant used came from the Global Directive defaults.
        //********************************************************************
        //   U1 Print sales
        //   U2 Print credits
        //********************************************************************
        DataStructure pNumberAlf = new (9);
        FixedDecimal<_9, _0> pNumber { get => pNumberAlf.GetZoned(0, 9, 0); set => pNumberAlf.SetZoned(value, 0, 9, 0); } 

        FixedDecimal<_7, _0> wCount;
        FixedDecimal<_4, _0> wPrevYr;
        FixedDecimal<_4, _0> wPrtYr;
        FixedString<Len<_1, _2, _0>> wUnderline;
        short X;
        DataStructure CUSTSL;
        FixedDecimalArrayInDS<_12, _11, _2> SlsArray; 

        DataStructure _DS3 = new (429);
        FixedString<_10> sUserId { get => _DS3.GetString(253, 10); set => _DS3.SetString((string)value, 253, 10); } 

        //**********************************************************************
        //**********************************************************************
#region Constructor and Dispose 
        public CUSTPRTS()
        {
            _instanceInit();
            CSMASTERL1.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            CUSTOMERL1.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            // QPRINT.Printer = "Microsoft Print to PDF"
            // QPRINT.ManuscriptPath = Spooler.GetNewFilePath(QPRINT.DclPrintFileName)
            // Open QPRINT DB(CurrentJob.PrinterDB)
            // Open QPRINT DB(CurrentJob.PrinterDB)

#region Populate Program Status Data Structure
            sUserId = CurrentJob.PsdsJobUser.ToUpper();
#endregion
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                CSMASTERL1.Close();
                CUSTOMERL1.Close();
            }
            base.Dispose(disposing);
        }


#endregion
        void StarEntry(int cparms)
        {
            do
            {
                //----------------------------------------------------------------------
                _IN[80] = CUSTOMERL1.Chain(true, pNumber) ? '0' : '1';
                if ((bool)_IN[80])
                    CMNAME = "????????".MoveLeft(CMNAME);
                // Write PrtHeading
                CSMASTERL1.Seek(SeekMode.SetLL, pNumber);
                _INLR = CSMASTERL1.ReadNextEqual(true, pNumber) ? '0' : '1';
                //----------------------------------------------------------------------
                while (_INLR == '0')
                {
                    if (CSYEAR == wPrevYr)
                    {
                        wPrtYr = 0m;
                    }
                    else
                    {
                        wPrtYr = CSYEAR;
                        wPrevYr = CSYEAR;
                    }
                    ChkTheInfo();
                    _INLR = CSMASTERL1.ReadNextEqual(true, pNumber) ? '0' : '1';
                }
                //----------------------------------------------------------------------
                // Write PrtCount
                //**********************************************************************
                //**********************************************************************
            } while (!(bool)_INLR);
        }
        void ChkTheInfo()
        {
            for (X = 1; X <= 12; X++)
            {
                if (CurrentJob.GetSwitch(1) == '0' && (SlsArray[(int)X - 1] > 0m))
                {
                    /*  Don't print sales */
                    SlsArray[(int)X - 1] = 0m;
                }
                if (CurrentJob.GetSwitch(2) == '0' && (SlsArray[(int)X - 1] < 0m))
                {
                    /*  Don't print credits */
                    SlsArray[(int)X - 1] = 0m;
                }
            }
            // IS THERE ANYTHING TO PRINT? -----------------------------------------
            for (X = 1; X <= 12; X++)
            {
                if (SlsArray[(int)X - 1] != 0m)
                {
                    // Write PrtDetail
                    wCount = wCount + 1;
                    break;
                }
            }
        }
        //**********************************************************************
        //**********************************************************************

#region Entry and activation methods for *ENTRY

        int _parms;

        void __ENTRY(ref object _pNumberAlf)
        {
            int cparms = 1;
            bool _cleanup = true;
            if (_pNumberAlf != null)
            {
                if (_pNumberAlf is IMODS)
                    pNumberAlf.Load((_pNumberAlf as IMODS).DumpAll());
                else if (_pNumberAlf is IDS)
                    pNumberAlf.Load((_pNumberAlf as IDS).Dump());
                else
                    pNumberAlf.Load(_pNumberAlf.ToString());
            }
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
                if (_cleanup && _pNumberAlf != null)
                {
                    if (_pNumberAlf is IMODS)
                        (_pNumberAlf as IMODS).LoadAll(pNumberAlf.Dump());
                    else if (_pNumberAlf is IDS)
                        (_pNumberAlf as IDS).Load(pNumberAlf.Dump());
                    else
                        _pNumberAlf = pNumberAlf.Dump();
                }
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR, ref object pNumberAlf)
        {
            object _ppNumberAlf;
            _ppNumberAlf = pNumberAlf;
            __inLR = RunProgram<CUSTPRTS>(_caller, (CUSTPRTS _instance) => _instance.__ENTRY(ref _ppNumberAlf));
            pNumberAlf = _ppNumberAlf;
        }
#endregion

        static CUSTPRTS()
        {
        }

        void _instanceInit()
        {
            X = 0;
            wUnderline = new string('-', 120);
            wPrtYr = 9999;
            wPrevYr = 9999;
            wCount = 0m;
            CSMASTERL1 = new DatabaseFile(PopulateBufferCSMASTERL1, PopulateFieldsCSMASTERL1, null, "CSMASTERL1", "*LIBL/CSMASTERL1", CSMASTERL1FormatIDs)
            { IsDefaultRFN = true };
            CUSTOMERL1 = new DatabaseFile(PopulateBufferCUSTOMERL1, PopulateFieldsCUSTOMERL1, null, "CUSTOMERL1", "*LIBL/CUSTOMERL1", CUSTOMERL1FormatIDs)
            { IsDefaultRFN = true };
            CUSTSL = new (extSizeCUSTSL, CUSTSL_000, CUSTSL_001, CUSTSL_002, CUSTSL_003, CUSTSL_004, CUSTSL_005, CUSTSL_006, CUSTSL_007, 
                CUSTSL_008, CUSTSL_009, CUSTSL_010, CUSTSL_011, CUSTSL_012, CUSTSL_013, CUSTSL_014, (Layout.PackedArray(12, 11, 2), 10));
            SlsArray = new FixedDecimalArrayInDS<_12, _11, _2>(CUSTSL, LayoutType.Packed, 10);
        }
    }

}

// Translated from Encore to C# on 10/11/2023 at 10:10:24 AM by ASNA Encore Translator® version 4.0.18.0
// ASNA Monarch(R) version 11.4.12.0 at 8/25/2023
// Migrated source location: library ERCAP, file QRPGSRC, member ORDPRINT

using ASNA.QSys.Runtime;
using ASNA.DataGate.Common;
using ACME.SunFarmCustomers_Job;

using System;
using ASNA.QSys.Runtime.JobSupport;


namespace ACME.SunFarm
{
    [ActivationGroup("*DFTACTGRP"), System.ComponentModel.Description("Print a Customer\'s Order Details")]
    [ProgramEntry("_ENTRY")]
    public partial class ORDPRINT : Program
    {

        DatabaseFile CUSTOMERL1;
        DatabaseFile ITEMMASTL1;
        DatabaseFile ORDERHDRL2;
        DatabaseFile ORDERLINL2;
        DatabaseFile SHIPPING;
        // DclPrintFile QPRINT DB(MyJob.MyPrinterDB) File("REPORTS\ORDPRINT") UsesPageFlds(*Yes) ImpOpen(*No) OverflowInd(*InOF) 
        internal const decimal QPRINT_PrintLineHeight = 50; // Notes: Units are LOMETRIC (one hundredth of a centimeter). The constant used came from the Global Directive defaults.
        //********************************************************************
        FixedDecimalArray<_20, _3, _0> wShipCode;
        FixedStringArray<_20, _30> wShipDesc;
        FixedDecimal<_5, _0> wCountLne;
        FixedString<_50> wCUSTNO;
        FixedDate<_USA, _Default> wDateUSA1;
        FixedDate<_USA, _Default> wDateUSA2;
        FixedDecimal<_5, _0> wITEMNUM;
        FixedString<_4> wLINNUM;
        FixedString<_13> wORDAMOUNT;
        FixedString<_9> wORDNBR;
        FixedString<_40> wPRTADDR;
        FixedDecimal<_9, _4> wEXTAMT;
        FixedDecimal<_9, _2> wTempAmt;
        DataStructure _DS9 = new (429);
        FixedString<_10> wUserId { get => _DS9.GetString(253, 10); set => _DS9.SetString((string)value, 253, 10); } 

        //********************************************************************
        // Fields defined in main C-Specs declared now as Global fields (Monarch generated)
        FixedDecimal<_9, _0> pCUSTNO;
        FixedDecimal<_9, _0> pORDNBR;

        // KLIST(s) relocated by Monarch
        
        
#region Constructor and Dispose 
        public ORDPRINT()
        {
            _instanceInit();
            CUSTOMERL1.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            ITEMMASTL1.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            ORDERHDRL2.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            ORDERLINL2.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            SHIPPING.Open(CurrentJob.Database, AccessMode.Read, false, false, ServerCursors.Default);
            // QPRINT.Printer = "Microsoft Print to PDF"
            // QPRINT.ManuscriptPath = Spooler.GetNewFilePath(QPRINT.DclPrintFileName)
            // Open QPRINT DB(CurrentJob.PrinterDB)

#region Populate Program Status Data Structure
            wUserId = CurrentJob.PsdsJobUser.ToUpper();
#endregion
        }

        override public void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                CUSTOMERL1.Close();
                ITEMMASTL1.Close();
                ORDERHDRL2.Close();
                ORDERLINL2.Close();
                SHIPPING.Close();
            }
            base.Dispose(disposing);
        }


#endregion
        void StarEntry(int cparms)
        {
            do
            {
                //----------------------------------------------------------------------
                // KLIST "KeyHeadL2" moved by Monarch to global scope.
                // KLIST "KeyLineL2" moved by Monarch to global scope.
                //----------------------------------------------------------------------
                _IN[66] = CUSTOMERL1.Chain(true, pCUSTNO) ? '0' : '1';
                wCUSTNO = EditCode.Apply(pCUSTNO, 0, 9, EditCodes.Z, "").Trim() + " " + CMNAME;
                wORDNBR = EditCode.Apply(pORDNBR, 0, 9, EditCodes.Z, "").Trim();
                // Write PrtNmeLine
                if (!CMADDR1.IsBlanks())
                {
                    wPRTADDR = (string)CMADDR1;
                    // Write PrtAdrLine
                }
                if (!CMADDR2.IsBlanks())
                {
                    wPRTADDR = (string)CMADDR2;
                    // Write PrtAdrLine
                }
                if (!CMCITY.IsBlanks())
                {
                    wPRTADDR = ((string)CMCITY).Trim() + ", " + CMSTATE + " " + CMPOSTCODE;
                    // Write PrtAdrLine
                }
                wPRTADDR = "";
                // Write PrtAdrLine
                //----------------------------------------------------------------------
                _IN[66] = ORDERHDRL2.Chain(true, pCUSTNO, pORDNBR) ? '0' : '1';
                wDateUSA1 = (DateTime)ORDDATE;
                wDateUSA2 = (DateTime)ORDSHPDATE;
                wTempAmt = (decimal)ORDAMOUNT;
                wORDAMOUNT = "$" + EditCode.Apply(wTempAmt, 2, 9, EditCodes.Three, "").Trim();
                _IN[66] = SHIPPING.ChainByRRN(true, (int)ORDSHPVIA) ? '0' : '1';
                if ((bool)_IN[66])
                    CARRIERDES = "UNKNOWN";
                // Write PrtHdrLine
                //----------------------------------------------------------------------
                ORDNUMBER = pORDNBR;
                ORDLINNUM = 0m;
                ORDERLINL2.Seek(SeekMode.SetLL, ORDNUMBER, ORDLINNUM);
                _INLR = ORDERLINL2.ReadNextEqual(true, ORDNUMBER) ? '0' : '1';
                while (_INLR == '0')
                {
                    //EOF?
                    _IN[66] = ITEMMASTL1.Chain(true, ORDITEMNUM) ? '0' : '1';
                    wLINNUM = EditCode.Apply(ORDLINNUM, 0, 9, EditCodes.Z, "").Trim();
                    wITEMNUM = (decimal)ORDITEMNUM;
                    wEXTAMT = ORDLQTY * ITEMPRICE;
                    Write_QPRINT_PrtDtlLine();
                    wCountLne = wCountLne + 1;
                    _INLR = ORDERLINL2.ReadNextEqual(true, ORDNUMBER) ? '0' : '1';
                }
                // Write PrtTotLine
            } while (!(bool)_INLR);
        }
        //***************************************************************
#region Except to Names with conditional indicators, Fetch overflow or Blank-After fields (Monarch generated)
        void Write_QPRINT_PrtDtlLine()
        {
        }
#endregion

#region Entry and activation methods for *ENTRY

        int _parms;

        void __ENTRY(ref FixedDecimal<_9, _0> _pCUSTNO, ref FixedDecimal<_9, _0> _pORDNBR)
        {
            int cparms = 2;
            bool _cleanup = true;
            pORDNBR = _pORDNBR;
            pCUSTNO = _pCUSTNO;
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
                    _pCUSTNO = pCUSTNO;
                    _pORDNBR = pORDNBR;
                }
            }
        }

        public static void _ENTRY(ICaller _caller, out Indicator __inLR, ref FixedDecimal<_9, _0> pCUSTNO, ref FixedDecimal<_9, _0> pORDNBR)
        {
            FixedDecimal<_9, _0> _ppCUSTNO = pCUSTNO;
            FixedDecimal<_9, _0> _ppORDNBR = pORDNBR;
            __inLR = RunProgram<ORDPRINT>(_caller, (ORDPRINT _instance) => _instance.__ENTRY(ref _ppCUSTNO, ref _ppORDNBR));
            pCUSTNO = _ppCUSTNO;
            pORDNBR = _ppORDNBR;
        }
#endregion

        static ORDPRINT()
        {
        }

        void _instanceInit()
        {
            wCountLne = 0m;
            wShipDesc = new FixedStringArray<_20, _30>((FixedString<_30>[])null);
            wShipDesc.Array.Initialize("Unknown");
            wShipCode = new FixedDecimalArray<_20, _3, _0>((FixedDecimal<_3, _0>[])null);
            wShipCode.Array.Initialize(0m);
            CUSTOMERL1 = new DatabaseFile(PopulateBufferCUSTOMERL1, PopulateFieldsCUSTOMERL1, null, "CUSTOMERL1", "*LIBL/CUSTOMERL1", CUSTOMERL1FormatIDs)
            { IsDefaultRFN = true };
            ITEMMASTL1 = new DatabaseFile(PopulateBufferITEMMASTL1, PopulateFieldsITEMMASTL1, null, "ITEMMASTL1", "*LIBL/ITEMMASTL1", ITEMMASTL1FormatIDs)
            { IsDefaultRFN = true };
            ORDERHDRL2 = new DatabaseFile(PopulateBufferORDERHDRL2, PopulateFieldsORDERHDRL2, null, "ORDERHDRL2", "*LIBL/ORDERHDRL2", ORDERHDRL2FormatIDs)
            { IsDefaultRFN = true };
            ORDERLINL2 = new DatabaseFile(PopulateBufferORDERLINL2, PopulateFieldsORDERLINL2, null, "ORDERLINL2", "*LIBL/ORDERLINL2", ORDERLINL2FormatIDs)
            { IsDefaultRFN = true };
            SHIPPING = new DatabaseFile(PopulateBufferSHIPPING, PopulateFieldsSHIPPING, null, "SHIPPING", "*LIBL/SHIPPING", SHIPPINGFormatIDs, isKeyed : false)
            { IsDefaultRFN = true };
        }
    }

}
